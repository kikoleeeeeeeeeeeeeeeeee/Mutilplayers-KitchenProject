using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private const string NUMBER_POPUP = "NumberPopup";

    private Animator animator;

    private int previousCountDownNumber;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameManager.Instance.OnstateChanged += GameManager_OnstateChanged;
    }

    private void GameManager_OnstateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
     
    private void Update()
    {
        int coutdownNumber =(int)MathF.Ceiling(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = coutdownNumber.ToString();
        //tostring内可以在双引号内使用 F2和#.##  都表示小数点后两位 

        if (previousCountDownNumber != coutdownNumber)
        {
            previousCountDownNumber = coutdownNumber;
            animator.SetTrigger(NUMBER_POPUP);

            SoundMnager.Instance.PlayCountDownSound();
        }

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
