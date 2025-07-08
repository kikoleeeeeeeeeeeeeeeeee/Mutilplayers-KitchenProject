using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManagerUi : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DliveryManager.Instance.OnRecipeSpwaned += DeliveryManager_OnRecipeSpwaned;
        DliveryManager.Instance.OnRecipeCompeleted += DeliveryManager_OnRecipeCompeleted;

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompeleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpwaned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }
       foreach(RecipetSo recipetSo in DliveryManager.Instance.GetWaitingRecipeSolist())
        {
            Transform  recipeTransform = Instantiate(recipeTemplate,container);
            recipeTransform.gameObject.SetActive(true);
            //recipeTransform.Find("RecipeName"); 
            recipeTransform.GetComponent<DeliveryManagerSingleUi>().SetRecipeSo(recipetSo);
        }
    }
}
