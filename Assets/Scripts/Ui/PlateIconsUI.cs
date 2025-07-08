using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject PlateKitchenObject;
    [SerializeField] private Transform iconTemplate;


    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        PlateKitchenObject.OnIngreidentAdded += PlateKitchenObject_OnIngreidentAdded;
    }

    private void PlateKitchenObject_OnIngreidentAdded(object sender, PlateKitchenObject.onInredientADDedEventArgs e)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (KitchenObjectSO kitchenObjectSO in PlateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransorm = Instantiate(iconTemplate, transform);
            //iconTransorm.Find("Image").GetComponent<>
            iconTransorm.gameObject.SetActive(true);
            iconTransorm.GetComponent<PlateIconsSingeUi>().SetKitchenObjectSo(kitchenObjectSO);
        }
    }
}
