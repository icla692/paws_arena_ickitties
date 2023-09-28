using Anura.Globals;
using DTerrain;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anura.Models
{
    [CreateAssetMenu(fileName = "Shape", menuName = "Configurations/Shape", order = 3)]
    public class ShapeConfig : ScriptableObject
    {
        [InfoBox("Don't duplicate the file. When we duplicate a file, the id will also be duplicated. \nUse the Configurations->Shape", EInfoBoxType.Normal)]

        [ReadOnly]
        public string ID;

        [Dropdown("Shapes")]
        public string shapeType;

        [ShowIf("IsCircle")]
        public int circleSize;

        [ShowIf("IsRect")]
        public int width;
        [ShowIf("IsRect")]
        public int height;

        [ShowIf("IsEllipse")]
        public int radiusX;
        [ShowIf("IsEllipse")]
        public int radiusY;
        [ShowIf("IsEllipse")]
        public int pixelSize;

        public Shape shape;


        private List<string> Shapes => Constants.Shapes;

        private bool IsCircle => shapeType == Shapes[0];
        private bool IsRect => shapeType == Shapes[1];
        private bool IsEllipse => shapeType == Shapes[2];

        public void Init()
        {
            switch (shapeType)
            {
                case "Circle":
                    shape = Shape.GenerateShapeCircle(circleSize);
                    break;

                case "Rect":
                    shape = Shape.GenerateShapeRect(width, height);
                    break;

                case "Ellipse":
                    shape = Shape.GeneratePixelatedEllipse(new Vector2Int(radiusX, radiusY), pixelSize);
                    break;

                default:
                    Debug.Log("Doesn't exists");
                    break;
            }
        }

        public int GetSize()
        {
            switch (shapeType)
            {
                case "Circle":
                    return circleSize;

                case "Ellipse":
                    return radiusX;

                case "Rect":
                    return (height + width) / 2;

                default:
                    Debug.Log("Doesn't exists");
                    return (height + width + circleSize) / 3;
            }
        }



        public int GetSizeX()
        {
            switch (shapeType)
            {
                case "Circle":
                    return circleSize;

                case "Ellipse":
                    return radiusX;

                case "Rect":
                    return width;

                default:
                    Debug.Log("Doesn't exists");
                    return (width + circleSize + radiusX) / 3;
            }
        }

        public int GetSizeY()
        {
            switch (shapeType)
            {
                case "Circle":
                    return circleSize;

                case "Ellipse":
                    return radiusY;

                case "Rect":
                    return height;

                default:
                    Debug.Log("Doesn't exists");
                    return (height + circleSize + radiusY) / 3;
            }
        }

        private void OnValidate()
        {
            if(ID.Equals(string.Empty))
                ID = Guid.NewGuid().ToString();
        }

    }
}