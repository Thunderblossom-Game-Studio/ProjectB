using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JE.General
{
    public static class Utilities
    {
        public static bool ContainsLayer(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & (1 << layer)) != 0;
        }
    }
}
