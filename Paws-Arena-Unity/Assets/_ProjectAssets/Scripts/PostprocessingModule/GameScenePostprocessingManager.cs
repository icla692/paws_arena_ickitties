using Anura.Templates.MonoSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameScenePostprocessingManager : MonoSingleton<GameScenePostprocessingManager>
{
    public Volume explosionLayer;
    public CameraShake cameraShake;

    public void EnableExplosionLayer(float duration)
    {
        cameraShake.setIsEnabled(true);
        LeanTween.value(0, 1, duration).setEaseOutBounce().setOnUpdate(val =>
        {
            explosionLayer.weight = val;
        }).setOnComplete(() =>
        {
            LeanTween.value(1, 0, duration).setEaseInBounce().setOnUpdate(val =>
            {
                explosionLayer.weight = val;
            }).setOnComplete(()=>
            {
                cameraShake.setIsEnabled(false);
            });
        });
    }
}
