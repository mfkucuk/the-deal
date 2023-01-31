using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class Dice3 : MonoBehaviour
{
    
    [SerializeField] private Sprite[] diceSides;
    public int result;
    
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private Animator diceAnimator;
    
    [SerializeField] private float duration;
    [SerializeField] private Vector3 lastScale;
    [SerializeField] private Vector3 rotationStrength;
    [SerializeField] private int vibrato;
    [SerializeField] private float random;

    private TweenerCore<Vector3, Vector3, VectorOptions> a;

    private void OnMouseDown()
    {
        if (Turn_Controller.TURN_STATE.NONE == Turn_Controller.Instance.TurnState)
        {
            StartCoroutine("RollTheDice");
        }
    }
    
    private void OnMouseEnter()
    {
        a = transform.DOScale(lastScale, duration).OnKill(() => {  a.Rewind();});
        //transform.DOShakeRotation(duration, rotationStrength);
        transform.DOShakePosition(duration, rotationStrength, vibrato, random);
    }

    private void OnMouseExit()
    {
        a.Kill();
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        diceAnimator.enabled = true;
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 6 (All inclusive)
            randomDiceSide = Random.Range(0, 7);

            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;
        result = finalSide;
        Turn_Controller.Instance.TurnState = Turn_Controller.TURN_STATE.PLAYER_TURN;
        Player.Instance.mode = Player.MODE.ATTACK;

        switch (finalSide)
        {
            case 1:
                Player.Instance.atk_pat = Player.ATTACK_PATTERNS.SINGLE_STRIKE;
                break;

            case 2:
                Player.Instance.atk_pat = Player.ATTACK_PATTERNS.THREE_BY_THREE;
                break;

            case 3:
                Player.Instance.atk_pat = Player.ATTACK_PATTERNS.WHIRLWIND;
                break;

            case 4:
                Player.Instance.atk_pat = Player.ATTACK_PATTERNS.THRUST_3;
                break;

            case 5:
                Player.Instance.atk_pat = Player.ATTACK_PATTERNS.SMITE;
                break;

            case 6:
                Player.Instance.atk_pat = Player.ATTACK_PATTERNS.LONG_AOE;
                break;

            case 7:
                Player.Instance.atk_pat = Player.ATTACK_PATTERNS.CHARGE;
                break;
        }
        diceAnimator.enabled = false;
        rend.sprite = diceSides[result-1];
        Grid.Instance.highlightTiles((int)Player.Instance.PlayerPos.tilePos.x, (int)Player.Instance.PlayerPos.tilePos.y);

    }


}