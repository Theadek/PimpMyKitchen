using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        RefreshVisual();
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, EventArgs e)
    {
        RefreshVisual();
    }

    public void RefreshVisual()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
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
