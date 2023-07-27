using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerIconsUI : MonoBehaviour
{
    [SerializeField] private BreadKitchenObject breadKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        breadKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        UpdateVisual();
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        List<KitchenObjectSO> breadKitchenObjectSOList = breadKitchenObject.GetKitchenObjectSOList();
        if (breadKitchenObjectSOList.Count > 0)
        {
            breadKitchenObjectSOList.Add(breadKitchenObject.GetKitchenObjectSO());
            foreach (KitchenObjectSO kitchenObjectSO in breadKitchenObject.GetKitchenObjectSOList())
            {
                Transform iconTransform = Instantiate(iconTemplate, transform);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
            }
        }
    }
}
