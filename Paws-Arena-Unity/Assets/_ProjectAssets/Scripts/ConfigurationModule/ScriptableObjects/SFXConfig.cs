using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anura.ConfigurationModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SFXConfig", menuName = "Configurations/SFXConfig", order = 3)]
    public class SFXConfig : ScriptableObject
    {
        [SerializeField] private AudioClip coinSound;

        public AudioClip GetCoinSound()
        {
            return coinSound;
        }
    }
}
