using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class SoundMnager : MonoBehaviour
{

    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume"; 
    public static SoundMnager Instance { get; private set; }
    [SerializeField] private AudioCliprousce audioCliprousce;

    private float volume=1f;

    private void Awake()
    {
        Instance = this;
         volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    { 
        DliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DliveryManager.Instance.OnRecipefailed += DeliveryManager_OnRecipefailed;
        CuttingCount.onAnyCut += CuttingCount_onAnyCut;
        Player.OnAnyPickedSomething += Player_OnpickSomething;
        BaseCounter.onAnyObjectPlaceHere += BaseCounter_onAnyObjectPlaceHere;
        TranshCount.OnAnyobjectTrasnhed += TranshCount_OnAnyobjectTrasnhed;
    }

    private void TranshCount_OnAnyobjectTrasnhed(object sender, System.EventArgs e)
    {
        TranshCount transhCounter = sender as TranshCount;
        PlaySound(audioCliprousce.objectDrop, transhCounter.transform.position); 
    }

    private void BaseCounter_onAnyObjectPlaceHere(object sender, System.EventArgs  e)
    {
        BaseCounter baseCounter = sender  as BaseCounter;
        PlaySound(audioCliprousce.objectDrop, baseCounter.transform.position); 
     }

    private void Player_OnpickSomething(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioCliprousce.objectPickup, player.transform.position);
    }

    private void CuttingCount_onAnyCut(object sender, System.EventArgs e)
    {
         
        CuttingCount cuttingCount = sender as CuttingCount;
        PlaySound(audioCliprousce.chop,cuttingCount.transform.position);
    }

    private void DeliveryManager_OnRecipefailed(object sender, System.EventArgs e)
    {
        DeliveryCount deliveryCount = DeliveryCount.Instance;
        PlaySound(audioCliprousce.deliveryFailed,deliveryCount.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCount deliveryCount = DeliveryCount.Instance;
        PlaySound(audioCliprousce.deliverySuccess, deliveryCount.transform.position);
    }

    private void PlaySound(AudioClip audioClip,Vector3 position ,float volumeMultiplier =  1f)
    {
        AudioSource.PlayClipAtPoint(audioClip,position,volumeMultiplier*volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
    }

    public void PlayFootStepSound(Vector3 position,float volume)
    {
        PlaySound(audioCliprousce.footstep, position, volume);
    }
    public void PlayCountDownSound()
    {
        PlaySound(audioCliprousce.warning,Vector3.zero);
    }
    public void PlayWarningSound(Vector3  position)
    {
        PlaySound(audioCliprousce.warning, position);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save(); 
    }

    public float GetVolume() { 
        return volume;
    }
}
