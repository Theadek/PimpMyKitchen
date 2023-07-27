using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;
            if(platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(platesSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                platesSpawnedAmount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }

        }
        else
        {
            // Player has something
            if (plateKitchenObjectSO.prefab.GetComponent<PlateKitchenObject>().IsValidKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // Kitchen Object can be stored on plate
                if (platesSpawnedAmount > 0)
                {
                    // Has available plates
                    KitchenObjectSO playerHoldingKitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                    player.GetKitchenObject().DestroySelf();

                    PlateKitchenObject newPlate = KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player) as PlateKitchenObject;
                    newPlate.TryAddIngredient(playerHoldingKitchenObjectSO);
                    newPlate.GetComponentInChildren<PlateIconsUI>().UpdateVisual();
                    newPlate.GetComponentInChildren<PlateCompleteVisual>().RefreshVisual();

                    platesSpawnedAmount--;
                    OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
