using System.Collections;
using UnityEngine;

public class Dice2 : MonoBehaviour
{


    // Array of dice sides sprites to load from Resources folder
    [SerializeField] private Sprite[] diceSides;
    public int result;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer rend;

    // Use this for initialization
    private void Start()
    {

        // Assign Renderer component
        rend = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
    }

    // If you left click over the dice then RollTheDice coroutine is started
    private void OnMouseDown()
    {
        if (Turn_Controller.TURN_STATE.NONE == Turn_Controller.Instance.TurnState)
        {
            StartCoroutine("RollTheDice");
        }
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {

        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide = 0;

        // Final side or value that dice reads in the end of coroutine
        int finalSide = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide = Random.Range(0, 3);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        finalSide = randomDiceSide + 1;
        result = finalSide;
        Turn_Controller.Instance.TurnState = Turn_Controller.TURN_STATE.PLAYER_TURN;
        Player.Instance.mode = Player.MODE.MOVEMENT;
        Player.Instance.step = finalSide;

        // Show final dice value in Console
        Grid.Instance.highlightTiles((int)Player.Instance.PlayerPos.tilePos.x, (int)Player.Instance.PlayerPos.tilePos.y);

    }




}
