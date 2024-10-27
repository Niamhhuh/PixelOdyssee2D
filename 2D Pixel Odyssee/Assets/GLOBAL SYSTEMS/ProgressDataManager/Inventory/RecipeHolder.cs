using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeHolder : MonoBehaviour
{
    public CraftRecipe Recipe;
    bool NewRecipe = true;
    void Awake()
    {
        foreach(CraftRecipe RecipeRef in DataManager.Recipe_List)
        {
            if (RecipeRef == Recipe)
            {
                NewRecipe = false;
                break;
            }
        }
        if (NewRecipe == true)
        {
            DataManager.Recipe_List.Add(Recipe);
        }
        
    }
}
