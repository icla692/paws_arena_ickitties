using System.Collections.Generic;
using UnityEngine;

namespace Anura.Globals
{
    public static class Constants
    {
        public static Color accentColor;

        public static readonly List<string> Shapes = new List<string>() { "Circle", "Rect", "Ellipse" };

        static Constants()
        {
            ColorUtility.TryParseHtmlString("#74006F",out accentColor);
        }
    }
}
