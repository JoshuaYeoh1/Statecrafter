using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string name;
    public StationType craftingStation;
    public List<Item> ingredients = new();
    public List<int> quantities = new();
    public float craftingTime;
    public List<Item> results = new();
}

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Current;

    void Awake()
    {
        Current=this;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<Recipe> recipes = new();

    public Recipe GetRecipe(Item targetItem)
    {
        foreach(Recipe recipe in recipes)
        {
            if(recipe.results.Contains(targetItem))
            {
                return recipe;
            }
        }

        return null;
    }

    public bool CanCraft(Recipe recipe, Dictionary<Item, int> inventory)
    {
        if(recipe==null) return false;

        for(int i=0; i<recipe.ingredients.Count; i++)
        {
            if(inventory.ContainsKey(recipe.ingredients[i]))
            {
                if(inventory[recipe.ingredients[i]] >= recipe.quantities[i])
                {
                    continue;
                }
                else
                {
                    Debug.LogWarning($"Not Enough {recipe.ingredients[i]} for {recipe.name}");
                    return false;
                }
            }
            else
            {
                Debug.LogWarning($"No {recipe.ingredients[i]} for {recipe.name}");
                return false;
            }
        }

        return true;
    }

    public bool CanCraft(Item targetItem, Dictionary<Item, int> inventory)
    {
        Recipe recipe = GetRecipe(targetItem);

        return CanCraft(recipe, inventory);
    }

    public void Craft(Recipe recipe, Dictionary<Item, int> inventory, GameObject station)
    {
        if(recipe==null) return;

        if(!CanCraft(recipe, inventory)) return;

        for(int i=0; i<recipe.ingredients.Count; i++)
        {
            inventory[recipe.ingredients[i]] -= recipe.quantities[i];
        }

        foreach(Item item in recipe.results)
        {
            ItemManager.Current.Spawn(item, station.transform.position);
        }

        // foreach(Item result in recipe.results)
        // {
        //     inventory[result] += 1;
        // }
    }

    public void Craft(Item targetItem, Dictionary<Item, int> inventory, GameObject station)
    {
        if(!CanCraft(targetItem, inventory)) return;

        Recipe recipe = GetRecipe(targetItem);

        Craft(recipe, inventory, station);
    }    
}
