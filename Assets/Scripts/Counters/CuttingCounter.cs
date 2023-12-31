using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;

    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipesSO;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(player.GetKitchenObject().GetKitchenObjectSO());
                if (cuttingRecipeSO != null)
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = Mathf.RoundToInt(GetKitchenObject().GetProgress());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax)); //can also just give 0 as 0 divided by whatever is still 0
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
                    // Player has plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        cuttingProgress = 0;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
                    }
                }
                else if (player.GetKitchenObject().TryGetBread(out BreadKitchenObject breadKitchenObject))
                {
                    // Player has bread
                    if (breadKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        cuttingProgress = 0;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
                    }
                }
            }
            else
            {
                // Player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);
                cuttingProgress = 0;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs(0f));
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (player.HasKitchenObject())
        {
            return;
        }

        if (HasKitchenObject())
        {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(GetKitchenObject().GetKitchenObjectSO());
            if(cuttingRecipeSO != null)
            {
                cuttingProgress++;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax));
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);

                if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
                }
                GetKitchenObject().SetProgress(cuttingProgress);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(kitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSO(input);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSO(KitchenObjectSO inputKitchenObjectS0) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipesSO)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectS0)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
