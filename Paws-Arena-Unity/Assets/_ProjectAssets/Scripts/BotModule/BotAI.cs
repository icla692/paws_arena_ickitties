using Anura.ConfigurationModule.Managers;
using Anura.ConfigurationModule.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static BotAIAim;

public class BotAI : MonoBehaviour
{
    public class LocationSim
    {
        public Location location;
        public float score;
        public float angle;
        public float power;
        public float distanceToEnemy;
        public bool directHit;
        public bool simulated;
        public float eta;

        public LocationSim(Location location)
        {
            this.location = location;
            score = 0;
            angle = Random.Range(0, 360);
            power = Random.Range(0, 1f);
            distanceToEnemy = Mathf.Infinity;
            eta = Mathf.Infinity;
            directHit = false;
            simulated = false;
        }
    }

    public class Location
    {
        public int stepsFromOrigin;
        public Vector3 position;
        public Vector3 launchPosition;
        public int direction;  // leftOfOrigin=-1, origin=0, rightOfOrigin=1
        public int enemyDirection;  // leftOfOrigin=-1, rightOfOrigin=1
        public Dictionary<Weapon, LocationSim> locationSims = new Dictionary<Weapon, LocationSim>();

        public Location(BotAI ai, int stepsFromOrigin, int direction, Vector3 position, Vector3 launchPosition)
        {
            this.stepsFromOrigin = stepsFromOrigin;
            this.direction = direction;
            this.position = position;
            this.launchPosition = launchPosition;
            enemyDirection = ai.GetEnemyDirection(position);
        }
    }

    private BotPlayerAPI api = BotPlayerAPI.Instance;
    private float TimeLeft => 30;  // TODO: Get the actual time left for the bot's turn
    private BotConfiguration Configuration => BotManager.Instance.GetConfiguration();

    private Bounds Bounds => Collider.bounds;
    private Vector3 Center => Bounds.center;

    public float MapMinY { get; private set; }
    public float MapMaxY { get; private set; }
    public float MapXWidth { get; private set; }

    private Rigidbody2D Rigidbody { get; set; }
    private Collider2D Collider { get; set; }
    public Transform LaunchPoint { get; set; }

    private Vector3 EnemyPosition => BotManager.Instance.Enemy[0].transform.position;

    private float movementStep;

    private BotAIAim BotAIAim { get; set; }

    // For debugging, to be deleted
    #region Debugging
    private bool debug = false;
    private List<GameObject> debugGOs = new List<GameObject>();
    private void DebugLocation(Location l, float scale = 1)
    {
        if (!debug) return;

        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        g.transform.position = l.position;
        g.transform.localScale = Vector3.one * scale;
        Destroy(g.GetComponent<Collider>());
        debugGOs.Add(g);
    }

    private void DebugList<K>(List<K> list)
    {
        string s = "";
        for (int i = 0; i < list.Count; i++)
            s += list[i].ToString() + " ";
    }
    #endregion

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();

        MapMinY = BotManager.Instance.leftMapBound.bounds.min.y;
        MapMaxY = BotManager.Instance.leftMapBound.bounds.max.y;
        MapXWidth = BotManager.Instance.rightMapBound.bounds.min.x - BotManager.Instance.leftMapBound.bounds.max.x;

        BotAIAim = new BotAIAim(this, BotManager.Instance.Enemy.ToArray());

        previousHP = BotManager.Instance.GetHP();

