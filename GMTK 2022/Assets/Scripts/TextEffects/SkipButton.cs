using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkipButton : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioSource hoverSound;
    [SerializeField] private Color color;

    private bool firstTime = true;

    private bool disableSkip;
    public bool DisavbleSkip => true;

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
        if (firstTime)
        {
            firstTime = false;
            
            DialogueManager.Instance.SkipDialogue();
            transform.parent.gameObject.SetActive(false);
        }
        
        
       // DialogueManager.Instance.SkipDialogue();
        
      //  if(disableSkip)
            //transform.parent.gameObject.SetActive(false);
      //  else
         //   transform.parent.gameObject.SetActive(true);
    }


}
