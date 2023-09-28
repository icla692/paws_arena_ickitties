using Anura.Templates.MonoSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoSingleton<TutorialManager>
{

    [Header("Data")]
    [TextArea]
    public List<string> messages;

    [Header("Elements")]
    public Image bg;
    public GameObject pascal;
    public GameObject pascalMainLocation;
    public GameObject pascalDownLocation;
    public GameObject pascalSecondaryPosition;
    public DummyBehaviour dummy;

    public List<GameObject> tutorialGameObjects;

    public GameObject textBox;
    public TMPro.TextMeshProUGUI textBoxContent;

    public Transform mainCanvas;
    public Transform overlayCanvas;

    public bool finished = false;

    [Header("Instructions")]
    public GameObject lowerLeftInstructions;
    public TMPro.TextMeshProUGUI lowerLeftInstructionsText;

    public GameObject upperRightInstructions;
    public TMPro.TextMeshProUGUI upperRightInstructionsText;
    public GameObject upperLeftCloseableInstructions;
    public TMPro.TextMeshProUGUI upperLeftCloseableInstructionsText;

    [Header("Stage 2")]
    public List<GameObject> stage2_objectsToActivate;
    public List<Transform> stage2_UIToHighlight;

    [Header("Stage 7")]
    public PlayerActionsBar playerActionsBar;
    public List<GameObject> weapons;
    public GameObject arrow_stage7;

    [Header("Surrender Stage")]
    public GameObject arrow_surrenderStage;
    public GameObject surrenderUI;
    public GameObject confirmationModal;
    public Button confirmationModalButton;
    public Button cancelModalButton;

    private bool enteredJumpCollider = false;
    private int idx = -1;
    private WeaponEntity crtWeapon;
    private int currentShots;

    private List<Transform> oldParents;
    private void Awake()
    {
        SetTutorialGameObjectsVisible(false);
    }

    private void Start()
    {
        oldParents = new List<Transform>();

        RoomStateManager.OnStateUpdated += OnStateUpdated;
    }

    private void OnDestroy()
    {
        RoomStateManager.OnStateUpdated -= OnStateUpdated;
    }

    private void OnStateUpdated(IRoomState state)
    {
        if (state is GamePausedState)
        {
            SetStage1();
        }
    }

    public void OnNext()
    {
        if (idx == 1)
        {
            idx++;
            SetStage2();
        }
        else if (idx == 2)
        {
            idx++;
            SetStage3();
        }
        else if (idx == 3)
        {
            idx = 3;
            StartCoroutine(SetStage4());
        }
        else if (idx == 6)
        {
            idx = 7;
            SetStage7FirstTime();
        }
    }

    private void SetStage1()
    {
        textBox.transform.localScale = Vector3.zero;
        SetTutorialGameObjectsVisible(true);
        LeanTween.scale(textBox, Vector3.one, 0.5f).setEaseOutBack();
        textBoxContent.text = messages[0];

        idx = 1;
    }

    private void SetStage2()
    {
        PlayerManager.Instance.DirectDamage(45);
        textBoxContent.text = messages[1];

        oldParents.Clear();
        foreach (Transform t in stage2_UIToHighlight)
        {
            oldParents.Add(t.parent);
            t.parent = overlayCanvas;
        }
        foreach (GameObject go in stage2_objectsToActivate)
        {
            go.SetActive(true);
        }
    }

    private void SetStage3()
    {
        textBoxContent.text = messages[2];
    }

    private IEnumerator SetStage4()
    {
        idx = 4;

        int i = 0;
        foreach (Transform t in stage2_UIToHighlight)
        {
            t.parent = oldParents[i++];
        }

        foreach (GameObject go in stage2_objectsToActivate)
        {
            go.SetActive(false);
        }

        textBox.SetActive(false);

        Color col = bg.color;
        LeanTween.value(bg.gameObject, 0.5f, 0f, 0.5f).setOnUpdate((val) =>
        {
            col.a = val;
            bg.color = col;
        }).setOnComplete(() =>
        {
            pascal.transform.parent = pascalDownLocation.transform;

            Vector3 initPos = pascal.transform.localPosition;
            Vector3 initScale = pascal.transform.localScale;

            LeanTween.value(pascal, 0, 1, 1.5f).setDelay(0.25f).setOnUpdate(val =>
            {
                pascal.transform.localPosition = initPos + (Vector3.zero - initPos) * val;
                pascal.transform.localScale = initScale + (Vector3.one - initScale) * val;
            }).setOnComplete(() =>
            {
                var scale = lowerLeftInstructions.transform.localScale;
                lowerLeftInstructions.transform.localScale = Vector3.zero;
                lowerLeftInstructions.SetActive(true);
                lowerLeftInstructionsText.text = messages[3];

                LeanTween.scale(lowerLeftInstructions, scale, 0.5f).setDelay(0.25f).setOnComplete(() =>
                {
                    CratesManager.Instance.SpawnCrate(new Vector3(31, 35), 100);
                    GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Enable();
                });
            });
        });




        while (!enteredJumpCollider)
        {
            yield return null;
        }

        SetStage5();
        yield return null;
    }

    private void SetStage5()
    {
        idx = 5;

        Vector3 initScale = lowerLeftInstructions.transform.localScale;
        LeanTween.scale(lowerLeftInstructions, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            lowerLeftInstructionsText.text = messages[4];
            LeanTween.scale(lowerLeftInstructions, initScale, 0.5f);
        });

        PlayerManager.Instance.onHealthUpdated += SetStage6;
    }

    private void SetStage6(int newHP)
    {
        idx = 6;
        PlayerManager.Instance.onHealthUpdated -= SetStage6;

        GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Disable();

        Color col = bg.color;
        LeanTween.value(bg.gameObject, 0, 0.5f, 0.5f).setOnUpdate((val) =>
        {
            col.a = val;
            bg.color = col;
        }).setOnComplete(() =>
        {
            pascal.transform.parent = pascalMainLocation.transform;
            Vector3 initPos = pascal.transform.localPosition;
            Vector3 initScale = pascal.transform.localScale;

            LeanTween.scale(lowerLeftInstructions, Vector3.zero, 0.5f).setOnComplete(() =>
            {
                lowerLeftInstructions.SetActive(false);
                LeanTween.value(pascal, 0, 1, 2f).setOnUpdate(val =>
                {
                    pascal.transform.localPosition = initPos + (Vector3.zero - initPos) * val;
                    pascal.transform.localScale = initScale + (Vector3.one - initScale) * val;
                }).setOnComplete(() =>
                {
                    textBox.SetActive(true);
                    textBox.transform.localScale = Vector3.zero;
                    textBoxContent.text = messages[5];
                    LeanTween.scale(textBox, Vector3.one, .5f);
                });

            });
        });

    }

    private void ResetStage7(int bulletCount)
    {
        dummy.onDummyHit -= SetStage9;
        dummy.onDummyMiss -= ResetStage7;

        upperRightInstructionsText.text = "Okay… That was a great shot, but let’s try and hit the target dummy this time.";

        SetStage7();
    }

    private void SetStage7FirstTime()
    {
        textBox.SetActive(false);

        GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Disable();

        LeanTween.scale(pascal, Vector3.zero, 2f).setEaseInBack().setOnComplete(() =>
        {
            SetStage7();

            upperRightInstructions.transform.localScale = Vector3.zero;
            upperRightInstructionsText.text = messages[6];
            upperRightInstructions.SetActive(true);

            LeanTween.scale(upperRightInstructions, Vector3.one, 1f).setEaseOutBack();
            pascal.transform.parent = pascalSecondaryPosition.transform;
            pascal.transform.localPosition = Vector3.zero;
            LeanTween.scale(pascal, Vector3.one, 1f).setEaseOutBack();
        });
    }
    private void SetStage7()
    {
        //GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Enable();

        Color col = bg.color;
        col.a = .5f;
        bg.color = col;

        arrow_stage7.SetActive(true);
        playerActionsBar.EnableWeaponsBar();

        oldParents.Clear();
        oldParents.Add(weapons[0].transform.parent);
        weapons[0].transform.parent = overlayCanvas;

        PlayerActionsBar.WeaponIndexUpdated += SetStage8;
    }

    private void SetStage8(int weaponIdx)
    {
        PlayerActionsBar.WeaponIndexUpdated -= SetStage8;
        GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Enable();

        Color col = bg.color;
        col.a = 0;
        bg.color = col;

        weapons[0].transform.parent = oldParents[0];
        arrow_stage7.SetActive(false);

        upperRightInstructionsText.text = messages[7];

        dummy.EnableDummy();
        dummy.onDummyHit += SetStage9;
        dummy.onDummyMiss += ResetStage7;
    }


    private void SetStage9(int bulletHits)
    {
        dummy.onDummyHit -= SetStage9;
        dummy.onDummyMiss -= ResetStage7;

        dummy.onDummyHit -= SetLastStage;
        dummy.onDummyMiss -= SetStage9IfAllMissed;

        StartCoroutine(Stage9Coroutine());
    }

    private IEnumerator Stage9Coroutine()
    {
        yield return new WaitForSeconds(2f);
        upperRightInstructionsText.text = messages[8];

        Color col = bg.color;
        col.a = 0.5f;
        bg.color = col;

        oldParents.Clear();
        for (int i = 1; i < weapons.Count; i++)
        {
            oldParents.Add(weapons[i].transform.parent);
            weapons[i].transform.parent = overlayCanvas;
        }

        PlayerActionsBar.WeaponIndexUpdated += SetStage9Phase2;
        PlayerThrowBehaviour.onLaunchPreparing += OnLaunchPreparing;
    }

    private void OnLaunchPreparing(WeaponEntity weapon)
    {
        crtWeapon = weapon;
    }

    private void SetStage9Phase2(int idx)
    {
        PlayerActionsBar.WeaponIndexUpdated -= SetStage9Phase2;

        GameInputManager.Instance.GetPlayerActionMap().GetPlayerActions().Enable();

        Color col = bg.color;
        col.a = 0f;
        bg.color = col;

        for (int i = 1; i < weapons.Count; i++)
        {
            weapons[i].transform.parent = oldParents[i-1];
        }

        currentShots = 0;
        dummy.onDummyHit += SetSurrenderStage;
        dummy.onDummyMiss += SetStage9IfAllMissed;
    }


    private void SetStage9IfAllMissed(int bulletCount)
    {
        currentShots+= bulletCount;
        if (currentShots >= crtWeapon.numberOfDamageDealers)
        {
            SetStage9(0);
        }
    }

    private void SetSurrenderStage(int bulletsHit)
    {
        currentShots = 0;
        dummy.onDummyHit -= SetSurrenderStage;
        dummy.onDummyMiss -= SetStage9IfAllMissed;

        Vector3 initScale = upperRightInstructions.transform.localScale;
        LeanTween.scale(upperRightInstructions, Vector3.zero, .5f).setOnComplete(() =>
        {
            LeanTween.scale(upperRightInstructions, initScale, .5f);

            Color col = bg.color;
            col.a = 0.5f;
            bg.color = col;

            oldParents.Clear();
            oldParents.Add(surrenderUI.transform.parent);
            surrenderUI.transform.parent = overlayCanvas;

            oldParents.Add(confirmationModal.transform.parent);
            confirmationModal.transform.parent = overlayCanvas;
            confirmationModalButton.interactable = false;
            cancelModalButton.onClick.AddListener(SetLastStage);

            arrow_surrenderStage.SetActive(true);
            upperRightInstructionsText.text = messages[9];
        });
    }

    private void SetLastStage(int bulletCount)
    {
        SetLastStage();
    }

    private void SetLastStage()
    {
        surrenderUI.transform.parent = oldParents[0];
        confirmationModal.transform.parent = oldParents[1];
        confirmationModalButton.interactable = true;
        cancelModalButton.onClick.RemoveListener(SetLastStage);
        arrow_surrenderStage.SetActive(false);

        Color col = bg.color;
        col.a = 0f;
        bg.color = col;

        upperRightInstructions.gameObject.SetActive(false);

        upperLeftCloseableInstructions.gameObject.SetActive(true);
        upperLeftCloseableInstructionsText.text = messages[10];

        dummy.onDummyHit -= SetLastStage;
        bg.raycastTarget = false;

        finished = true;
        RoomStateManager.Instance.SetState(new MyTurnState());

        dummy.onDummyDead += dummy.Relocate;
    }

    public void OnCloseUpperLeft()
    {
        upperLeftCloseableInstructions.SetActive(false);
        SetTutorialGameObjectsVisible(false);
    }

    private void SetTutorialGameObjectsVisible(bool value)
    {
        foreach (GameObject go in tutorialGameObjects)
        {
            go.SetActive(value);
        }
    }

    public void OnEnteredJumpCollider(Collider2D collider)
    {
        if (collider.gameObject.layer == 8)
        {
            enteredJumpCollider = true;
        }
    }
}
