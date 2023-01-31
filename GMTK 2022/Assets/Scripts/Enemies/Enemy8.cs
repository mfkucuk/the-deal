using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy8 : Enemy
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy8Animator;
    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private float turnDuration = 3f;

    [SerializeField] private TMP_Text damageText;
    private SpriteRenderer rend;
    
    [SerializeField] private GameObject enemyCountDown;
    private TextTimer textTimer;

    private bool characterDead = false;

    private void Start()
    {
        enemyName = "Skeleton";
        maxHealth = health;
        turnToPlay = 1;
        isBoundToTurn = false;
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
        if (!characterDead)
        {
            state = STATE.CHASE;

            for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
            {
                for (int j = EnemyPos.y - 1; j <= EnemyPos.y + 1; j++)
                {
                    if (i >= 0 && i < 8 && j >= 0 && j < 8)
                    {
                        if (Grid.Instance.getTileAtPosition(i, j) == Player.Instance.PlayerPos)
                        {
                            state = STATE.ATTACK;
                            break;
                        }
                    }
                }
            }


            if (state == STATE.CHASE)
            {
                EnemyInfo.Instance.hideInfoTiles();
                MoveEnemy();
            }
            else
            {
                AttackPlayer();
            }
        }
        
    }

    // This is the simplest enemy which moves one at a time.
    public override void MoveEnemy()
    {
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

        // Remove occupied tiles.
        removeOccupiedTiles(freeTiles);

        if (freeTiles.Count == 0) { return; }

        // Chase Logic
        Tile targetTile = null;
        float minDistance = float.MaxValue;

        foreach (Tile entry in freeTiles)
        {
            if (Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2);
                targetTile = entry;
            }
        }

        // move the enemy after every check has been made
        Grid.Instance.enemyLocations.Remove(EnemyPos);
        Grid.Instance.enemyLocations[targetTile] = this;

        if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.PLAYER_TURN &&
            Player.Instance.atk_pat == Player.ATTACK_PATTERNS.SMITE && Player.Instance.mode == Player.MODE.ATTACK)
        {
            Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y).canTakeAction.SetActive(false);
            Grid.Instance.getTileAtPosition(targetTile.x, targetTile.y).canTakeAction.SetActive(true);

            Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y).Hitbox.SetActive(false);
            Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y).Shaker.resetShaking();
        }

        if (Turn_Controller.Instance.TurnState == Turn_Controller.TURN_STATE.PLAYER_TURN &&
            Player.Instance.move_pat == Player.MOVEMENT.HOOK && Player.Instance.mode == Player.MODE.MOVEMENT)
        {
            Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y).canTakeAction.SetActive(false);
            Grid.Instance.getTileAtPosition(targetTile.x, targetTile.y).canTakeAction.SetActive(true);

            Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y).Hitbox.SetActive(false);
            Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y).Shaker.resetShaking();
        }

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
        Player.Instance.TakeDamage(damage);
        enemy8Animator.SetTrigger("Attack");
    }

    public override void TakeTurn()
    {

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
            
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;
            EnemyInfo.Instance.hideInfoTiles();
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy8Animator, enemyCountDown));

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }
}
