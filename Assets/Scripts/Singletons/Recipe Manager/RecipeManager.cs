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

    public List<Recipe> GetRecipes(Item targetItem)
    {
        List<Recipe> matches = new();

        foreach(Recipe recipe in recipes)
        {
            if(recipe.results.Contains(targetItem))
            {
                matches.Add(recipe);
            }
        }
        return matches;
    }

    public bool CanCraft(Recipe recipe, Inventory inv)
    {
        if(recipe==null) return false;

        for(int i=0; i<recipe.ingredients.Count; i++)
        {
            if(inv.HasItem(recipe.ingredients[i]))
            {
                if(inv.GetItemQuantity(recipe.ingredients[i]) >= recipe.quantities[i])
                {
                    continue;
                }
                else return false;
            }
            else return false;
        }
        return true;
    }

    public bool CanCraft(Item targetItem, Inventory inv)
    {
        Recipe recipe = GetRecipe(targetItem);

        return CanCraft(recipe, inv);
    }

    public void Craft(Recipe recipe, Inventory inv, GameObject station)
    {
        if(recipe==null) return;

        if(!CanCraft(recipe, inv)) return;

        for(int i=0; i<recipe.ingredients.Count; i++)
        {
            inv.RemoveItem(recipe.ingredients[i], recipe.quantities[i]);
        }

        foreach(Item item in recipe.results)
        {
            GameObject spawned = ItemManager.Current.Spawn(item, station.transform.position);

            StationManager.Current.OccupyTarget(spawned, inv.gameObject);
        }
    }

    public void Craft(Item targetItem, Inventory inv, GameObject station)
    {
        if(!CanCraft(targetItem, inv)) return;

        Recipe recipe = GetRecipe(targetItem);

        Craft(recipe, inv, station);
    }

    public Item GetGoalIngredient(Item goalItem, Inventory inv)
    {
        Item goalIngredient = Item.None;

        Stack<Item> nextItem = new();
        nextItem.Push(goalItem);

        while(nextItem.Count>0)
        {
            Item currentItem = nextItem.Pop();
            Recipe currentRecipe = GetRecipe(currentItem);

            if(currentRecipe!=null) // if have recipe
            {
                for(int i=0; i<currentRecipe.ingredients.Count; i++)
                {
                    Item ingredient = currentRecipe.ingredients[i];
                    int quantity = currentRecipe.quantities[i];

                    if(!inv.HasItem(ingredient, quantity))
                    {
                        nextItem.Push(ingredient); // check the sub ingredient
                        break;
                    }

                    goalIngredient = currentItem;
                }
            }
            else goalIngredient = currentItem; // raw ingredient
        }

        return goalIngredient;
    }
}
