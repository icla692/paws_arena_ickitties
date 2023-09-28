using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal static class TerrainNavigationLibrary
{
    public enum Direction
    {
        Left,
        Right
    }

    public const int LAYER_CHUNK = 6;
    public const int LAYER_ENVIRONMENT = 9;
    public const int LAYER_CHARACTER = 8;

    public static readonly LayerMask LAYERMASK_CHUNK = 1 << LAYER_CHUNK;
    public static readonly LayerMask LAYERMASK_ENVIRONMENT = 1 << LAYER_ENVIRONMENT;
    public static readonly LayerMask LAYERMASK_CHARACTER = 1 << LAYER_CHARACTER;

    public static readonly LayerMask LAYERMASK_TERRAIN = LAYERMASK_CHUNK | LAYERMASK_ENVIRONMENT;
    public static readonly LayerMask LAYERMASK_HITTABLES = LAYERMASK_TERRAIN | LAYERMASK_CHARACTER;

    private const float TRACING_STEP = 0.2f;

    /// <summary>
    /// Get the position where 'obj' would end up by traveling (up to) 'xDisplacement' in a given 'direction'.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="direction"></param>
    /// <param name="xDisplacement"></param>
    /// <returns></returns>
    public static Vector3 GetPositionAtXDisplacement(Bounds obj, Direction direction, float xDisplacement)
    {
        Vector3 xDir = DirectionToVec(direction);
        float xLengthTraversed = TRACING_STEP;

        Vector3 sourcePos = obj.center;
        Vector3 probingPos = sourcePos;
        Vector3 lastGoodProbingPos = sourcePos;

        while (xLengthTraversed <= xDisplacement)
        {
            float y = probingPos.y;
            probingPos = sourcePos + xLengthTraversed * xDir;
            probingPos.y = y;

            if (!PositionIsBetweenHorizontalBounds(probingPos, obj))
            {
                break;
            }

            RaycastHit2D hit = RaycastDownAtYIntervals(probingPos, obj);
            if (hit)
            {
                probingPos.y = hit.point.y + (obj.size.y / 2);                
            }
            else
            {
                break;
            }

            // Check if object can fit in the location
            if (Physics2D.OverlapCapsule(
                hit.point + obj.size.y / 2 * Vector2.up,
                0.9f * obj.size,
                GetCapsuleDirection(obj),
                0,
                LAYERMASK_TERRAIN) == null)
            {
                lastGoodProbingPos = probingPos;
                xLengthTraversed += TRACING_STEP;
            }
            else
            {
                xLengthTraversed += TRACING_STEP / 2;
            }
        }

        return lastGoodProbingPos;
    }

    public static bool PositionIsBetweenHorizontalBounds(Vector3 pos, Bounds obj)
    {
        float objWidth = obj.size.x / 2;

        Bounds left = BotManager.Instance.leftMapBound.bounds;
        if (pos.x < left.max.x + objWidth) return false;

        Bounds right = BotManager.Instance.rightMapBound.bounds;
        if (pos.x > right.min.x - objWidth) return false;

        return true;
    }

    private static RaycastHit2D RaycastDown(Vector3 origin)
    {
        return Physics2D.Raycast(origin, Vector3.down, Mathf.Infinity, LAYERMASK_TERRAIN);
    }

    private static RaycastHit2D RaycastDownAtYIntervals(Vector3 origin, Bounds obj, int attempts = 100)
    {
        float y = 0;

        for (int i=0; i<attempts; i++)
        {
            RaycastHit2D hit = RaycastDown(origin + Vector3.up * y);
            if (hit)
            {
                return hit;
            }
            y += obj.size.y / 2;
        }

        return new RaycastHit2D();
    }

    private static CapsuleDirection2D GetCapsuleDirection(Bounds bounds)
    {
        if (bounds.size.x > bounds.size.y) return CapsuleDirection2D.Horizontal;
        return CapsuleDirection2D.Vertical;
    }

    private static Vector3 DirectionToVec(Direction dir)
    {
        return dir == Direction.Left ? Vector3.left : Vector3.right;
    }
}
