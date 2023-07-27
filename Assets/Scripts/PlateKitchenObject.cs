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
            AddIngredient(kitchenObjectSO);
            return true;
        }
    }

    private void AddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, EventArgs.Empty);
    }

    public bool TryAddBurger(BreadKitchenObject breadKitchenObject)
    {
        List<KitchenObjectSO> burgerIngredients = breadKitchenObject.GetKitchenObjectSOList();
        burgerIngredients.Add(breadKitchenObject.GetKitchenObjectSO());

        foreach(KitchenObjectSO burgerIngredient in burgerIngredients)
        {
            if (kitchenObjectSOList.Contains(burgerIngredient))
            {
                // There is a duplicated ingredient
                return false;
            }
        }
        foreach (KitchenObjectSO burgerIngredient in burgerIngredients)
        {
            if (!IsValidKitchenObjectSO(burgerIngredient))
            {
                // Invalid kitchenObject in burger
                return false;
            }
        }

        foreach (KitchenObjectSO burgerIngredient in burgerIngredients)
        {
            AddIngredient(burgerIngredient);
        }

        return true;
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
