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
    public AudioClip buttonClick2SFX;
    public AudioClip stoneThrowingSFX;
    public AudioClip enemyHitSFX;
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
    public void PlayButton2ClickSFX()
    {
        sfxSource.PlayOneShot(buttonClick2SFX);
    }
    public void PlayStoneThrowingSFX()
    {
        sfxSource.PlayOneShot(stoneThrowingSFX);
    }
    public void PlayEnemyHitSFX()
    {
        sfxSource.PlayOneShot(enemyHitSFX);
    }
    public void PlayPlayerHitSFX()
    {
        sfxSource.PlayOneShot(playerHitSFX);
    }
    public void PlayStageClearSFX()
    {
        sfxSource.PlayOneShot(stageClearSFX);
    }
    public void PlayGameOverSFX()
    {
        sfxSource.PlayOneShot(gameOverSFX);
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

