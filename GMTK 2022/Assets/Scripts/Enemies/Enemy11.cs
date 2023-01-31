using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Enemy11 : Enemy
{

    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy11Animator;

    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;

    private float turnDuration = 5f;

    private SpriteRenderer rend;
    private bool isFlying;
    [SerializeField] private GameObject enemyCountDown;
    private TextTimer textTimer;

    private bool characterDead = false;
    private ArrayList _attackPlaces;

    private void Start()
    {
        enemyName = "Gargoyle";
        health = 3;
        maxHealth = health;
        turnToPlay = 2;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();

        textTimer = enemyCountDown.GetComponent<TextTimer>();
        turnDuration = textTimer.val;

        textTimer.OnEnemyDo += OnEnemyDo;

        damage = 2;
        isFlying = false;
        _attackPlaces = new ArrayList();
    }

    private void OnDestroy()
    {
        textTimer.OnEnemyDo -= OnEnemyDo;
    }

    public void OnEnemyDo()
    {
        if (!characterDead)
        {
            isFlying = !isFlying;
            if (isFlying)
            {
                enemy11Animator.SetInteger("Fly", 1);
            }
            else
            {
                enemy11Animator.SetInteger("Fly", 0);
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
        Tile targetSide = null;
        float minDistanceDiagonal = float.MaxValue;

        foreach (Tile entry in Player.Instance.sides)
        {
            if (Mathf.Pow(EnemyPos.x - entry.x, 2) + Mathf.Pow(EnemyPos.y - entry.y, 2) <= minDistanceDiagonal)
            {
                minDistanceDiagonal = Mathf.Pow(EnemyPos.x - entry.x, 2) + Mathf.Pow(EnemyPos.y - entry.y, 2);
                targetSide = entry;
            }
        }


        Tile targetTile = null;
        float minDistance = float.MaxValue;
        foreach (Tile entry in freeTiles)
        {
            if (Mathf.Pow(entry.x - targetSide.x, 2) + Mathf.Pow(entry.y - targetSide.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - targetSide.x, 2) + Mathf.Pow(entry.y - targetSide.y, 2);
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
        foreach (Tile tile in _attackPlaces)
        {
            if (tile.Equals(Player.Instance.PlayerPos))
            {
                Player.Instance.TakeDamage(damage);
            }

            tile.danger.SetActive(false);
        }

        _attackPlaces.Clear();
    }

    public override void TakeTurn()
    {
        turnToPlay--;

        if (turnToPlay == 1)
        {
            _attackPlaces = ChargeAttack();
        }

        if (turnToPlay == 0)
        {
            EnemyInfo.Instance.hideInfoTiles();
            MoveEnemy();
            AttackPlayer();

            turnToPlay = 2;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (health <= 0) return;
        if (!isFlying || Player.Instance.currentAttackSkill is SmiteSkill)
        {
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
                foreach (Tile tile in _attackPlaces)
                {
                    tile.danger.SetActive(false);
                }

                _attackPlaces.Clear();
                EnemyInfo.Instance.hideInfoTiles();
                StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy11Animator));
                Grid.Instance.enemyLocations.Remove(EnemyPos);
                Grid.Instance.NumberOfEnemies--;

                if (Grid.Instance.NumberOfEnemies <= 0)
                {
                    base.AllEnemyDied();
                }
            }
        }
    }

    public ArrayList ChargeAttack()
    {
        var attackPlaces = new ArrayList();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == EnemyPos.x && j == EnemyPos.y)
                    && (i == EnemyPos.x || j == EnemyPos.y))
                {
                    attackPlaces.Add(Grid.Instance.getTileAtPosition(i, j));
                    Grid.Instance.getTileAtPosition(i, j).danger.SetActive(true);
                }
            }
        }

        return attackPlaces;
    }
}
