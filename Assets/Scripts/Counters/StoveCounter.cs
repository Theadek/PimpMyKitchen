using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;
using static IHasProgress;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventHandler> OnStateChanged;
    public class OnStateChangedEventHandler
    {
        public OnStateChangedEventHandler(bool isOn)
        {
            this.isOn = isOn;

        }
        public bool isOn;
    }

    [SerializeField] private StoveRecipeSO[] stoveRecipeSOArray;

    private float fryingTimer;


    private void Start()
    {
        fryingTimer = 0f;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            if(HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO(), out StoveRecipeSO stoveRecipeSO))
            {
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs(fryingTimer / stoveRecipeSO.fryingTimerMax));
                if (fryingTimer >= stoveRecipeSO.fryingTimerMax)
                {
                    fryingTimer = 0f;
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(stoveRecipeSO.output, this);
                    OnStateChanged?.Invoke(this, new OnStateChangedEventHandler(false));
                }
                else
                {
                    OnStateChanged?.Invoke(this, new OnStateChangedEventHandler(true));
                }
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                StoveRecipeSO StoveRecipeSO = GetStoveRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
                if (StoveRecipeSO != null)
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
                else
                {
                    Debug.Log("Wrong input");
                }
            }
        }
        else
        {
            // Something on Counter
            if (player.HasKitchenObject())
            {
                // Player has something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        OnStateChanged?.Invoke(this, new OnStateChangedEventHandler(false));
                        fryingTimer = 0f;
                        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs(0f));
                    }
                }
                else if (player.GetKitchenObject().TryGetBread(out BreadKitchenObject breadKitchenObject))
                {
                    if (breadKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        OnStateChanged?.Invoke(this, new OnStateChangedEventHandler(false));
                        fryingTimer = 0f;
                        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs(0f));
                    }
                }

            }
            else
            {
                // Player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);
                OnStateChanged?.Invoke(this, new OnStateChangedEventHandler(false));
                fryingTimer = 0f;
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs(0f));
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO, out StoveRecipeSO stoveRecipeSO)
    {
        stoveRecipeSO = GetStoveRecipeSO(kitchenObjectSO);
        return stoveRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        StoveRecipeSO stoveRecipeSO = GetStoveRecipeSO(input);
        if (stoveRecipeSO != null)
        {
            return stoveRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private StoveRecipeSO GetStoveRecipeSO(KitchenObjectSO inputKitchenObjectS0)
    {
        foreach (StoveRecipeSO stoveRecipeSO in stoveRecipeSOArray)
        {
            if (stoveRecipeSO.input == inputKitchenObjectS0)
            {
                return stoveRecipeSO;
            }
        }
        return null;
    }
}
