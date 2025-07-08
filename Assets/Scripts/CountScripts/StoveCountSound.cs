using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCountSound : MonoBehaviour
{
    [SerializeField] private StoveCount stoveCount;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCount.OnStateChanged += StoveCount_OnStateChanged;
        stoveCount.onProgressChanged += StoveCount_onProgressChanged;
    }

    private void StoveCount_onProgressChanged(object sender, Ihasprogress.onProgressCHANGedEventArgs e)
    {
        float burnShowProgress = .5f;
        playWarningSound = stoveCount.IsFried() && e.progressNormalized >= burnShowProgress;
    }

    private void StoveCount_OnStateChanged(object sender, StoveCount.onStateChangedEventArgs e)
    {
        bool playsound = e.state == StoveCount.State.Frying || e.state == StoveCount.State.Fried;
        if (playsound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundMnager.Instance.PlayWarningSound(stoveCount.transform.position);
            }
        }
    }
}
