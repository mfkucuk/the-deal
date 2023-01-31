using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class MoveDice : MonoBehaviour
{
    [SerializeField] private Dice dice;
    [SerializeField] private Shake shakeController;

    [SerializeField] private Animator diceThrowAnimator;
    
    [SerializeField] private TMP_Text moveText;
    public Skill[] _movePrefabs;

    private Skill[] _moveSkills;

    [SerializeField] private AttackDice attackDice;

    private bool cantDice = true;
    private CharacterMovement _cm;

    public bool anyDiceRolling { get; set; } = false;

    public bool stopMoveDice { get; set; } = false;

    private void Start()
    {
        Enemy.OnAllEnemyDied += OnCantDice;
        Player.Instance.OnPlayerDeath += OnCantDice;

        dice.diceFaceAmount = InventoryData.Instance.GetMoveDiceFaceData();
        _moveSkills = new Skill[InventoryData.Instance.GetMoveDiceFaceData()];
        dice.diceSides = new Sprite[InventoryData.Instance.GetMoveDiceFaceData()];
        var index = 0;
        for (int i = 0; i < InventoryData.Instance.GetMoveSkillCount(); i++)
        {
            if (InventoryData.Instance.GetMoveSkillData(i) == 2)
            {
                Skill newSkill = Instantiate(_movePrefabs[i], new Vector2(-1000, -1000), Quaternion.identity);
                _moveSkills[index++] = newSkill;
            }       
        }

        for (int i = 0; i < _moveSkills.Length; i++)
        {
            dice.diceSides[i] = _moveSkills[i].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void OnDestroy()
    {
        Enemy.OnAllEnemyDied -= OnCantDice;
        Player.Instance.OnPlayerDeath -= OnCantDice;
    }

    private void OnCantDice()
    {
        cantDice = false;
    }

    private void Update()
    {
        if(Grid.Instance.GameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                shakeController.resetShaking();

                if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.NONE && cantDice && dice.CanDice && !stopMoveDice && !attackDice.anyDiceRolling && !PuaseMenu.GameIsPaused)
                {
                    dice.OnDiceRolled += OnDiceRolled;

                    anyDiceRolling = true;
                    moveText.enabled = false;

                    diceThrowAnimator.SetBool("ThrowDice", true);

                    AudioManager.Instance.Play("ThrowDice");

                    StartCoroutine(dice.RollTheDice());
                }
            }
        }

    }

    private void OnMouseEnter()
    {
        if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.NONE && dice.CanDice && Grid.Instance.GameStarted && cantDice && !stopMoveDice)
            shakeController.StartMouseOnShaking(transform);
    }

    private void OnMouseExit()
    {
        shakeController.resetShaking();
    }
    public void OnMouseDown()
    {
        if (Grid.Instance.GameStarted)
        {
            shakeController.resetShaking();
            dice.OnDiceRolled += OnDiceRolled;

            if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.NONE && cantDice && dice.CanDice && !stopMoveDice && !attackDice.anyDiceRolling && !PuaseMenu.GameIsPaused)
            {
                moveText.enabled = false;
                
                anyDiceRolling = true;
            
                diceThrowAnimator.SetBool("ThrowDice", true);
            
                AudioManager.Instance.Play("ThrowDice");
            
                StartCoroutine(dice.RollTheDice());
            }
        }

    }

    private void OnDiceRolled(int result)
    {
        diceThrowAnimator.SetBool("ThrowDice", false);

        anyDiceRolling = false;

        Turn_Controller.Instance.TurnState = Turn_Controller.TURN_STATE.PLAYER_TURN;
        _cm = FindObjectOfType<CharacterMovement>();

        Player.Instance.currentMoveSkill = _moveSkills[result - 1];

        Player.Instance.mode = Player.MODE.MOVEMENT;

        Player.Instance.move_pat = _moveSkills[result - 1].movePattern;
        dice.rend.sprite = _moveSkills[result - 1].skillImage;

        // Show final dice value in Console
        Grid.Instance.highlightTiles(Player.Instance.PlayerPos.x, Player.Instance.PlayerPos.y);

        dice.OnDiceRolled -= OnDiceRolled;

        if (Player.Instance.currentMoveSkill is CursedTeleportSkill)
        {
            ArrayList freeTiles = new ArrayList();
            Tile jumpTile = null;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    freeTiles.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }

            foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
            {
                freeTiles.Remove(entry.Key);
            }

            jumpTile = (Tile)freeTiles[Random.Range(0, freeTiles.Count)];

            Vector2 jumpVector = new Vector2((jumpTile.tilePos.x + TileHolder.Instance.x) * TileHolder.Instance.s,
                            (jumpTile.tilePos.y + TileHolder.Instance.y) * TileHolder.Instance.s);
            _cm.Jump(Player.Instance.transform, jumpVector, "JumpVoice");
            Player.Instance.PlayerPos = jumpTile;
            Player.Instance.CalculateAllDiagonalsOfPlayer(Player.Instance.PlayerPos.x, Player.Instance.PlayerPos.y, 10);
            Player.Instance.CalculateAllSidesOfPlayer();

            Player.Instance.currentPos = jumpVector;

            for (int i = 0; i < Grid.Instance.gridSize; i++)
            {
                for (int j = 0; j < Grid.Instance.gridSize; j++)
                {
                    Grid.Instance.grid2D[i, j].canTakeAction.SetActive(false);
                }
            }

            Player.Instance.currentMoveSkill = null;
            Turn_Controller.Instance.TurnState = Turn_Controller.TURN_STATE.ENEMY_TURN;
            Turn_Controller.Instance.CarryOutEnemyTurns();
        }
    }

}
