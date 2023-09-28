using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTerrain
{
    /// <summary>
    /// Shape is a simple class that holds a list of Ranges (not ranges) and then is used to destroy terrain with.
    /// To make complicated shape destructions (not squares, circles etc.) don't use it as it supports only list of ranges.
    /// </summary>
    public class Shape
    {
        public List<Range> Ranges;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Shape(int w, int h)
        {
            Width = w;
            Height = h;
            Ranges = new List<Range>();
        }

        public static Shape GenerateShapeRange(int length)
        {
            Shape s = new Shape(1, length);
            s.Ranges.Add(new Range(0, length));
            return s;
        }

        /// <summary>
        /// Generates a Shape: circle.
        /// </summary>
        /// <param name="r">Radius</param>
        /// <returns>Shape: circle.</returns>
        public static Shape GenerateShapeCircle(int r)
        {
            int centerX = r;
            int centerY = r;

            Shape s = new Shape(2 * r, 2 * r);
            for (int i = 0; i <= 2 * r; i++)
            {
                bool down = false;
                int min = 0;
                int max = 0;
                for (int j = 0; j <= 2 * r; j++)
                {
                    if (Mathf.Sqrt((centerX - i) * (centerX - i) + (centerY - j) * (centerY - j)) < r)
                    {
                        if (down == false)
                        {
                            down = true;
                            min = j;
                        }
                    }
                    else
                    {
                        if (down)
                        {
                            max = j;
                            break;
                        }
                    }
                }

                if (down)
                {
                    Range range = new Range(min, max);
                    s.Ranges.Add(range);
                }
            }

            return s;
        }

        public static Shape GenerateEllipse(Vector2Int r)
        {
            return GeneratePixelatedEllipse(r, 1);
        }

        public static Shape GeneratePixelatedEllipse(Vector2Int r, int pixelSize=8)
        {
            int centerX = r.x;
            int centerY = r.y;

            Shape s = new Shape(2 * centerX, 2 * centerY);
            for (int i = 0; i <= 2 * centerX; i+=pixelSize)
            {
                bool down = false;
                int min = 0;
                int max = 0;

                for (int j = 0; j <= 2 * centerY; j+= pixelSize)
                {
                    if (CheckPointInsideEllipse(i, j, r.x, r.y, centerX, centerY))
                    {
                        if (down == false)
                        {
                            down = true;
                            min = j;
                        }
                    }
                    else
                    {
                        if (down)
                        {
                            max = j;
                            break;
                        }
                    }
                }

                if (down && min != max)
                {
                    for (int k = 0; k < pixelSize; k++)
                    {
                        Range range = new Range(min, max);
                        s.Ranges.Add(range);
                    }
                }
            }

            return s;
        }

        private static bool CheckPointInsideEllipse(int x, int y, int rx, int ry, int h, int k)
        {
            return ((x - h) * (x - h) * 1.0f / rx / rx + (y - k) * (y - k) * 1.0f / ry / ry <= 1);
        }

        public static Shape GenerateShapeRect(int w, int h)
        {
            Shape s = new Shape(w, h);
            for (int i = 0; i < w; i++)
            {
                s.Ranges.Add(new Range(0, h - 1));
            }

            return s;
        }
    }
}