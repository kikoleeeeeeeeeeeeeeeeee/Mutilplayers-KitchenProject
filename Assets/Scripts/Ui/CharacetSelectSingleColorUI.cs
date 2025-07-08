using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CharacetSelectSingleColorUI : MonoBehaviour
{
    [SerializeField] private int colorId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectGameObject;

    private void Awake()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            KitchenGameMutilplayer.Instance.ChangePlayerColor(colorId);
        });
    }

    private void Start()
    {
        KitchenGameMutilplayer.Instance.OnPlayerDataNetworkListChanged += KitchenGameMutilplayer_OnPlayerDataNetworkListChanged;
        image.color = KitchenGameMutilplayer.Instance.GetPlayerColor(colorId);
        UpdateIsSelected();
    }

    private void KitchenGameMutilplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if (KitchenGameMutilplayer.Instance.GetPlayerData().colorId == colorId)
        {
            selectGameObject.SetActive(true);
        }
        else
        {
            selectGameObject.SetActive(false);
        }    
    }
    private void OnDestroy()
    {
        KitchenGameMutilplayer.Instance.OnPlayerDataNetworkListChanged -= KitchenGameMutilplayer_OnPlayerDataNetworkListChanged;
    }
}
