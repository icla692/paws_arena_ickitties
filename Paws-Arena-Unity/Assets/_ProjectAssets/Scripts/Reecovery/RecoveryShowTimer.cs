using System.Collections;
using UnityEngine;
using TMPro;

public class RecoveryShowTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recoveryDisplay;

    private void OnEnable()
    {
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        while (gameObject.activeSelf)
        {
            if (GameState.selectedNFT.CanFight)
            {
                recoveryDisplay.text = string.Empty;
                yield return new WaitForSeconds(5);
            }
            else
            {
                int _minutes = GameState.selectedNFT.MinutesUntilHealed;
                if (_minutes!=0)
                {
                    recoveryDisplay.text = _minutes + "m";
                    yield return new WaitForSeconds(5);
                }
                else
                {
                    recoveryDisplay.text = (int)GameState.selectedNFT.TimeUntilHealed.TotalSeconds + "s";
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }
}
