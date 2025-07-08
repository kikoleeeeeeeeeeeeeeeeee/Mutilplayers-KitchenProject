using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUi : MonoBehaviour
{
    private const string POPUP = "PopUp";

    [SerializeField] private Image backgroundImag;
    [SerializeField] private Image IconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failureColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failureSprite;


    private Animator animator;


    private void Awake()
    {
        animator=GetComponent<Animator>();          
    }
    private void Start()
    {
        DliveryManager.Instance.OnRecipeSuccess += DliveryManager_OnRecipeSuccess;
        DliveryManager.Instance.OnRecipefailed += DliveryManager_OnRecipefailed;


        gameObject.SetActive(false);
    }

    private void DliveryManager_OnRecipefailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImag.color = failureColor;
        IconImage.sprite = failureSprite;
        messageText.text = "DELIVERY\nFAILED";
    }

    private void DliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImag.color = successColor;
        IconImage.sprite = successSprite;  
        messageText.text = "DELIVERY\nFAILED";
    } 
}
