using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy7 : Enemy
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy7Animator;

    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float turnDuration = 10f;

    [SerializeField] private Enemy8 enemy8Prefab;
    
    [SerializeField] private GameObject enemyCountDown;
    private TextTimer textTimer;

    private SpriteRenderer rend;
    
    private bool characterDead = false;

    private void Start()
    {
        enemyName = "Summoner";
        maxHealth = health;
        turnToPlay = 1;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();
        
        textTimer = enemyCountDown.GetComponent<TextTimer>();
        turnDuration = textTimer.val;
        
        textTimer.OnEnemyDo += OnEnemyDo;
        damage = 1;
    }
    
    private void OnDestroy()
    {
        textTimer.OnEnemyDo -= OnEnemyDo;
    }

    public void OnEnemyDo()
    {
        if(!characterDead)
            AttackPlayer();
    }

    // This is the simplest enemy which moves one at a time.
    public override void MoveEnemy()
    {
        ArrayList freeTiles = new ArrayList();

        // Tiles enemy can go.
        for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
        {
            if (i != EnemyPos.x && (i >= 0 && i < 8))
            {
                freeTiles.Add(Grid.Instance.getTileAtPosition(i, EnemyPos.y));
            }
        }

        for (int i = EnemyPos.y - 1; i <= EnemyPos.y + 1; i++)
        {
            if (i != EnemyPos.y && (i >= 0 && i < 8))
            {
                freeTiles.Add(Grid.Instance.getTileAtPosition(EnemyPos.x, i));
            }
        }

        // Remove occupied tiles.
        removeOccupiedTiles(freeTiles);

        if (freeTiles.Count == 0) { return; }

        // Chase Logic (this enemy escapes from the player)
        Tile targetTile = null;
        float maxDistance = float.MinValue;

        foreach (Tile entry in freeTiles)
        {
            if (Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2) >= maxDistance)
            {
                maxDistance = Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2);
                targetTile = entry;
            }
        }

        // move the enemy after every check has been made
        Grid.Instance.enemyLocations.Remove(EnemyPos);
        Grid.Instance.enemyLocations[targetTile] = this;

        Vector2 jumpVector = new Vector2((targetTile.x + TileHolder.Instance.x) * TileHolder.Instance.s,
                            (targetTile.y + TileHolder.Instance.y) * TileHolder.Instance.s);
        
        if (bleedingCount > 0)
        {
            AudioManager.Instance.Play("EnemyTakeDamage");

            damageText.text = "1";

            int randomDamage = Random.Range(0, 2);

            if (randomDamage == 0)
                enemyTakeDamageAnimator.SetTrigger("TakeDamage1");
            else if (randomDamage == 1)
                enemyTakeDamageAnimator.SetTrigger("TakeDamage2");

            TakeDamage(1);
            bleedingCount--;
        }
        
        _characterMovement.Jump(transform, jumpVector, "JumpVoice");
        //pos.localPosition = targetTile.tilePos;
        if (EnemyPos != null) EnemyPos = targetTile;
    }

    public override void AttackPlayer()
    {
        /* 
            Summon two skeletons close to the players. 
         */
        enemy7Animator.SetTrigger("Attack");
        
        ArrayList freeTiles = new ArrayList();

        // Tiles enemy can go.
        for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
        {
            for (int j = EnemyPos.y - 1; j <= EnemyPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    freeTiles.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }

        removeOccupiedTiles(freeTiles);

        // Chase Logic
        Tile targetTile = null;
        float minDistance = float.MaxValue;
        
        foreach (Tile entry in freeTiles)
        {
            if (Mathf.Pow(entry.x - Player.Instance.transform.position.x, 2) + Mathf.Pow(entry.y - Player.Instance.transform.position.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - Player.Instance.transform.position.x, 2) + Mathf.Pow(entry.y - Player.Instance.transform.position.y, 2);
                targetTile = entry;
            }
        }

        Enemy skeleton1 = Instantiate(enemy8Prefab, new Vector2((targetTile.x + TileHolder.Instance.x) * TileHolder.Instance.s, (targetTile.y + TileHolder.Instance.y) * TileHolder.Instance.s), Quaternion.identity);
        skeleton1.EnemyPos = targetTile;
        Grid.Instance.enemyLocations[targetTile] = skeleton1;
        Grid.Instance.NumberOfEnemies++;

        if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.PLAYER_TURN &&
            Player.Instance.atk_pat == Player.ATTACK_PATTERNS.SMITE && Player.Instance.mode == Player.MODE.ATTACK)
        {
            Grid.Instance.getTileAtPosition(targetTile.x, targetTile.y).canTakeAction.SetActive(true);
        }

        if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.PLAYER_TURN &&
            Player.Instance.move_pat == Player.MOVEMENT.HOOK && Player.Instance.mode == Player.MODE.MOVEMENT)
        {
            Grid.Instance.getTileAtPosition(targetTile.x, targetTile.y).canTakeAction.SetActive(true);
        }
    }

    public override void TakeTurn()
    {
        turnToPlay--;

        if (turnToPlay == 0)
        {

            EnemyInfo.Instance.hideInfoTiles();
            MoveEnemy();
            turnToPlay = 2;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (health <= 0) return;
        health -= damage;

        AudioManager.Instance.Play("EnemyTakeDamage");

        damageText.text = damage.ToString();

        int randomDamage = Random.Range(0, 2);

        if (randomDamage == 0)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage1");
        else if (randomDamage == 1)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage2");

        if (health <= 0)
        {
            characterDead = true;
            
            EnemyInfo.Instance.hideInfoTiles();
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy7Animator, enemyCountDown));
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }
}
