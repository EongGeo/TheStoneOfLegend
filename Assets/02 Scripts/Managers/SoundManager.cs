using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip mainMenuBGM;
    public AudioClip normalBGM;
    public AudioClip stageBGM;

    [Header("SFX")]
    public AudioClip buttonClickSFX;
    public AudioClip stoneThrowingSFX;
    public AudioClip stoneHitSFX;
    public AudioClip playerHitSFX;
    public AudioClip gameOverSFX;
    public AudioClip stageClearSFX;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClickSFX() 
    {
        sfxSource.PlayOneShot(buttonClickSFX);
    }

    public void PlayNormalBGM()
    {
        bgmSource.clip = normalBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void PlayMainMenuBGM() 
    {
        bgmSource.clip = mainMenuBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }
    public void PlayStageBGM()
    {
        bgmSource.clip = stageBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}

