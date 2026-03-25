using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObjects
{

    public event EventHandler <OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectsSO;
    }

    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectSOList;
   
   
    private List<KitchenObjectsSO> kitchenObjectsSOList;

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectsSO>();
    }

    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectsSO)
    {  if (!validKitchenObjectSOList.Contains(kitchenObjectsSO)) { 
        //not a valid ing.
        return false;
        
        }
        if (kitchenObjectsSOList.Contains(kitchenObjectsSO))
        {
            //Already has this type
            return false;

        }
        else
        {

            kitchenObjectsSOList.Add(kitchenObjectsSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectsSO = kitchenObjectsSO
            });

            return true;
        }
    }

    public
        
        List<KitchenObjectsSO> GetKitchenObjectsSOList() {
        return kitchenObjectsSOList;
    }
}
