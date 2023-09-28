using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphicsAnimationsEventThrow : MonoBehaviour
{
    public PlayerGraphicsBehaviour playerGraphicsBehaviour;
    public void PreJumpAnimEnded()
    {
        playerGraphicsBehaviour.PreJumpAnimEnded();
    }
    public void SetIsMidJump()
    {
        playerGraphicsBehaviour.SetIsMidJump();
    }
    public void AfterJump()
    {
        playerGraphicsBehaviour.AfterJump();
    }
}
