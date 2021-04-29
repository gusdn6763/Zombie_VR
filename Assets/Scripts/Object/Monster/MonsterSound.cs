using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MonsterSound : MonoBehaviour
{
    private AudioSource currentSound;
    public Dictionary<string, AudioClip> monsterSounds;

    private void Awake()
    {
        currentSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (SoundManager.instance.soundIsOn)
        {
            currentSound.mute = false;
            currentSound.volume = SoundManager.instance.currentSoundVolume;
        }
        else
        {
            currentSound.mute = true;
        }
        currentSound.Play();
    }

    public void ChangeSound(string name)
    {
        AudioClip tmp;
        if (monsterSounds.TryGetValue(name, out tmp))
        {
            currentSound.clip = tmp;
        }
        else
        {
            Debug.LogError("MonterSound의 클립이 없습니다");
        }
        currentSound.Play();
    }
}
