using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUi : MonoBehaviour
{
    [SerializeField] private StoveCount StoveCount;
    private void Start()
    {
        StoveCount.onProgressChanged += StoveCount_onProgressChanged;

        Hide();
    }

    private void StoveCount_onProgressChanged(object sender, Ihasprogress.onProgressCHANGedEventArgs e)
    {
        float burnShowProgress = .5f;
        bool show = StoveCount.IsFried()&& e.progressNormalized>= burnShowProgress;

        if (show)
        {
            Show();
        }
        else
        {
            Hide();
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
 