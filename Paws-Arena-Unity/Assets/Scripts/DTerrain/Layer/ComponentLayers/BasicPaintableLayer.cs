using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    public class BasicPaintableLayer : PaintableLayer<PaintableChunk>
    {
        [HideInInspector]
        public bool isInit = false;
        //CHUNK SIZE X!!!!
        public virtual void Start()
        {
            SpawnChunks();
            InitChunks();
            isInit = true;
        }

        public virtual void Update()
        {
        }
    }

}
