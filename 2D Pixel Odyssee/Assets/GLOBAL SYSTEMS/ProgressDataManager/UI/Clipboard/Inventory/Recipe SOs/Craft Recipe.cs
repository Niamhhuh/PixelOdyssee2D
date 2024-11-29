using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class CraftRecipe : ScriptableObject
{
    public int KeyID_A;
    public int KeyID_B;
    public int Crafted_Item_ID;
}
