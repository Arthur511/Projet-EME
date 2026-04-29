using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FusionWord", menuName = "Create/Dictionary/Word Recipe Library")]
public class WordRecipeLibrary : ScriptableObject
{
    public List<WordRecipe> WordRecipes;

    //WordRecipe _currentRecipe;

    public WordRecipe CheckRecipe(Word wordA, Word word2)
    {
        foreach (WordRecipe recipe in WordRecipes)
        {
            if ((recipe.WordOne == wordA && recipe.WordTwo == word2) || (recipe.WordOne == word2 && recipe.WordTwo == wordA))
            {
                //_currentRecipe = recipe;
                return recipe;
            }
        }
        return null;
    }
}
