using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DarkmageBoss : Enemy
{

    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator darkmageAnimator;

    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;

    [SerializeField] private float turnDuration = 5f;
    [SerializeField] private GameObject enemyCountDown;
    private TextTimer textTimer;
    
    private bool characterDead = false;

    [SerializeField] private DarkmageBossClone _clonePrefab;
    private ArrayList _clones;

    private SpriteRenderer rend;
    private ArrayList _attackPlaces;

    private bool phase1Complete = false;

    private void Start()
    {
        enemyName = "Darkmage";
        health = 30;
        maxHealth = health;
        turnToPlay = 1;
        isBoundToTurn = true;
        damage = 2;

        _attackPlaces = new ArrayList();
        _clones = new ArrayList();

        rend = GetComponent<SpriteRenderer>();
        
        textTimer = enemyCountDown.GetComponent<TextTimer>();
        turnDuration = textTimer.val;
        
        textTimer.OnEnemyDo += OnEnemyDo;
    }
    
    private void OnDestroy()
    {
        textTimer.OnEnemyDo -= OnEnemyDo;
    }
    
    public void OnEnemyDo()
    {
        if (!characterDead)
        {
            if (health > 20)
            {
                // Attack 5x5
                for (int i = EnemyPos.x - 2; i <= EnemyPos.x + 2; i++)
                {
                    for (int j = EnemyPos.y - 2; j <= EnemyPos.y + 2; j++)
                    {
                        if (i >= 0 && i < 8 && j >= 0 && j < 8)
                        {
                            if (Grid.Instance.getTileAtPosition(i, j).Equals(Player.Instance.PlayerPos))
                            {
                                Player.Instance.TakeDamage(damage);
                            }
                        }
                    }
                }

                darkmageAnimator.SetTrigger("Attack");
            }
        }
    }
    
    public override void MoveEnemy()
    {
        ArrayList freeTiles = new ArrayList();

        if (health > 20)
        {
            for (int i = EnemyPos.x - 2; i <= EnemyPos.x + 2; i++)
            {
                for (int j = EnemyPos.y - 2; j <= EnemyPos.y + 2; j++)
                {
                    if (i >= 0 && i < 8 && j >= 0 && j < 8)
                    {
                        freeTiles.Add(Grid.Instance.getTileAtPosition(i, j));
                    }
                }
            }
        }
        else if (health > 10)
        {
            // Second-phase movement
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0 || j == 0 || i == 7 || j == 7)
                    {
                        freeTiles.Add(Grid.Instance.getTileAtPosition(i, j));
                    }
                }
            }
        }
        else
        {
            // Third-phase movement
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
        }

        // Remove occupied tiles.
        removeOccupiedTiles(freeTiles);

        if (freeTiles.Count == 0) { return; }

        // Chase Logic
        Tile targetTile = null;
        float minDistance = float.MaxValue;

        if (health <= 20 && health > 10)
        {
            targetTile = (Tile)freeTiles[Random.Range(0, freeTiles.Count)];
        }
        else
        {
            foreach (Tile entry in freeTiles)
            {
                if (Mathf.Pow(entry.x - Player.Instance.transform.position.x, 2) + Mathf.Pow(entry.y - Player.Instance.transform.position.y, 2) <= minDistance)
                {
                    minDistance = Mathf.Pow(entry.x - Player.Instance.transform.position.x, 2) + Mathf.Pow(entry.y - Player.Instance.transform.position.y, 2);
                    targetTile = entry;
                }
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

            health--;
            bleedingCount--;
        }

        _characterMovement.Jump(transform, jumpVector, "JumpVoice");
        //pos.localPosition = targetTile.tilePos;
        if (EnemyPos != null) EnemyPos = targetTile;
    }
    public override void AttackPlayer()
    {
        if (health > 20)
        {
            // this is handled in OnEnemyDo
        }
        else if (health > 10)
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
        else
        {
            darkmageAnimator.SetTrigger("Attack");
            Player.Instance.TakeDamage(damage);
        }
    }
    public override void TakeTurn()
    {
        /* (when hp > 5) The boss will use a 3x3 long range attack if the player is far away.
         * and teleport when the player gets close to it.
         * 
         * (when hp > 0) The boss will create 3 clones of itself. Everytime the real boss takes
         * damage positions will randomized. */

        turnToPlay--;

        if (health > 20)
        {
            if (turnToPlay == 0)
            {
                EnemyInfo.Instance.hideInfoTiles();
                MoveEnemy();
                turnToPlay = 2;
            }
        }
        else if (health > 10)
        {
            enemyCountDown.SetActive(false);
            state = STATE.ATTACK;
            for (int i = EnemyPos.x - 2; i <= EnemyPos.x + 2; i++)
            {
                for (int j = EnemyPos.y - 2; j <= EnemyPos.y + 2; j++)
                {
                    if (i >= 0 && i < 8 && j >= 0 && j < 8)
                    {
                        if (Grid.Instance.getTileAtPosition(i, j) == Player.Instance.PlayerPos)
                        {
                            state = STATE.CHASE;
                            break;
                        }
                    }
                }
            }

            if (turnToPlay == 1 && state == STATE.ATTACK)
            {
                _attackPlaces = ChargeAttack();
            }

            if (turnToPlay == 0)
            {
                if (state == STATE.CHASE)
                {
                    EnemyInfo.Instance.hideInfoTiles();
                    darkmageAnimator.SetTrigger("Tp");
                }

                AttackPlayer();

                turnToPlay = 2;
            }
        }
        else
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

        if (health <= 20 && health > 10)
        {
            while (Grid.Instance.enemyLocations.Count != 1)
            {
                Enemy e = null;
                foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
                {
                    if (entry.Value is Enemy5 || entry.Value is Enemy6)
                    {
                        e = entry.Value;
                        break;
                    }
                }

                e.RemoveEnemy();
                Destroy(e.gameObject);
            }
        }

        if (health <= 10 && health > 0)
        {
            foreach (Tile tile in _attackPlaces)
            {
                tile.danger.SetActive(false);
            }
            damageText.enabled = false;
            var spawnTiles = new ArrayList();

            foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
            {
                if (entry.Value is DarkmageBossClone)
                {
                    _clones.Add(entry.Value);
                }
            }

            foreach (DarkmageBossClone clone in _clones)
            {
                clone.RemoveEnemy();
                Destroy(clone.gameObject);
            }

            _clones.Clear();

            spawnTiles.Add(Grid.Instance.getTileAtPosition(0, 0));
            spawnTiles.Add(Grid.Instance.getTileAtPosition(0, 7));
            spawnTiles.Add(Grid.Instance.getTileAtPosition(7, 0));
            spawnTiles.Add(Grid.Instance.getTileAtPosition(7, 7));

            Grid.Instance.enemyLocations.Remove(EnemyPos);

            while (spawnTiles.Count != 1)
            {
                int randomNumber = Random.Range(0, spawnTiles.Count);
                Tile tile = (Tile)spawnTiles[randomNumber];
                spawnTiles.RemoveAt(randomNumber);

                var clone = Instantiate(_clonePrefab, new Vector2((tile.tilePos.x + TileHolder.Instance.x) * TileHolder.Instance.s, (tile.tilePos.y + TileHolder.Instance.y) * TileHolder.Instance.s), Quaternion.identity);
                clone.EnemyPos = tile;
                Grid.Instance.enemyLocations[tile] = clone;
            }

            Grid.Instance.NumberOfEnemies += 3;

            Tile bossTile = (Tile)spawnTiles[0];
            spawnTiles.RemoveAt(0);
            Grid.Instance.enemyLocations[bossTile] = this;
            transform.position = new Vector2((bossTile.tilePos.x + TileHolder.Instance.x) * TileHolder.Instance.s, (bossTile.tilePos.y + TileHolder.Instance.y) * TileHolder.Instance.s);
            EnemyPos = bossTile;
            turnToPlay = 2;
        }

        if (health <= 0)
        {
            foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
            {
                if (entry.Value is DarkmageBossClone)
                {
                    _clones.Add(entry.Value);
                }
            }

            foreach (DarkmageBossClone clone in _clones)
            {
                clone.RemoveEnemy();
                Destroy(clone.gameObject);
            }

            EnemyInfo.Instance.hideInfoTiles();
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, darkmageAnimator));
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }

    private ArrayList ChargeAttack()
    {
        var attackPlaces = new ArrayList();
        List<AttackDelegate> patterns = new List<AttackDelegate>();
        patterns.Add(new AttackDelegate(SquareAttack));
        patterns.Add(new AttackDelegate(BishopAttack));
        patterns.Add(new AttackDelegate(KnightAttack));
        patterns.Add(new AttackDelegate(OddAttack));
        patterns.Add(new AttackDelegate(HorizontalAttack));
        patterns.Add(new AttackDelegate(VerticalAttack));

        int rand = Random.Range(0, patterns.Count);
        patterns[rand](attackPlaces);
        patterns.RemoveAt(rand);

        rand = Random.Range(0, patterns.Count);
        patterns[rand](attackPlaces);
        patterns.RemoveAt(rand);

        foreach (Tile tile in attackPlaces)
        {
            tile.danger.SetActive(true);
        }

        darkmageAnimator.SetTrigger("Cast");

        return attackPlaces;
    }

    public delegate void AttackDelegate(ArrayList ap);

    private void SquareAttack(ArrayList ap)
    {
        for (int i = Player.Instance.PlayerPos.x - 1; i <= Player.Instance.PlayerPos.x + 1; i++)
        {
            for (int j = Player.Instance.PlayerPos.y - 1; j <= Player.Instance.PlayerPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    ap.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }
    }

    private void BishopAttack(ArrayList ap)
    {
        int i = 0;
        int j = 0;
        for (; i <= 7 && j <= 7; i++, j++)
        {
            if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == EnemyPos.x && j == EnemyPos.y))
            {
                ap.Add(Grid.Instance.getTileAtPosition(i, j));
            }
        }
        i = 7;
        j = 0;
        for (; i >= 0 && j <= 7; i--, j++)
        {
            if ((i >= 0 && j >= 0 && i < 8 && j < 8) && !(i == EnemyPos.x && j == EnemyPos.y))
            {
                ap.Add(Grid.Instance.getTileAtPosition(i, j));
            }
        }
    }

    private void KnightAttack(ArrayList ap)
    {
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if ((i >= 0 && j >= 0 && i < 8 && j < 8) && (i == EnemyPos.x || j == EnemyPos.y) && !(i == EnemyPos.x && j == EnemyPos.y))
                {
                    ap.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }
    }

    private void OddAttack(ArrayList ap)
    {
        int n = Random.Range(0, 2);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i + j) % 2 == n)
                {
                    ap.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }
    }
    
    private void HorizontalAttack(ArrayList ap)
    {
        int n = Random.Range(0, 2);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (i % 2 == n)
                {
                    ap.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }
    }

    private void VerticalAttack(ArrayList ap)
    {
        int n = Random.Range(0, 2);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (j % 2 == n)
                {
                    ap.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }
    }

    public override IEnumerator DeathAnimation(SpriteRenderer rend, ParticleSystem ps, Animator a, GameObject countDown = null)
    {
        AudioManager.Instance.Play("EnemyDead");

        if (countDown != null)
            countDown.SetActive(false);

        a.SetTrigger("Dead");
        ps.Play();

        yield return null;
    }
}
