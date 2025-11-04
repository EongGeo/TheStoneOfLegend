using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSettingSystem : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 메인 메뉴 씬일 때만 슬라이더 찾기
        if (scene.name == "MainMenuScene")
        {
            Slider[] sliders = FindObjectsOfType<Slider>(true);
            foreach (var slider in sliders)
            {
                if (slider.name == "BGMSlider")
                    bgmSlider = slider;
                else if (slider.name == "SFXSlider")
                    sfxSlider = slider;
            }

            if (bgmSlider != null)
            {
                float bgmValue = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
                bgmSlider.value = bgmValue;
                mixer.SetFloat("BGMVolume", Mathf.Log10(bgmValue) * 20);
                bgmSlider.onValueChanged.AddListener(SetBGMVolume);
            }

            if (sfxSlider != null)
            {
                float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
                sfxSlider.value = sfxValue;
                mixer.SetFloat("SFXVolume", Mathf.Log10(sfxValue) * 20);
                sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            }
        }
    }


    private void SetBGMVolume(float value)
    {
        mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    private void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    /*
     * 사람의 청각은 선형이 아니라 로그 스케일에 가깝기 때문에 Mathf.Log10(value) * 20을 사용해야 자연스러운 볼륨 조절이 됨.
     */
}
