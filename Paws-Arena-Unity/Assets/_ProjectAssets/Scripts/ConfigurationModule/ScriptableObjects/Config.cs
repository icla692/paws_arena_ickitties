using UnityEngine;

namespace Anura.ConfigurationModule.ScriptableObjects
{
    public enum GameType
    {
        MULTIPLAYER,
        SINGLEPLAYER,
        TUTORIAL
    }

    [CreateAssetMenu(fileName = "Config", menuName = "Configurations/Config", order = 1)]
    public class Config : ScriptableObject
    {
        [Header("Match Configuration")]
        [Space]

        [SerializeField]
        private GameType gameType;

        [SerializeField]
        private int maxNumberOfRounds = 15;
        [SerializeField]
        private int turnDurationInSeconds = 30;

        [Header("Player configurations")]
        [Space]

        [SerializeField]
        private int playerTotalHealth = 100;

        [SerializeField, Tooltip("Whether or not a player can steer while jumping")]
        private bool airControl = false;

        [SerializeField] private float playerSpeed;
        
        [SerializeField, Tooltip("Amount of force added when the player jumps.")]
        private float playerJumpForce;
        
        [Range(0, .3f), SerializeField, Tooltip("How much to smooth out the movement")] 
        private float movementSmoothing = .05f;

        [Header("Indicator configurations")]
        [Space]

        [SerializeField] private float indicatorSpeed;
        [SerializeField] private Vector2 bulletSpeed;
        [SerializeField] private float factorRotationRocket;
        [Range(2f, 12f), SerializeField] private float circleShootRadius = 5.5f;


        [Header("Chat configurations")]
        [Space]

        [SerializeField] private int numberOfLines;
        [SerializeField] private float chatLineHeight;
        [SerializeField] private float offsetHeightRefreshChat;

        [Header("Game Economy")]
        [Space]

        [SerializeField] private int betValue=25;

        public bool GetIsMultiplayer()
        {
            return gameType == GameType.MULTIPLAYER;
        }

        public GameType GetGameType()
        {
            return gameType;
        }
        public int GetMaxNumberOfRounds()
        {
            return maxNumberOfRounds;
        }

        public int GetTurnDurationInSeconds()
        {
            return turnDurationInSeconds;
        }

        public int GetPlayerTotalHealth()
        {
            return playerTotalHealth;
        }


        public bool GetAirControl()
        {
            return airControl;
        }
     
        public float GetPlayerSpeed()
        {
            return playerSpeed;
        }

        public float GetPlayerJumpForce()
        {
            return playerJumpForce;
        }

        public float GetMovementSmoothing()
        {
            return movementSmoothing;
        }


        //indicator

        public float GetIndicatorSpeed()
        {
            return indicatorSpeed;
        }

        public float GetBulletSpeed(float multiplier)
        {
            return multiplier * bulletSpeed.y;
        }

        public float GetFactorRotationIndicator()
        {
            return factorRotationRocket;
        }
        public float GetCircleShootRadius()
        {
            return circleShootRadius;
        }

        //chat

        public int GetNumberOfLines()
        {
            return numberOfLines;
        }

        public float GetChatLineHeight()
        {
            return chatLineHeight;
        }

        public float GetHeightRefreshingChat()
        {
            return (GetNumberOfLines() * GetChatLineHeight()) - offsetHeightRefreshChat;
        }

        //Economy

        public int GetBetValue()
        {
            return betValue;
        }
    }
}
