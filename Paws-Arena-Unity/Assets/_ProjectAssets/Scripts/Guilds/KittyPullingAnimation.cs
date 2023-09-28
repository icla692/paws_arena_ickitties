using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KittyPullingAnimation : MonoBehaviour
{
   [SerializeField] private Image body;
   [SerializeField] private Image eyes;
   [SerializeField] private Sprite[] bodySprites;
   [SerializeField] private Sprite[] eyesSprites;
   [SerializeField] private float delayBetweenSprites;
   [SerializeField] private float delayBetweenLoops;

   private int maxAmountOfSprites;

   private void Awake()
   {
      maxAmountOfSprites = bodySprites.Length;
   }

   private void OnEnable()
   {
      StopAllCoroutines();
      StartCoroutine(AnimationRoutine());
   }

   private IEnumerator AnimationRoutine()
   {
      while (true)
      {
         for (int _i = 0; _i < maxAmountOfSprites; _i++)
         {
            body.sprite = bodySprites[_i];
            eyes.sprite = eyesSprites[_i];
            yield return new WaitForSeconds(delayBetweenSprites);
         }

         yield return new WaitForSeconds(delayBetweenLoops);
      }
   }
}
