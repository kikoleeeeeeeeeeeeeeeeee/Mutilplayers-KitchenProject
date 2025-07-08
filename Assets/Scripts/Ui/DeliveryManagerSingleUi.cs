using System.Collections;
using System.Collections.Generic; 
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUi : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;


    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSo(RecipetSo recipetSo)
    {     
        recipeNameText.text = recipetSo.recipeName;
        foreach(Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in recipetSo.kitchenObjectsoList) {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }

    } 
}
