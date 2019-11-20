using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerItemPrefabs))]
public class PlayerItemPrefabsEditor : Editor
{


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        if (GUILayout.Button("Get Player Items")) AddAllPlayerItemPrefabs();
        serializedObject.ApplyModifiedProperties();
    }


    private void AddAllPlayerItemPrefabs()
    {
        var assetGuids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Prefabs/Ingredients" });
        var foundItems = new List<Ingredient>();
        foreach (var assetGuid in assetGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetGuid);
            var objectID = (Ingredient)AssetDatabase.LoadAssetAtPath(path, typeof(Ingredient));
            if (objectID != null) foundItems.Add(objectID);
        }
        var playerItemPrefabs = target as PlayerItemPrefabs;
        playerItemPrefabs.items = foundItems.ToArray();
    }

}
