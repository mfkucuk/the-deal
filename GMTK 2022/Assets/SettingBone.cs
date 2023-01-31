using UnityEngine;

public class SettingBone : MonoBehaviour
{
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private int index;
    
    private void OnMouseDown()
    {
        settingsMenu.SetVolume(index);
    }
    
}
