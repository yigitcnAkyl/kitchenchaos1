using System;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress

    
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEvetArgs> OnStateChanged;
    public class OnStateChangedEvetArgs
    {
        public State state;
    }
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;  

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
        { 
            case State.Idle:

                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {

                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                {
                    //fried
                   

                    GetKitchenObjects().DestroySelf();

                    KitchenObjects.SpawnKitchenObject(fryingRecipeSO.output, this);

                      
                     
                        state = State.Fried;
                        burningTimer = 0f;

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEvetArgs
                        {
                            state = state
                        });
                    }
                break;

            case State.Fried:

                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {

                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        //fried


                        GetKitchenObjects().DestroySelf();

                        KitchenObjects.SpawnKitchenObject(burningRecipeSO.output, this);

                       
                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEvetArgs
                        {
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {

                            progressNormalized = 0f

                        });
                    }
                    break;
            case State.Burned:

                break;
        }

            
            
        }
    }

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
                    // player carrying something that can be fried
                    playerMovement.GetKitchenObjects().SetKitchenObjectParent(this);


                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObjects().GetKitchenObjectsSO());

                    state = State.Frying;

                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEvetArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {

                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
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
                    state = State.Idle;
                    OnStateChanged?.Invoke(this, new OnStateChangedEvetArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {

                        progressNormalized = 0f

                    });

                }
            }
            else
            { // Oyuncunun eli boşsa, tezgâhtaki nesneyi al
                GetKitchenObjects().SetKitchenObjectParent(playerMovement);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEvetArgs
                {
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {

                    progressNormalized = 0f

                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectsSO inputkitchenObjectsSO)
    {
        FryingRecipeSO FryingRecipeSO = GetFryingRecipeSOWithInput(inputkitchenObjectsSO);
        return FryingRecipeSO != null;
    }

    private KitchenObjectsSO GetOutputForInput(KitchenObjectsSO inputkitchenObjectsSO)
    {
        FryingRecipeSO FryingRecipeSO = GetFryingRecipeSOWithInput(inputkitchenObjectsSO);
        if (FryingRecipeSO != null)
        {
            return FryingRecipeSO.output;

        }
        else
        {
            return null;
        }
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectsSO inputkitchenObjectsSO)
    {
        foreach (FryingRecipeSO FryingRecipeSO in fryingRecipeSOArray)
        {
            if (FryingRecipeSO.input == inputkitchenObjectsSO)
            {
                return FryingRecipeSO;
            }
        }

        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectsSO inputkitchenObjectsSO)
    {
        foreach (BurningRecipeSO BurningRecipeSO in burningRecipeSOArray)
        {
            if (BurningRecipeSO.input == inputkitchenObjectsSO)
            {
                return BurningRecipeSO;
            }
        }

        return null;
    }
}
 