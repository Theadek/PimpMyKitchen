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
            if(KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (platesSpawnedAmount <= 0) return;


        if (!player.HasKitchenObject())
        {
            // Player is empty handed
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            platesSpawnedAmount--;
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Player has something
            if (plateKitchenObjectSO.prefab.GetComponent<PlateKitchenObject>().IsValidKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                // Kitchen Object can be stored on plate

                if(player.GetKitchenObject().TryGetBread(out BreadKitchenObject breadKitchenObject))
                {
                    // Player has bread, so needs to add ingredients from bread too
                    List<KitchenObjectSO> ingredientsInBurger = breadKitchenObject.GetKitchenObjectSOList();

                    KitchenObjectSO playerHoldingKitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                    player.GetKitchenObject().DestroySelf();

                    PlateKitchenObject newPlate = KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player) as PlateKitchenObject;
                    foreach (KitchenObjectSO kitchenObjectSO in ingredientsInBurger)
                    {
                        newPlate.TryAddIngredient(kitchenObjectSO);
                    }
                    newPlate.TryAddIngredient(playerHoldingKitchenObjectSO);
                }
                else
                {
                    KitchenObjectSO playerHoldingKitchenObjectSO = player.GetKitchenObject().GetKitchenObjectSO();
                    player.GetKitchenObject().DestroySelf();

                    PlateKitchenObject newPlate = KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player) as PlateKitchenObject;
                    newPlate.TryAddIngredient(playerHoldingKitchenObjectSO);
                }


                platesSpawnedAmount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);


            }

        }
    }
}
