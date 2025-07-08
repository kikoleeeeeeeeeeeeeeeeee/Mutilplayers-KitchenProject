using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable]
    public struct kitchenObjectSO_GameOjbect
    {
        public  KitchenObjectSO KitchenObjectSO; 
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject platekitchenObject;
    [SerializeField] private List<kitchenObjectSO_GameOjbect> kitchenObjectSO_GameOjbectList;
  
    
    private void Start()
    {
        platekitchenObject.OnIngreidentAdded += PlatekitchenObject_OnIngreidentAdded;
        foreach (kitchenObjectSO_GameOjbect kitchenObjectSOGameOjbect in kitchenObjectSO_GameOjbectList)
        {
            kitchenObjectSOGameOjbect.gameObject.SetActive(false);
        }
    }

    private void PlatekitchenObject_OnIngreidentAdded(object sender, PlateKitchenObject.onInredientADDedEventArgs e)
    {
        foreach(kitchenObjectSO_GameOjbect kitchenObjectSOGameOjbect in kitchenObjectSO_GameOjbectList)
        {
            if(kitchenObjectSOGameOjbect.KitchenObjectSO==e.KitchenObjectSO)
            {
                kitchenObjectSOGameOjbect.gameObject.SetActive(true);
            }
        }
    }
}

 