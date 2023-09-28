using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameText : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = GameState.nickname;
    }
}
