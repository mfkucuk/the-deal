using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbiBehav : MonoBehaviour
{
    [SerializeField] private GameObject pressE;
    

    public void openPressE()
    {
        pressE.SetActive(true);
    }

    public void closePressE()
    {
        pressE.SetActive(false);
    }

    private void StartDialogue()
    {
        DialogueTrigger.Instance.TriggerDialogue();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ABI");

        if (collision.CompareTag("Skull"))
        {
            openPressE();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Skull"))
        {
            closePressE();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Skull"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
            }
        }
    }
}
