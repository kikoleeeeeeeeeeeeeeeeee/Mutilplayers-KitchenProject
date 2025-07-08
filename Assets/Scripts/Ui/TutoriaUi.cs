using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutoriaUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltnateText;
    [SerializeField] private TextMeshProUGUI pauseText;


    private void Start()
    {

        GameInput.Instance.OnBindingRebind += gameinput_OnBindingRebind;
        //GameManager.Instance.OnstateChanged += GameManager_OnstateChanged;
        GameManager.Instance.OnLocalPlayerReadyChange += GameManager_OnLocalPlayerReadyChange;
        UpdateVisual();

        Show();    
    }

    private void GameManager_OnLocalPlayerReadyChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Hide(); 
        }
    }


    private void gameinput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Intercat);
        interactAltnateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
