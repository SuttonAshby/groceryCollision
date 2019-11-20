using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{

    public static void SetLayerRecursively(this GameObject obj, string layerName)
    {
        var layer = LayerMask.NameToLayer(layerName);
        SetLayerRecursively(obj, layer);
    }

    private static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

}
