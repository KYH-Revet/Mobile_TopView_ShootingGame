using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    [Header("Volume Slider")]
    public Slider bgmVolumeSlider;
    public Slider effectVolumeSlider;

    private void Start()
    {
        // Volume value synchronization
        if (SoundManager.instance != null)
        {
            bgmVolumeSlider.value = SoundManager.instance.bgmVolume;
            effectVolumeSlider.value = SoundManager.instance.effectVolume;
        }
    }

    // Slide Functions

    /// <summary>BGM Slider 값 적용</summary>
    public void SetBGMVolume()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.SetBGMVolume(bgmVolumeSlider.value);
    }
    /// <summary>Effect Slider 값 적용</summary>
    public void SetEffectmVolume()
    {
        if(SoundManager.instance != null)
            SoundManager.instance.SetEffectVolume(effectVolumeSlider.value);
    }


    // Button Functions

    /// <summary>PlayerPrefs에 설정한 옵션값을 저장</summary>
    public void Save()
    {
        // Save the value
        PlayerPrefs.SetFloat("Volume_BGM", bgmVolumeSlider.value);
        PlayerPrefs.SetFloat("Volume_Effect", effectVolumeSlider.value);
        PlayerPrefs.Save();
    }
}
