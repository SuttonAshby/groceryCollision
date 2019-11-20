using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerItemPrefabs : ScriptableObject
{

    public Ingredient[] items;

    public GameObject[] GetPrefabsForIds(string[] ids)
    {
        var list = new List<GameObject>();
        foreach (var id in ids)
        {
            foreach (var objectID in items)
            {
                if (id == objectID.id)
                {
                    list.Add(objectID.gameObject);
                    break;
                }
            }
        }
        return list.ToArray();
    }

}
