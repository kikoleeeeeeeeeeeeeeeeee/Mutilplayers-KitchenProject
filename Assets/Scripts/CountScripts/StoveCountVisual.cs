using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCountVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private StoveCount stoveCount;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    private void Start()
    {
        stoveCount.OnStateChanged += StoveCount_OnStateChanged;
    }

    private void StoveCount_OnStateChanged(object sender, StoveCount.onStateChangedEventArgs e)
    {
        bool showVisual = e.state==StoveCount.State.Frying||e.state==StoveCount.State.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
 