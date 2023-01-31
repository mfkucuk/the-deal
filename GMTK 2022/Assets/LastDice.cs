using System.Collections;
using UnityEngine;

public class LastDice : MonoBehaviour
{
    public Dice dice;
    public GameObject sprite;
    public Animator diceThrowAnimator;
    
    public void InitLastDice()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        //sprite.SetActive(true);
        
        AudioManager.Instance.Play("CharacterTakeDamage");
        
        StartCoroutine(WaitAndRoll());
    }

    IEnumerator WaitAndRoll()
    {
        yield return new WaitForSeconds(1f);
        
        diceThrowAnimator.SetBool("ThrowDice", true);

        AudioManager.Instance.Play("ThrowDice");

        StartCoroutine(dice.RollTheDice());
    }

}
