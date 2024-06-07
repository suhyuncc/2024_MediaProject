using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
//gh
public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private SoundInfo _soundinfo;
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider BGM;
    [SerializeField] private Slider EffectV;

    private void OnEnable()
    {
        BGM.value = _soundinfo.BGMvalue;
        EffectV.value = _soundinfo.SFXvalue;
    }

    public void SetBGMVolume()
    {
        float sound = BGM.value;

        if (sound == 0) Mixer.SetFloat("BGM", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("BGM", sound);
        }

        _soundinfo.BGMvalue = (int)(BGM.value);
    }

    public void SetEffectVolume()
    {

        float sound = EffectV.value;

        if (sound == 0) Mixer.SetFloat("SFX", -80);
        else
        {
            sound = -40 + sound * 4 / 10;
            Mixer.SetFloat("SFX", sound);
        }

        _soundinfo.SFXvalue = (int)(EffectV.value);
    }

}
