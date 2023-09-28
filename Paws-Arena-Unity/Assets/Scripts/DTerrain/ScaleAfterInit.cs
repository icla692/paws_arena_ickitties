using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    public class ScaleAfterInit : MonoBehaviour
    {
        public BasicPaintableLayer paintableLayer;
        public Vector3 targetScale;
        private void Start()
        {
            StartCoroutine(ScaleToTargetAfterInit());    
        }

        private IEnumerator ScaleToTargetAfterInit()
        {
            while (!paintableLayer.isInit)
            {
                yield return null;
            }

            transform.localScale = targetScale;
        }
    }
}