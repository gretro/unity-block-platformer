using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class ColliderUtils
    {
        public static bool IsOnLayerMask(GameObject gameObject, LayerMask layerMask)
        {
            return ((1 << gameObject.layer) & layerMask) != 0;
        }
    }
}