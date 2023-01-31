using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] bones;

    [SerializeField] private Transition transition;

    private bool settingMenuStarting;
    
    private void Start()
    {
        StartCoroutine(transition.StartTransition());
        
        settingMenuStarting = true;
        SetVolume(SettingsData.Instance.GetSettingsBoneAmount());
    }

    public void SetVolume(int boneIndex)
    {

        if (boneIndex == SettingsData.Instance.GetSettingsBoneAmount() && !settingMenuStarting)
            boneIndex = -1;

        settingMenuStarting = false;
        
        for (int i = 0; i <= boneIndex; i++)
        {
            var tempColor = bones[i].GetComponent<Image>().color;

            tempColor.a = 1f;

            bones[i].GetComponent<Image>().color = tempColor;

        }
        
        for (int i = boneIndex+1; i < bones.Length; i++)
        {
            var tempColor = bones[i].GetComponent<Image>().color;

            tempColor.a = 0.5f;

            bones[i].GetComponent<Image>().color = tempColor;
        }
        
        if(boneIndex == -1)
            SettingsData.Instance.AudioMixer.SetFloat("Volume", -80);
        else if(boneIndex == bones.Length-1)
            SettingsData.Instance.AudioMixer.SetFloat("Volume", 0);
        else
            SettingsData.Instance.AudioMixer.SetFloat("Volume", -80 / (2*(boneIndex+1) ) );
        
        SettingsData.Instance.SetSettingsBoneAmount(boneIndex);
    }
   
}
