using Anura.ConfigurationModule.Managers;
using Anura.Models;
using Anura.Templates.MonoSingleton;
using DTerrain;
using System;
using UnityEngine;

public class PaintingManager : MonoSingleton<PaintingManager>
{

    [SerializeField] public BasicPaintableLayer primaryLayer;
    [SerializeField] public BasicPaintableLayer secondaryLayer;

    private ShapeConfig currentShape;

    private void Start()
    {
        GetShape(0);
    }

    internal void GetShape(int idx)
    {
        currentShape = ConfigurationManager.Instance.Shapes.GetShape(idx);
    }

    public ShapeConfig GetCurrentShape()
    {
        return currentShape;
    }

    public void Destroy(Vector3 hitPoint)
    {
        hitPoint -= primaryLayer.transform.position;
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(hitPoint.x * primaryLayer.PPU) - currentShape.GetSizeX(), (int)(hitPoint.y * primaryLayer.PPU) - currentShape.GetSizeY()),
            Shape = currentShape.shape,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.DESTROY
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.clear,
            Position = new Vector2Int((int)(hitPoint.x * secondaryLayer.PPU) - currentShape.GetSizeX(), (int)(hitPoint.y * secondaryLayer.PPU) - currentShape.GetSizeY()),
            Shape = currentShape.shape,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.NONE
        });

    }

    public void Build(Vector3 hitPoint)
    {
        primaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(hitPoint.x * primaryLayer.PPU) - currentShape.GetSizeX(), (int)(hitPoint.y * primaryLayer.PPU) - currentShape.GetSizeY()),
            Shape = currentShape.shape,
            PaintingMode = PaintingMode.NONE,
            DestructionMode = DestructionMode.BUILD
        });

        secondaryLayer?.Paint(new PaintingParameters()
        {
            Color = Color.black,
            Position = new Vector2Int((int)(hitPoint.x * secondaryLayer.PPU) - currentShape.GetSizeX(), (int)(hitPoint.y * secondaryLayer.PPU) - currentShape.GetSizeY()),
            Shape = currentShape.shape,
            PaintingMode = PaintingMode.REPLACE_COLOR,
            DestructionMode = DestructionMode.BUILD
        });
    }
}
