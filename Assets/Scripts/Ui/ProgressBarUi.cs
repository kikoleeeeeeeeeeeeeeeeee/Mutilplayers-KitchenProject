 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUi : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject hasprogressGameObject;
    [SerializeField] private Image barimage;
    private Ihasprogress hasProgress;
    private void Start()
    {
        hasProgress=hasprogressGameObject.GetComponent<Ihasprogress>();
        if (hasProgress == null)
        {
            Debug.LogError("111111");
        }
        hasProgress.onProgressChanged += hasProgress_onProgressChanged;
        
        barimage.fillAmount = 0f;
        Hide();
    }

    private void hasProgress_onProgressChanged(object sender, Ihasprogress.onProgressCHANGedEventArgs e)
    {
         barimage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)  
        {
            Hide();
        }
        else
        {
            Show();
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
