using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioSource hoverSound;
    [SerializeField] private Color color;

    private void OnMouseEnter()
    {
        hoverSound.Play();
        text.color = color;
    }

    private void OnMouseExit()
    {
        text.color = Color.white;
    }
    
    private void OnMouseDown()
    {
        LevelController.Instance.LoadMainMenu();
    }


}