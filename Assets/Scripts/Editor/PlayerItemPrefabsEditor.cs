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
        var assetGuids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets/Prefabs/Environment/Blocks" });
        var foundItems = new List<ObjectID>();
        foreach (var assetGuid in assetGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetGuid);
            var objectID = (ObjectID)AssetDatabase.LoadAssetAtPath(path, typeof(ObjectID));
            if (objectID != null) foundItems.Add(objectID);
        }
        var spawner = target as PlayerItemPrefabs;
        spawner.items = foundItems.ToArray();
    }

}
