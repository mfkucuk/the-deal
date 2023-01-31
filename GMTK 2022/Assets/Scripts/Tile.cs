using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileRenderer;
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private Transform _tileHolder;
    
    private Transform _cameraPos;

    private Shake shaker;
    public Shake Shaker => shaker;
    public Vector2 tilePos;
    public Vector2 infoTilePos;
    
    public GameObject highlight;
    public GameObject canTakeAction;
    public GameObject Hitbox;
    public GameObject danger;

    public SpriteRenderer HighlightSpriteRenderer;
    public SpriteRenderer CanTakeActionSpriteRenderer;
    public SpriteRenderer HitboxSpriteRenderer;
    public SpriteRenderer DangerSpriteRenderer;

    public BoxCollider2D collider2D;

    private AttackDice attackDice;

    public int x, y;

    public void Init(int posX, int posY)
    {
        x = posX;
        y = posY;
        tilePos = new Vector2(posX, posY);
    }

    private void Start()
    {
        _cameraPos = Camera.main.transform;
        collider2D = GetComponent<BoxCollider2D>();
        shaker = FindObjectOfType<Shake>();
        attackDice = FindObjectOfType<AttackDice>();
        infoTilePos = transform.position;
    }

    void OnMouseEnter()
    {
        if (!Player.Instance.isAttacking)
        {
            Grid.Instance.getTileAtPosition(x, y).highlight.SetActive(true);
        }
        else
        {
            Player.Instance.PlayerMouseEnter(x, y, shaker);
        }

        if (Grid.Instance.enemyLocations != null)
        {
            foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
            {
                if (Grid.Instance.getTileAtPosition(x, y).Equals(entry.Key))
                {
                    EnemyInfo.Instance.showInfoTiles(entry.Value);
                }
            }
        }
        
    }

    void OnMouseExit()
    {
        if (!Player.Instance.isAttacking)
        {
            Grid.Instance.getTileAtPosition(x, y).highlight.SetActive(false);
        }
        else
        {
            Player.Instance.PlayerMouseExit(x, y, shaker);
        }

        if (Grid.Instance.enemyLocations != null)
        {
            foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
            {
                if (Grid.Instance.getTileAtPosition(x, y).Equals(entry.Key))
                {
                    EnemyInfo.Instance.hideInfoTiles();
                }
            }
        }
        
    }


    /*
     * OnMouseDown will handle all actions the player can make.
     * If the player have just threw a movement dice, clicking on the player will display tiles it can go.
     * If the player have just threw an attack dice, clicking on the player will diplay the tiles of effect of the strike.
     */
    private void OnMouseDown()
    {
        shaker.resetShaking();
        if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.PLAYER_TURN)
        {
            int posX = this.x;  // Tile x
            int posY = this.y;  // Tile y

            if (Player.Instance.mode == Player.MODE.MOVEMENT)
            {
                if (Grid.Instance.getTileAtPosition(x, y).canTakeAction.activeSelf == true)
                {
                    /*
                        * Move the character, and delete the red tiles. Clear the redTiles dictionary.
                        * Update OnClickPlayer to false.
                        */
                    //Player.Instance.pos.localPosition = Grid.Instance.getTileAtPosition(x, y).tilePos;
                    Tile jumpTile = null;
                    Tile clickedTile = Grid.Instance.getTileAtPosition(x, y);
                                     
                    if (Player.Instance.move_pat == Player.MOVEMENT.HOOK)
                    {
                        float minDistance = float.MaxValue;
                        List<Tile> locations = new List<Tile>();

                        for (int i = clickedTile.x - 1; i <= clickedTile.x + 1; i++)
                        {
                            for (int j = clickedTile.y - 1; j <= clickedTile.y + 1; j++)
                            {
                                if (i >= 0 && i < 8 && j >= 0 && j < 8 && !Grid.Instance.enemyLocations.ContainsKey(Grid.Instance.getTileAtPosition(i, j)))
                                {
                                    locations.Add(Grid.Instance.getTileAtPosition(i, j));
                                }
                            }
                        }

                        foreach (Tile tile in locations)
                        {
                            if (Mathf.Pow(tile.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(tile.y - Player.Instance.PlayerPos.y, 2) <= minDistance)
                            {
                                minDistance = Mathf.Pow(tile.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(tile.y - Player.Instance.PlayerPos.y, 2);
                                jumpTile = tile;
                            }
                        }
                    }
                    else
                    {
                        jumpTile = clickedTile;
                    }

                    Vector2 jumpVector = new Vector2((jumpTile.tilePos.x + TileHolder.Instance.x) * TileHolder.Instance.s,
                            (jumpTile.tilePos.y + TileHolder.Instance.y) * TileHolder.Instance.s);
                    _characterMovement.Jump(Player.Instance.transform, jumpVector, "JumpVoice");
                    Player.Instance.PlayerPos = jumpTile;
                    Player.Instance.CalculateAllDiagonalsOfPlayer(jumpTile.x, jumpTile.y, 8);
                    Player.Instance.CalculateAllSidesOfPlayer();

                    Player.Instance.currentPos = jumpVector;

                    for (int i = 0; i < Grid.Instance.gridSize; i++)
                    {
                        for (int j = 0; j < Grid.Instance.gridSize; j++)
                        {
                            Grid.Instance.grid2D[i, j].canTakeAction.SetActive(false);
                        }
                    }

                    Turn_Controller.Instance.TurnState = Turn_Controller.TURN_STATE.ENEMY_TURN;
                    Turn_Controller.Instance.CarryOutEnemyTurns();
                }
                else
                {
                    AudioManager.Instance.Play("FalseClicking");
                    shaker.DoShake(_cameraPos.transform);
                }
            }
            else if (Player.Instance.mode == Player.MODE.ATTACK)
            {
                /*
                    * Attack the orange tiles, reset isAttacking, clear the orangeTiles dictionary, clear the redTiles dictionary.
                    * Update OnClickPlayer to false.
                    */
                if (Grid.Instance.getTileAtPosition(x, y).canTakeAction.activeSelf == true)
                {
                    if (Player.Instance.atk_pat == Player.ATTACK_PATTERNS.CHARGE)
                    {
                        //Player.Instance.pos.localPosition = Grid.Instance.getTileAtPosition(x, y).tilePos;
                        Vector2 jumpVector = new Vector2((Grid.Instance.getTileAtPosition(x, y).tilePos.x + TileHolder.Instance.x) * TileHolder.Instance.s,
                            (Grid.Instance.getTileAtPosition(x, y).tilePos.y + TileHolder.Instance.y) * TileHolder.Instance.s);
                        _characterMovement.Jump(Player.Instance.transform, jumpVector, "JumpVoice");
                        Player.Instance.PlayerPos = Grid.Instance.getTileAtPosition(x, y);
                        Player.Instance.CalculateAllDiagonalsOfPlayer(x, y, 8);
                        Player.Instance.CalculateAllSidesOfPlayer();

                        Player.Instance.currentPos = jumpVector;
                    }

                    for (int i = 0; i < Grid.Instance.gridSize; i++)
                    {
                        for (int j = 0; j < Grid.Instance.gridSize; j++)
                        {
                            Grid.Instance.grid2D[i, j].canTakeAction.SetActive(false);
                        }
                    }

                    for (int i = 0; i < Grid.Instance.gridSize; i++)
                    {
                        for (int j = 0; j < Grid.Instance.gridSize; j++)
                        {
                            if (Grid.Instance.enemyLocations.TryGetValue(Grid.Instance.grid2D[i, j], out var enemy) && Grid.Instance.grid2D[i, j].Hitbox.activeSelf)
                            {
                                Player.Instance.IsAttacked = true;
                                AudioManager.Instance.Play("CharacterAttackSound");
                                
                                attackDice.PlayWeaponAnimation();

                                for (int b = 0; b < Player.Instance.currentAttackSkill.abilities.Count; b++)
                                {
                                    if (enemy != null)
                                    {
                                        if (Player.Instance.currentAttackSkill.abilities[b].DoAbility(enemy))
                                        {
                                            enemy._abilityAnims[b].GetComponent<SpriteRenderer>().sprite = Player.Instance.currentAttackSkill.ability[Player.Instance.currentAttackSkill.abilities[b].GetInd()].GetComponent<SpriteRenderer>().sprite;
                                            StartCoroutine(enemy.PlayAbilityAnimation(enemy._abilityAnims[b]));
                                        }
                                    }
                                }

                                EnemyInfo.Instance.hideInfoTiles();
                                EnemyInfo.Instance.showInfoTiles(enemy);
                                Player.Instance.dealDamage.Play();

                                enemy.TakeDamage(Player.Instance.currentAttackSkill.damage); // will change                              
                            }

                            Grid.Instance.getTileAtPosition(i, j).Hitbox.SetActive(false);
                        }
                    }

                    if (Player.Instance.IsAttacked)
                        Player.Instance.IsAttacked = false;
                    else
                        AudioManager.Instance.Play("Missing");
                    
                    Player.Instance.isAttacking = false;
                    Player.Instance.currentAttackSkill.HidePattern(x, y);
                    Turn_Controller.Instance.TurnState = Turn_Controller.TURN_STATE.ENEMY_TURN;
                    Turn_Controller.Instance.CarryOutEnemyTurns();
                }
                else
                {
                    AudioManager.Instance.Play("FalseClicking");
                    shaker.DoShake(_cameraPos.transform);
                }
            }
            
        }

        OnMouseEnter();
    }


}
