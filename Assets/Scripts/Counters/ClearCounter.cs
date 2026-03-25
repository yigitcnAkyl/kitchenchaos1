using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectsSO kitchenObjectsSO;





    public override void Interact(PlayerMovement playerMovement)

    {
        if (!HasKitchenObject())
        {
            // there isn't a kitchenobject here
            if (playerMovement.HasKitchenObject())
            {
                //Player is carrying something 
                playerMovement.GetKitchenObjects().SetKitchenObjectParent(this);
            }
            else
            {
                //Player  not carrying anything 
            }
        }
        else
        {
            //there is a kitchenobjcet

            if (playerMovement.HasKitchenObject())
            {
                //player is carrying something
                if (playerMovement.GetKitchenObjects().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {


                    if (plateKitchenObject.TryAddIngredient(GetKitchenObjects().GetKitchenObjectsSO()))
                    {
                        GetKitchenObjects().DestroySelf();
                    }

                }
                else
                {
                    //player is not holding a plate something else
                    if(GetKitchenObjects().TryGetPlate(out  plateKitchenObject))
                    {
                        //counter has plate
                      if(plateKitchenObject.TryAddIngredient(playerMovement.GetKitchenObjects().GetKitchenObjectsSO()))
                        {
                            playerMovement.GetKitchenObjects().DestroySelf();

                        }
                            
                    }
                }

            }
            else
            {//player is not carrying anything
                GetKitchenObjects().SetKitchenObjectParent(playerMovement);

            }
        }
    }
}
            
        
    