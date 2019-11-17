using UnityEngine;
using System;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public SpawnManagerScriptableObject data;
    public SpawnManagerScriptableObject trashData;
    public Recipes player1Recipes;
    public Recipes player2Recipes;
    public PlayerItemPrefabs playerItemPrefabs;

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
                Instantiate(el.prefab, gameObject.transform.GetChild(spawnPos-1).position, Quaternion.identity);
                spawnPos--;
            }
        }
    }

    private void SpawnOffList(){
        int spawnPos = gameObject.transform.childCount;
        //tweak float to increase scarcity
        var items = GetPlayerSpawnItems(1f);
        foreach(var item in items){
            if(spawnPos == 0){
                spawnPos =  gameObject.transform.childCount;
            }
            Instantiate(item, gameObject.transform.GetChild(spawnPos - 1).position, Quaternion.identity);
            spawnPos--;
        }
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
                Instantiate(el.prefab, gameObject.transform.GetChild(spawnPos-1).position, Quaternion.identity);
                spawnPos--;
            }
        }
    }



    [ContextMenu("Log Player Items")]
    private void LogPlayerItems()
    {
        var items = GetPlayerSpawnItems(1f);
        foreach (var item in items) Debug.Log(item);
    }

    private GameObject[] GetPlayerSpawnItems(float includeChance = 1f)
    {
        var names = GetPlayerSpawnItemNames(includeChance);
        var prefabs = playerItemPrefabs.GetPrefabsForIds(names);
        return prefabs;
    }

    private string[] GetPlayerSpawnItemNames(float includeChance = 1f)
    {
        var player1Items = player1Recipes.GetAllIngredientNames(true);
        var player2Items = player2Recipes.GetAllIngredientNames(true);
        var list = new List<string>(player1Items);
        foreach (var player2Item in player2Items)
        {
            if (Array.IndexOf(player1Items, player2Item) < 0 || includeChance == 1f || UnityEngine.Random.value < includeChance)
            {
                list.Add(player2Item);
            }
        }
        return list.ToArray();
    }

}
