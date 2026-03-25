using UnityEngine;

public class DeliveryCounter : BaseCounter
{


    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(PlayerMovement playerMovement)
    {
        if (playerMovement.HasKitchenObject())
        {
            if (playerMovement.GetKitchenObjects().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {  //player holding a plate


                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                playerMovement.GetKitchenObjects().DestroySelf();
            }
        }
    }
}
