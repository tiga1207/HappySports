using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;

    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);

        bgmSlider.value = SoundManager.Instance.musicSoundVolume;
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.SetMusicSoundVolume(value);
    }
}