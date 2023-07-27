using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler OnIngredientAdded;

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!IsValidKitchenObjectSO(kitchenObjectSO))
        {
            // Not a valid ingredient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // No duplicates!
            return false;
        }
        else
        {
            // Added new object
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }

    public bool IsValidKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        return validKitchenObjectSOList.Contains(kitchenObjectSO);
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
