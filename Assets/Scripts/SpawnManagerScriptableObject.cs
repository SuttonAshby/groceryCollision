using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class SpawnManagerScriptableObject : ScriptableObject
{
    [Serializable]
    public class SpawnRecord {
        public Transform prefab;
        public int number;
    }

    public SpawnRecord[] items;
    // public string prefabName;


    // public int numberOfPrefabsToCreate;
    // public Vector3[] spawnPoints;
}
