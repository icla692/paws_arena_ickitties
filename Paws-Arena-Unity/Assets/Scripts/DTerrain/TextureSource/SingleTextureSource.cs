using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    public class SingleTextureSource : MonoBehaviour, ITextureSource
    {
        [field: SerializeField]
        public Texture2D OriginalTexture { get; private set; }

        private Texture2D texture;
        public Texture2D Texture
        {
            get => texture;
            set
            {
                OriginalTexture = new Texture2D(value.width, value.height);
                OriginalTexture.filterMode = value.filterMode;
                Graphics.CopyTexture(value, OriginalTexture);

                texture = new Texture2D(value.width, value.height);
                texture.filterMode = value.filterMode;
                Graphics.CopyTexture(value, texture);
            }
        }

        public int PPU { get; set; }

        public virtual void SetUpToRenderer(SpriteRenderer spriteRenderer)
        {

        }
    }
}