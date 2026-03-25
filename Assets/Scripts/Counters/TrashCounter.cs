using System;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    public static event EventHandler OnAnyObjectTrashed;

    new public static void resetStaticData()
    {
             OnAnyObjectTrashed = null;
    }
    public override void Interact(PlayerMovement playerMovement)
    {
        if (playerMovement.HasKitchenObject())
        {
            playerMovement.GetKitchenObjects().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this,EventArgs.Empty);
        }
    }
}