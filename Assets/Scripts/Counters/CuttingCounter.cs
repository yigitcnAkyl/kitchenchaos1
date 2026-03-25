using System;
using System.Runtime.InteropServices;
using UnityEngine;

    public class CuttingCounter : BaseCounter, IHasProgress
    {
    public static event EventHandler OnAnyCut;

    new public static void resetStaticData()
    {
        OnAnyCut = null;
    }


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
 
    public event EventHandler OnCut;


        [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;


    private int cuttingProgress;


    public override void Interact(PlayerMovement playerMovement)
    {
        if (!HasKitchenObject())
        {
            // Tezgahta bir şey yok
            if (playerMovement.HasKitchenObject())
            {
                // Oyuncunun elinde bir şey var
                KitchenObjectsSO playerObjectSO = playerMovement.GetKitchenObjects().GetKitchenObjectsSO();

                if (HasRecipeWithInput(playerObjectSO))
                {
                    // Oyuncunun taşıdığı nesne kesilebilecek bir şeyse, tezgâha yerleştir
                    playerMovement.GetKitchenObjects().SetKitchenObjectParent(this);

                    cuttingProgress = 0;



                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
        }
        else
        {
            // Tezgahta bir şey var
            if (playerMovement.HasKitchenObject())
            {
                if (playerMovement.GetKitchenObjects().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {


                    if (plateKitchenObject.TryAddIngredient(GetKitchenObjects().GetKitchenObjectsSO()))
                    {
                        GetKitchenObjects().DestroySelf();
                    }

                }
            }
            else { GetKitchenObjects().SetKitchenObjectParent(playerMovement); }
        } 
    }

    public override void InteractAlternate(PlayerMovement playerMovement)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObjects().GetKitchenObjectsSO()))
        {
            //there is an object here and it can be cut
            cuttingProgress++;

            OnCut?.Invoke(this,EventArgs.Empty);
            Debug.Log(OnAnyCut.GetInvocationList().Length);
            OnAnyCut?.Invoke(this,EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectsSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObjects().GetKitchenObjectsSO());

                GetKitchenObjects().DestroySelf();

                KitchenObjects.SpawnKitchenObject(outputKitchenObjectSO, this);

            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectsSO inputkitchenObjectsSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputkitchenObjectsSO);
        return cuttingRecipeSO != null;
    }

        private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputkitchenObjectsSO)
        {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputkitchenObjectsSO);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;

        }  
          else {
            return null; 
        }
        }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectsSO inputkitchenObjectsSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputkitchenObjectsSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }

    }
   

