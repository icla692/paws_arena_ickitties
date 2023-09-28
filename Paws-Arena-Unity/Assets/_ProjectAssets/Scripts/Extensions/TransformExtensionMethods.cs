using UnityEngine;

namespace Anura.Extensions
{
    public static class TransformExtensionMethods
    {
        public static void ResetTransform(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }

        public static void RandomTransformPosition(this Transform trans, float minRange, float maxRange, float y)
        {
            trans.position = new Vector3(Random.Range(minRange, maxRange), y, Random.Range(minRange, maxRange));
        }
    }
}
