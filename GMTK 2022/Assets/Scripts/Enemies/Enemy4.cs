using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy4 : Enemy
{
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy4Animator;
    
    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;
    
    private SpriteRenderer rend;

    private void Start()
    {
        enemyName = "Banshee";
        maxHealth = health;
        turnToPlay = 1;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();
        damage = 1;
    }

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
        Tile targetDiagonal = null;
        float minDistanceDiagonal = float.MaxValue; 

        foreach (Tile entry in Player.Instance.diagonals)
        { 
            if (Mathf.Pow(EnemyPos.x - entry.x, 2) + Mathf.Pow(EnemyPos.y - entry.y, 2) <= minDistanceDiagonal) {
                minDistanceDiagonal = Mathf.Pow(EnemyPos.x - entry.x, 2) + Mathf.Pow(EnemyPos.y - entry.y, 2);
                targetDiagonal = entry;
            }
        }


        Tile targetTile = null;
        float minDistance = float.MaxValue;
        foreach (Tile entry in freeTiles)
        {
            if (Mathf.Pow(entry.x - targetDiagonal.x, 2) + Mathf.Pow(entry.y - targetDiagonal.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - targetDiagonal.x, 2) + Mathf.Pow(entry.y - targetDiagonal.y, 2);
                targetTile = entry;
            }
        }

        // move enemy
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
        Player.Instance.TakeDamage(damage);
        enemy4Animator.SetTrigger("Attack");
    }

    public override void TakeTurn()
    {
        state = STATE.CHASE;

        foreach (Tile entry in Player.Instance.diagonals)
        {
            if (EnemyPos.Equals(entry))
            {
                state = STATE.ATTACK;
                break;
            }
        }

        turnToPlay--;

        if (turnToPlay == 0)
        {
            if (state == STATE.CHASE)
            {
                EnemyInfo.Instance.hideInfoTiles();
                MoveEnemy();
            }
            else
            {
                AttackPlayer();
            }

            turnToPlay = 1;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (health <= 0) return;
        health -= damage;
        
        AudioManager.Instance.Play("EnemyTakeDamage");
        
        damageText.text = damage.ToString();
        
        int randomDamage = Random.Range(0, 2);
        
        if(randomDamage == 0)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage1");
        else if(randomDamage == 1)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage2");

        if (health <= 0)
        {
            EnemyInfo.Instance.hideInfoTiles();
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy4Animator));
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }
}
