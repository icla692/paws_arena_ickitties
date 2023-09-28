using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITextureSource
{
    Texture2D Texture { get; set; }
    int PPU { get; set; }
    void SetUpToRenderer(SpriteRenderer spriteRenderer);
}
