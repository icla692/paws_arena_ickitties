using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class ClickAndDestroyOptimized : ClickAndDestroy
{
    protected override void OnLMBClick()
    {
        Vector3 cameraClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;

        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - ellipseSize.x, (int)(p.y * primaryLayer.PPU) - ellipseSize.y),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - ellipseSize.x, (int)(p.y * secondaryLayer.PPU) - ellipseSize.y),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.NONE
        });

    }

    protected override void OnRMBClick()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition) - primaryLayer.transform.position;
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(p.x * primaryLayer.PPU) - ellipseSize.x, (int)(p.y * primaryLayer.PPU) - ellipseSize.y),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.NONE,
            DestructionMode = DestructionMode.BUILD
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(p.x * secondaryLayer.PPU) - ellipseSize.x, (int)(p.y * secondaryLayer.PPU) - ellipseSize.y),
            Shape = destroyCircle,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.BUILD
        });

    }
}
