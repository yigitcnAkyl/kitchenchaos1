using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectsSO> kitchenObejctSOList;
    public string recipeName;



}
