using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private BreadKitchenObject breadKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    private void Start()
    {
        breadKitchenObject.OnIngredientAdded += BreadKitchenObject_OnIngredientAdded;
        RefreshVisual();
    }

    private void BreadKitchenObject_OnIngredientAdded(object sender, System.EventArgs e)
    {
        RefreshVisual();
    }

    public void RefreshVisual()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (KitchenObjectSO kitchenObjectSO in breadKitchenObject.GetKitchenObjectSOList())
        {
            foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSOGameObjectList)
            {
                if (kitchenObjectSO_GameObject.kitchenObjectSO == kitchenObjectSO)
                {
                    kitchenObjectSO_GameObject.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
