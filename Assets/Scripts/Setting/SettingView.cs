using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    [SerializeField] private Toggle musicOn;
    [SerializeField] private Toggle soundOn;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider soundVolume;

    private void OnEnable()
    {
        musicOn.isOn = SoundManager.instance.bgmIsOn;
        soundOn.isOn = SoundManager.instance.soundIsOn;
        musicVolume.value = SoundManager.instance.currentBgmVolume;
        soundVolume.value = SoundManager.instance.currentSoundVolume;
    }
}
