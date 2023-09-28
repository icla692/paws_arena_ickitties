using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.colorfulcoding.AfterGame
{
    public class AfterGameCharacter : MonoBehaviour
    {
        public Animator animator;

        private void Start()
        {
            StartCoroutine(CharacterAnimationCoroutine());
        }

        private IEnumerator CharacterAnimationCoroutine()
        {
            int checkIfIWon = GameResolveStateUtils.CheckIfIWon(GameState.gameResolveState);
            yield return new WaitForSeconds(1.5f);
            if (checkIfIWon < 0)
            {
                animator.SetBool("isDead", true);
            }
        }
    }
}
