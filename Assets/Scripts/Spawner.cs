using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public SpawnManagerScriptableObject data;
    public SpawnManagerScriptableObject trashData;
    public Recipes player1Recipes;
    public Recipes player2Recipes;
    public PlayerItemPrefabs playerItemPrefabs;

    public float includeChance = 1f;
    public float inclusionLoops = 1f;

    void Start()
    {
        SpawnOffList();
        SpawnTrash();
    }

    void SpawnEntities()
    {
        int spawnPos = gameObject.transform.childCount;
        // Debug.Log(spawnPos);
        // Debug.Log(creator.transform.childCount);
        // Debug.Log(data.items[1].number);
        foreach(var el in data.items){
            // Debug.Log(el.number);
            for(var j = 0; j < el.number; j++){
                if(spawnPos == 0){
                    spawnPos =  gameObject.transform.childCount;
                }
                Debug.Log("spawning from: " + spawnPos);
                var tr = Instantiate(el.prefab, gameObject.transform.GetChild(spawnPos-1).position, Quaternion.identity);
                AddMouseEventSender(tr.gameObject);
                spawnPos--;
            }
        }
    }

    private void SpawnOffList(){
        int spawnPos = gameObject.transform.childCount;
        //tweak float to increase scarcity
        var items = GetPlayerSpawnItems();
        foreach(var item in items){
            if(spawnPos == 0){
                spawnPos =  gameObject.transform.childCount;
            }
            var obj = Instantiate(item, gameObject.transform.GetChild(spawnPos - 1).position, Quaternion.identity);
            AddMouseEventSender(obj);
            spawnPos--;
        }
    }

    private void AddMouseEventSender(GameObject obj)
    {
#if !UNITY_EDITOR
        return;
#endif
        var coll = obj.GetComponent<Collider>();
        if (coll == null) coll = obj.GetComponentInChildren<Collider>();
        if (coll != null) coll.gameObject.AddComponent<MouseEventSender>();


        var rb = obj.GetComponent<Rigidbody>();
        if (rb == null) rb = obj.GetComponentInChildren<Rigidbody>();
        if (rb.gameObject == coll.gameObject) return;
        if (rb != null) rb.gameObject.AddComponent<MouseEventSender>();
    }

    private void SpawnTrash(){
        int spawnPos = gameObject.transform.childCount;
        // Debug.Log(spawnPos);
        // Debug.Log(creator.transform.childCount);
        // Debug.Log(data.items[1].number);
        foreach(var el in trashData.items){
            // Debug.Log(el.number);
            for(var j = 0; j < el.number; j++){
                if(spawnPos == 0){
                    spawnPos =  gameObject.transform.childCount;
                }
                // Debug.Log("spawning from: " + spawnPos);
                var tr = Instantiate(el.prefab, gameObject.transform.GetChild(spawnPos-1).position, Quaternion.identity);
                AddMouseEventSender(tr.gameObject);
                spawnPos--;
            }
        }
    }



    [ContextMenu("Log Player Items")]
    private void LogPlayerItems()
    {
        var items = GetPlayerSpawnItems();
        foreach (var item in items) Debug.Log(item);
    }

    private GameObject[] GetPlayerSpawnItems()
    {
        var names = GetPlayerSpawnItemNames();
        var prefabs = playerItemPrefabs.GetPrefabsForIds(names);
        return prefabs;
    }

    private string[] GetPlayerSpawnItemNames()
    {
        var player1Items = player1Recipes.GetAllIngredientNames(true);
        var player2Items = player2Recipes.GetAllIngredientNames(true);
        var items = player1Items.Concat(player2Items);

        var list = new List<string>();
        for(var i = 0; i < inclusionLoops; i++)
        {
            foreach (var item in items)
            {
                if (list.IndexOf(item) < 0 || UnityEngine.Random.value <= includeChance)
                {
                    list.Add(item);
                }
            }
        }
        return list.ToArray();
    }

}
