using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultsUI : MonoBehaviour
{
    private const string POP_UP = "Popup";

    [SerializeField] private Animator successfulDeliveryImage;
    [SerializeField] private Animator failedDeliveryImage;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        successfulDeliveryImage.gameObject.SetActive(false);
        failedDeliveryImage.gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        failedDeliveryImage.gameObject.SetActive(true);
        failedDeliveryImage.SetTrigger(POP_UP);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        successfulDeliveryImage.gameObject.SetActive(true);
        successfulDeliveryImage.SetTrigger(POP_UP);
    }
}