        movementStep = Bounds.size.x * Configuration.stepLength;
    }

    private void OnEnable()
    {
        AreaEffectsManager.Instance.OnAreaDamage += AreaDamage;
    }

    private void OnDisable()
    {
        AreaEffectsManager.Instance.OnAreaDamage -= AreaDamage;
    }

    private bool playing = false;

    public void Play()
    {
        playing = true;
        foreach (var g in debugGOs) Destroy(g);
        debugGOs.Clear();

        StartCoroutine(PlayTurn());
    }

    public void Wait()
    {
        playing = false;
    }

    private Vector3? lastPlayerHit = null;
    private void AreaDamage(Vector2 position, float area, int maxDamage, bool damageByDistance, bool hasPushForce, float pushForce, int bulletCount)
    {
        if (playing) return;
        lastPlayerHit = position;
    }

    private Location chosenLocation = null;
    private Weapon chosenWeapon;

    private IEnumerator PlayTurn()
    {
        chosenLocation = null;

        List<bool> moveDirs = new List<bool> { true, true };
        int steps = Configuration.maxTravelSteps;
        float stepFactor = 1;
        
        int attempt = 0;

        do
        {
            List<Location> potentialLocations = GetPotentialLocations(moveDirs[0], moveDirs[1], steps, stepFactor);
            
            float startThinkingTime = Time.time;
            yield return StartCoroutine(ChooseLocation(potentialLocations));
            if (chosenLocation == null) break;
            
            // DebugLocation(chosenLocation);
            if (chosenLocation.direction == -1) moveDirs[0] = false;
            if (chosenLocation.direction == 1) moveDirs[1] = false;

            // Prepare in case of repeat
            if (!moveDirs.Contains(true))
            {
                if (attempt == 0)
                {
                    steps = 1;
                    stepFactor = 0.5f;
                    moveDirs = new List<bool> { true, true };
                }

                attempt++;
            }

            // Respect minimum thinking time (humanity)
            float thinkingTime = Time.time - startThinkingTime;
            float requiredThinkingTime = GetRandomizedHumanityTime(Configuration.minThinkingTime);
            
            if (requiredThinkingTime > Configuration.maxThinkingTime)
                requiredThinkingTime = Configuration.maxThinkingTime;
            if (thinkingTime < requiredThinkingTime)
                yield return new WaitForSeconds(requiredThinkingTime - thinkingTime);

            yield return StartCoroutine(Move());
        } while (repeatMoveTo && TimeLeft > Configuration.ActionTimeTotal);
        

        yield return StartCoroutine(Shoot());

        previousHP = BotManager.Instance.GetHP();
    }

    private float GetRandomizedHumanityTime(float time)
    {
        return time * Random.Range(1, Configuration.HumanityTimesRandomizerMultiplier);
    }

    private List<Location> GetPotentialLocations(bool left, bool right, int steps, float stepFactor)
    {
        var result = new List<Location> { GetLocation() };

        for (int i = 1; i <= steps; i++)
        {
            if (left) AddLocationIfNew(ref result, GetLocation(-1, i, stepFactor));
            if (right) AddLocationIfNew(ref result, GetLocation(1, i, stepFactor));
        }

        // foreach (var l in result) DebugLocation(l, 0.5f);

        return result;
    }

    private void AddLocationIfNew(ref List<Location> list, Location location)
    {
        foreach (var l in list)
        {
            if (Vector3.Distance(l.position, location.position) < Configuration.movementArrivalDistance)
                return;
        }

        list.Add(location);
    }

    private IEnumerator ChooseLocation(List<Location> locations)
    {
        BotAIAim.StartSimulation(locations);
        yield return new WaitWhile(() => BotAIAim.Simulating);
        locations = BotAIAim.GetSimulationResults();

        for (int i = 0; i < locations.Count; i++)
        {
            EvaluateLocationScores(locations[i]);
        }

        DecideLocation(locations);
    }

    private void DecideLocation(List<Location> locations)
    {
        List<KeyValuePair<Weapon, LocationSim>> sims = new List<KeyValuePair<Weapon, LocationSim>>();
        foreach (Location l in locations)
            sims.AddRange(l.locationSims.Where(x => x.Value.simulated));

        var ordered = sims.OrderByDescending(x => x.Value.score).Take(Configuration.locationDecidingConsiderTopChoices);
        float marginScore = ordered.First().Value.score * (1 - Configuration.locationDecidingMargin);

        List<Weapon> weightedList = new List<Weapon>();

        foreach (var entry in ordered.Where(x => x.Value.score >= marginScore))
        {
            for (int i = 0; i < BotAIAim.WeaponsData[entry.Key].weight; i++)
                weightedList.Add(entry.Key);
        }

        chosenWeapon = weightedList[Random.Range(0, weightedList.Count)];
        chosenLocation = ordered.First(x => x.Key == chosenWeapon).Value.location;
    }

    private int previousHP;

    private void EvaluateLocationScores(Location location)
    {
        foreach (var sim in location.locationSims)
        {
            if (!sim.Value.simulated) continue;

            float score = 0;

            if (Configuration.weightDirectHit > 0)
            {
                if (sim.Value.directHit) score += Configuration.weightDirectHit;
            }

            if (Configuration.weightBestShotDistance > 0)
            {
                score += Mathf.Lerp(Configuration.weightBestShotDistance, 0, Mathf.Clamp(sim.Value.distanceToEnemy, 0, MapXWidth) / MapXWidth);
            }

            if (Configuration.weightDirectionImportance > 0)
            {
                float s = Configuration.maxTravelSteps * location.enemyDirection;
                score += Mathf.InverseLerp(s, -s, location.stepsFromOrigin) * Configuration.weightDirectionImportance;
            }

            if (Configuration.weightHeightImportance > 0)
            {
                float heightFraction = Mathf.InverseLerp(MapMinY, MapMaxY, location.position.y);
                score += Mathf.Lerp(0, Configuration.weightHeightImportance, heightFraction);
            }

            if (Configuration.weightMoveAwayFromPlayerShots > 0 && lastPlayerHit is Vector3 lph)
            {
                float add =                
                    Configuration.weightMoveAwayFromPlayerShots *
                    Mathf.InverseLerp(0, Configuration.maxTravelSteps * Configuration.stepLength, Vector3.Distance(location.position, lph));
                score += add;
            }

            sim.Value.score = score / Configuration.WeightsTotal;
        }        
    }

    private bool repeatMoveTo = false;

    private IEnumerator Move()
    {
        yield return null;
        yield return null;
        if (chosenLocation == null) yield break;

        repeatMoveTo = false;

        api.Move(chosenLocation.direction);

        int counter = 0;

        float stuckTime = Time.time;
        Vector3 stuckPos = Center;
        
        while (Mathf.Abs(Center.x - chosenLocation.position.x) > Configuration.movementArrivalDistance)
        {            
            yield return null;

            if (counter % Configuration.movementUpdateFrames == 0)
            {
                // Do not approach enemy
                if (Configuration.approachEnemy > 0 && chosenLocation.direction == GetEnemyDirection(Center))
                {
                    if (Vector3.Distance(Center, EnemyPosition) <= Configuration.approachEnemy * Bounds.size.x)
                    {
                        repeatMoveTo = true;
                        break;
                    }
                }

                // Change direction
                if ((chosenLocation.position.x - Center.x) * chosenLocation.direction < 0)
                {
                    chosenLocation.direction *= -1;
                    api.CancelMove();
                    yield return null;
                    api.Move(chosenLocation.direction);
                }
                else
                {
                    // Jump
                    if (Mathf.Abs(Rigidbody.velocity.x) < 0.01f)
                    {
                        api.Jump();
                    }
                }                
            }
            // Check if stuck
            else if (Time.time - stuckTime >= Configuration.movementStuckTime)
            {
                if (Vector3.Distance(Center, stuckPos) < Bounds.size.x)
                {
                    repeatMoveTo = true;
                    break;
                }
                stuckTime = Time.time;
                stuckPos = Center;
            }

            counter++;
        }

        api.CancelMove();
    }

    private IEnumerator Shoot()
    {
        if (chosenLocation == null) yield break;

        // Weapon selection
        yield return null;
        yield return new WaitForSeconds(GetRandomizedHumanityTime(Configuration.weaponPickingTime));

        api.weaponIdx = BotAIAim.WeaponsData[chosenWeapon].internalIndex;
        api.SelectWeapon();

        // Aiming
        yield return null;        
        float targetAngle = chosenLocation.locationSims[chosenWeapon].angle;        
        float targetPower = chosenLocation.locationSims[chosenWeapon].power;
        float accPenalty = GetPowerAccuracyPenalty(targetPower);
        targetPower += accPenalty;

        float totalTime = GetRandomizedHumanityTime(Configuration.aimingTime);

        if (totalTime > 0)
        {
            api.SelectAnglePower();
            float startTime = Time.time;
            yield return new WaitForSeconds(totalTime * Mathf.Clamp(Configuration.aimingThinkingFraction, 0, 1));

            float startAngle = targetAngle * Random.Range(0.5f, 1.5f);
            api.shootAngle = startAngle;
            float startPower = targetPower * Random.Range(0.3f, 1f);
            api.shootPower = startPower;
            api.SelectAnglePower();

            yield return null;

            while (true)
            {
                float elapsed = Time.time - startTime;
                float elapsedFraction = elapsed / totalTime;
                if (elapsedFraction >= 1) break;

                float logT = LogarithmicTimeInterpolation(elapsed, totalTime, 2, 12);
                api.shootAngle = Mathf.Lerp(startAngle, targetAngle, logT);
                api.shootPower = Mathf.Lerp(startPower, targetPower, logT);
                api.SelectAnglePower();

                yield return null;
            }
        }

        api.shootAngle = targetAngle;
        api.shootPower = targetPower;
        api.SelectAnglePower();

        // Shooting
        yield return null;
        yield return new WaitForSeconds(GetRandomizedHumanityTime(Configuration.shootingTime));
        
        api.Shoot();

        if (chosenWeapon == Weapon.Split)
        {
            yield return new WaitForSeconds(chosenLocation.locationSims[chosenWeapon].eta - 1);
            api.Shoot();
        }        
    }

    private float GetPowerAccuracyPenalty(float targetPower)
    {
        var preset = BotManager.Instance.GetPreset();
        if (preset) return preset.GetPowerAccuracyPenalty(targetPower);
        return 0;
    }

    private float LogarithmicTimeInterpolation(float time, float totalTime, float logBase, float logMaxY)
    {
        float maxX = Mathf.Pow(logBase, logMaxY);

        float x = Mathf.Lerp(1, maxX, Mathf.InverseLerp(0, totalTime, time));
        float y = Mathf.Log(x, logBase);

        return Mathf.InverseLerp(0, logMaxY, y);
    }

    private Location GetLocation()
    {
        return new Location(this, 0, 0, Center, LaunchPoint.position);
    }

    private Location GetLocation(int direction, int steps, float stepFactor)
    {
        float xOffset = steps * movementStep * stepFactor;

        Vector3 newPosition = TerrainNavigationLibrary.GetPositionAtXDisplacement(
                Bounds,
                GetTerrainDirection(direction),
                xOffset);

        return new Location(
            this,
            direction * steps,
            direction,
            newPosition,
            newPosition + (LaunchPoint.position - Center));
    }

    private int GetEnemyDirection(Vector3 pos)
    {
        float xDif = pos.x - EnemyPosition.x;
        if (xDif > 0) return -1;
        return 1;
    }

    private TerrainNavigationLibrary.Direction GetTerrainDirection(int d)
    {
        return d == -1 ? TerrainNavigationLibrary.Direction.Left : TerrainNavigationLibrary.Direction.Right;
    }
}