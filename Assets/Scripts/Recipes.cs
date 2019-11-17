using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class Recipes : ScriptableObject
{

    public ItemTree.Item[] recipeRoots;
    public ItemTree itemTree;

    [ContextMenu("Log All Ingredients")]
    public void LogAllIngredients()
    {
        var ingredients = GetAllIngredients();
        foreach (var item in ingredients) Debug.Log(item.Name);
    }

    public ItemTree.Item[] GetAllIngredients()
    {
        var ingredients = new List<ItemTree.Item>();
        foreach (var item in recipeRoots)
        {
            AddItemToIngredients(item, ingredients);
        }
        return ingredients.ToArray();
    }

    private void AddItemToIngredients(ItemTree.Item item, List<ItemTree.Item> ingredients)
    {
        if (item == null) return;
        if (!ingredients.Contains(item)) ingredients.Add(item);
        foreach (var option in item.Options)
        {
            foreach (var child in option.Items)
            {
                AddItemToIngredients(child, ingredients);
            }
        }
    }

}
