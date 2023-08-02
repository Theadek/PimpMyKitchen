using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentScoreUI : MonoBehaviour
{
    private const string SLIDE = "Slide";

    [SerializeField] private TextMeshProUGUI currentScore;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void UpdateVisual()
    {
        currentScore.text = DeliveryManager.Instance.GetCurrentScore().ToString();
    }

    private void Start()
    {
        UpdateVisual();
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            animator.SetTrigger("Slide");
        }
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }
}
