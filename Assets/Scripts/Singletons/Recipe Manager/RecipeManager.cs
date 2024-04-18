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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

    public bool CanCraft(Item targetItem, Dictionary<Item, int> inventory)
    {
        Recipe recipe = GetRecipe(targetItem);

        if(recipe==null) return false;

        for(int i=0; i<recipe.ingredients.Count; i++)
        {
            if(inventory.ContainsKey(recipe.ingredients[i]))
            {
                if(inventory[recipe.ingredients[i]] >= recipe.quantities[i])
                {
                    continue;
                }
                else return false;
            }
            else return false;
        }

        return true;
    }

    public void Craft(Item targetItem, Dictionary<Item, int> inventory)
    {
        if(!CanCraft(targetItem, inventory)) return;

        Recipe recipe = GetRecipe(targetItem);

        if(recipe==null) return;

        for(int i=0; i<recipe.ingredients.Count; i++)
        {
            inventory[recipe.ingredients[i]] -= recipe.quantities[i];
        }

        foreach(Item result in recipe.results)
        {
            inventory[result] += 1;
        }
    }
}
