using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    public interface IChunkCollider
    {
        void UpdateColliders(List<Column> pixelData, ITextureSource textureSource);
    }
}