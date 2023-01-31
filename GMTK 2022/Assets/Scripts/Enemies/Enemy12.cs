using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy12 : Enemy
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy12Animator;
    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private float turnDuration = 4f;

    [SerializeField] private TMP_Text damageText;
    private SpriteRenderer rend;

    [SerializeField] private GameObject enemyCountDown;
    private TextTimer textTimer;

    private bool characterDead = false;

    private ArrayList _movePlaces;

    private void Start()
    {
        enemyName = "Mimic";
        maxHealth = health;
        health = 4;
        turnToPlay = 1;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();
        _movePlaces = new ArrayList();

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
            AttackPlayer();
        }

    }

    // This is the simplest enemy which moves one at a time.
    public override void MoveEnemy()
    {
        ArrayList freeTiles = new ArrayList();

        if (Player.Instance.mode == Player.MODE.ATTACK) { return; }

        if (Player.Instance.currentMoveSkill is Move1Skill)
        {
            freeTiles = MoveAll(1);
        }
        else if (Player.Instance.currentMoveSkill is Move2Skill)
        {
            freeTiles = MoveAll(2);
        }
        else if (Player.Instance.currentMoveSkill is Move3Skill)
        {
            freeTiles = MoveAll(3);
        }
        else if (Player.Instance.currentMoveSkill is Move4Skill)
        {
            freeTiles = MoveAll(4);
        }
        else if (Player.Instance.currentMoveSkill is TeleportSkill)
        {
            freeTiles = Teleport();
        }
        else if (Player.Instance.currentMoveSkill is LMoveSkill)
        {
            freeTiles = MoveL();
        }
        else if (Player.Instance.currentMoveSkill is HookSkill)
        {
            freeTiles = HookPlayer();
        }
        else if (Player.Instance.currentMoveSkill is CursedTeleportSkill)
        {
            freeTiles = CursedTeleport();
        }

        if (freeTiles.Count == 0) { return; }

        // Chase Logic
        Tile targetTile = null;
        float minDistance = float.MaxValue;

        if (!(Player.Instance.currentMoveSkill is CursedTeleportSkill))
        {
            foreach (Tile entry in freeTiles)
            {
                if (Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2) <= minDistance)
                {
                    minDistance = Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2);
                    targetTile = entry;
                }
            }
        }
        else
        {
            targetTile = (Tile)freeTiles[Random.Range(0, freeTiles.Count)];
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

        enemy12Animator.SetTrigger("Reflection");
    }

    public override void AttackPlayer()
    {
        for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
        {
            for (int j = EnemyPos.y - 1; j <= EnemyPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    if (Player.Instance.PlayerPos.Equals(Grid.Instance.getTileAtPosition(i, j)))
                    {
                        Player.Instance.TakeDamage(damage);
                    }
                }
            }
        }
        enemy12Animator.SetTrigger("Attack");
    }

    public override void TakeTurn()
    {
        turnToPlay--;
        
        if (turnToPlay == 0)
        {
            if (state == STATE.CHASE)
            {
                EnemyInfo.Instance.hideInfoTiles();
                MoveEnemy();
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
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy12Animator, enemyCountDown));

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }

    public ArrayList MoveAll(int step)
    {
        var movePlaces = new ArrayList();

        for (int i = EnemyPos.x - step; i <= EnemyPos.x + step; i++)
        {
            for (int j = EnemyPos.y - step; j <= EnemyPos.y + step; j++)
            {
                if (!(i == EnemyPos.x && j == EnemyPos.y) && (i >= 0 && i < 8 && j >= 0 && j < 8))
                {
                    movePlaces.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }

        removeOccupiedTiles(movePlaces);

        return movePlaces;
    }

    public ArrayList Teleport()
    {
        var movePlaces = new ArrayList();

        movePlaces.Add(Grid.Instance.getTileAtPosition(0, EnemyPos.y));
        movePlaces.Add(Grid.Instance.getTileAtPosition(7, EnemyPos.y));
        movePlaces.Add(Grid.Instance.getTileAtPosition(EnemyPos.x, 0));
        movePlaces.Add(Grid.Instance.getTileAtPosition(EnemyPos.x, 7));

        removeOccupiedTiles(movePlaces);

        return movePlaces;
    }

    public ArrayList MoveL()
    {
        var movePlaces = new ArrayList();

        for (int i = EnemyPos.x - 2; i <= EnemyPos.x + 2; i++)
        {
            for (int j = EnemyPos.y - 2; j <= EnemyPos.y + 2; j++)
            {
                if ((i >= 0 && i < 8 && j >= 0 && j < 8) && (i == EnemyPos.x - 2 || i == EnemyPos.x + 2 || j == EnemyPos.y - 2 || j == EnemyPos.y + 2))
                {
                    if ((EnemyPos.x + EnemyPos.y) % 2 == 0)
                    {
                        if ((i + j) % 2 == 1)
                        {
                            movePlaces.Add(Grid.Instance.getTileAtPosition(i, j));
                        }
                    }
                    else
                    {
                        if ((i + j) % 2 == 0)
                        {
                            movePlaces.Add(Grid.Instance.getTileAtPosition(i, j));
                        }
                    }
                }
            }
        }

        removeOccupiedTiles(movePlaces);

        return movePlaces;
    }

    public ArrayList CursedTeleport()
    {
        var movePlaces = new ArrayList();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                movePlaces.Add(Grid.Instance.getTileAtPosition(i, j));
            }
        }

        removeOccupiedTiles(movePlaces);

        return movePlaces;
    }

    public ArrayList HookPlayer()
    {
        var movePlaces = new ArrayList();

        float minDistance = float.MaxValue;
        Tile jumpTile = null;

        for (int i = Player.Instance.PlayerPos.x - 1; i <= Player.Instance.PlayerPos.x + 1; i++)
        {
            for (int j = Player.Instance.PlayerPos.y - 1; j <= Player.Instance.PlayerPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    Tile nearTile = Grid.Instance.getTileAtPosition(i, j);
                    if (Mathf.Pow(nearTile.x - EnemyPos.x, 2) + Mathf.Pow(nearTile.y - EnemyPos.y, 2) <= minDistance)
                    {
                        minDistance = Mathf.Pow(nearTile.x - EnemyPos.x, 2) + Mathf.Pow(nearTile.y - EnemyPos.y, 2);
                        jumpTile = nearTile;
                    }
                }
            }
        }

        movePlaces.Add(jumpTile);

        removeOccupiedTiles(movePlaces);

        return movePlaces;
    }
}
