using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;



    private float spawnPlateTimer;
    private float spawnTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnTimerMax)
        {
            spawnPlateTimer = 0;


            if (platesSpawnedAmount < platesSpawnedAmountMax) {

                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this,  EventArgs.Empty);
        }
        }
        
    }

    public override void Interact(PlayerMovement playerMovement)
    {
        if (!playerMovement.HasKitchenObject())
        {//player doesnt have object
            if (platesSpawnedAmount > 0)
            {
                //there 1 or more plate here
                platesSpawnedAmount--;
                KitchenObjects.SpawnKitchenObject(plateKitchenObjectSO, playerMovement);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }

    }
}
