using UnityEngine;
using System;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public SpawnManagerScriptableObject data;
    public Recipes player1Recipes;
    public Recipes player2Recipes;

    void Start()
    {
        SpawnEntities();
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

        // int currentSpawnPointIndex = 0;

        // for (int i = 0; i < spawnManagerValues.numberOfPrefabsToCreate; i++)
        // {
        //     // Creates an instance of the prefab at the current spawn point.
        //     GameObject currentEntity = Instantiate(entityToSpawn, gameObject.transform);

        //     // Sets the name of the instantiated entity to be the string defined in the ScriptableObject and then appends it with a unique number.
        //     currentEntity.name = spawnManagerValues.prefabName + instanceNumber;

        //     // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
        //     // currentSpawnPointIndex = (currentSpawnPointIndex + 1) % spawnManagerValues.spawnPoints.Length;

        //     instanceNumber++;
        // }

    }

    [ContextMenu("Log Player Items")]
    private void LogPlayerItems()
    {
        var items = GetPlayerSpawnItems(1f);
        foreach (var item in items) Debug.Log(item);
    }

    private string[] GetPlayerSpawnItems(float includeChance = 1f)
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
