using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingeUi : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image image;
    public void SetKitchenObjectSo(KitchenObjectSO kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
