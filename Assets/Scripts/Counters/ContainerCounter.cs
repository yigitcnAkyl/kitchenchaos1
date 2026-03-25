using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;



    public override void Interact(PlayerMovement playerMovement)

    {
        if (!playerMovement.HasKitchenObject())
        {
            //If player doesn't have something container gives an object but if player has something container does nothing

            KitchenObjects.SpawnKitchenObject(kitchenObjectsSO, playerMovement);

            

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
    

}