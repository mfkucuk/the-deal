using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    private static Grid _instance;

    public static Grid Instance { get { return _instance; } }

    public int gridSize = 8;

    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Tile tileBottomPrefab;
    [SerializeField] private Tile tileUpLeftPrefab;
    [SerializeField] private Tile tileUpRightPrefab;

    [SerializeField] private GameObject tileHolder;
    [SerializeField] private DarkmageBoss _bossPrefab;
    public int NumberOfEnemies;

    [SerializeField] private Transform cam;

    public Dictionary<Tile, Enemy> enemyLocations;

    public Tile[,] grid2D = null;
    
    [HideInInspector]
    public bool GameStarted = false;
    public Action OnGameStarted;
    

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public IEnumerator StartFightScene()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine( GenerateGrid());
    }

    public IEnumerator DestroyGrids(float duration)
    {
        WaitForSeconds wfs = new WaitForSeconds(duration);

        for (int i = 0; i < gridSize; i++)
        {
            int k, l;
            
            for (int j = 0; j < gridSize / 2; j++)
            {
                yield return wfs;

                k = gridSize - j - 1;
                l = gridSize - i - 1;
                
                AudioManager.Instance.Play("SquareSpawn");
                Destroy(getTileAtPosition(l,k).gameObject);
                
                AudioManager.Instance.Play("SquareSpawn");
                Destroy(getTileAtPosition(i,j).gameObject);
                
            }
        }

    }
    
    public IEnumerator HideAllGrids(float duration)
    {
        WaitForSeconds wfs = new WaitForSeconds(duration);

        for (int i = 0; i < gridSize; i++)
        {
            int k, l;
            
            for (int j = 0; j < gridSize / 2; j++)
            {
                yield return wfs;

                k = gridSize - j - 1;
                l = gridSize - i - 1;

                getTileAtPosition(i, j).GetComponent<SpriteRenderer>().enabled = false;

                getTileAtPosition(i, j).HighlightSpriteRenderer.enabled = false;
                getTileAtPosition(i, j).CanTakeActionSpriteRenderer.enabled = false;
                getTileAtPosition(i, j).HitboxSpriteRenderer.enabled = false;
                getTileAtPosition(i, j).DangerSpriteRenderer.enabled = false;

                getTileAtPosition(l, k).GetComponent<SpriteRenderer>().enabled = false;
                
                getTileAtPosition(l, k).HighlightSpriteRenderer.enabled = false;
                getTileAtPosition(l, k).CanTakeActionSpriteRenderer.enabled = false;
                getTileAtPosition(l, k).HitboxSpriteRenderer.enabled = false;
                getTileAtPosition(l, k).DangerSpriteRenderer.enabled = false;

            }
        }

    }
    
    public IEnumerator DestroyEnemies(float duration)
    {
        WaitForSeconds wfs = new WaitForSeconds(duration);


        foreach (KeyValuePair<Tile, Enemy> entry in enemyLocations)
        {
            yield return wfs;
            if (entry.Value != null) Destroy(entry.Value.gameObject);
        }


    }

    IEnumerator GenerateGrid()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);
        
        grid2D = new Tile[gridSize, gridSize];
        enemyLocations = new Dictionary<Tile, Enemy>();

        for (int i = 0; i < gridSize; i++)
        {
            int k, l;
            
            for (int j = 0; j < gridSize / 2; j++)
            {
                yield return wfs;

                k = gridSize - j - 1;
                l = gridSize - i - 1;
                
                Tile newTile2;
                
                if (k == 0)
                {
                    newTile2 = Instantiate(tileBottomPrefab, new Vector3(l, k), Quaternion.identity);
                }
                else if (k == 7 && l == 0)
                {
                    newTile2 = Instantiate(tileUpLeftPrefab, new Vector3(l, k), Quaternion.identity);
                }
                else if (l == 7 && k == 7)
                {
                    newTile2 = Instantiate(tileUpRightPrefab, new Vector3(l, k), Quaternion.identity);
                }
                else
                {
                    newTile2 = Instantiate(tilePrefab, new Vector3(l, k), Quaternion.identity);
                }
                
                AudioManager.Instance.Play("SquareSpawn");
                newTile2.name = $"Tile {l} {k}";
                newTile2.Init(l, k);
                grid2D[l, k] = newTile2;
                newTile2.transform.SetParent(tileHolder.transform, false);
                
                Tile newTile;
                if (j == 0)
                {
                    newTile = Instantiate(tileBottomPrefab, new Vector3(i, j), Quaternion.identity);
                }
                else if (j == 7 && i == 0)
                {
                    newTile = Instantiate(tileUpLeftPrefab, new Vector3(i, j), Quaternion.identity);
                }
                else if (i == 7 && j == 7)
                {
                    newTile = Instantiate(tileUpRightPrefab, new Vector3(i, j), Quaternion.identity);
                }
                else
                {
                    newTile = Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity);
                }
                
                AudioManager.Instance.Play("SquareSpawn");
                newTile.name = $"Tile {i} {j}";
                newTile.Init(i, j);
                grid2D[i, j] = newTile;
                newTile.transform.SetParent(tileHolder.transform, false);
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (GameSceneData.Instance.GetGameSceneTutorial() == 2)
            StartCoroutine(Tutorial());
        else if(FightSceneData.Instance.GetBossFightIsReady() == 1)
            StartCoroutine(AddBoss());
        else
            StartCoroutine(AddEnemies());

        Player.Instance.BoardReady = true;
        
        //cam.transform.position = new Vector3((float) gridSize / 2 - .5f, (float) gridSize / 2 - .5f, -10);
    }

    IEnumerator AddEnemies()
    {       
        WaitForSeconds wfs = new WaitForSeconds(0.5f);
        
        List<Enemy> enemies = new List<Enemy>();

        int randomPoint = Random.Range(FightSceneManager.Instance.currLevelDifficulty.totalPointMin, FightSceneManager.Instance.currLevelDifficulty.totalPointMax+1);

        while (randomPoint > 0)
        {
            Enemy enemy = FightSceneManager.Instance.currLevelDifficulty.enemyPrefabs
                [Random.Range(0, FightSceneManager.Instance.currLevelDifficulty.enemyPrefabs.Length)];
            
            enemies.Add(enemy);
            
            if(enemy is Enemy1) 
                randomPoint -= 1;
            if(enemy is Enemy2) 
                randomPoint -= 2;
            if(enemy is Enemy3) 
                randomPoint -= 1;
            if(enemy is Enemy4) 
                randomPoint -= 1;
            if(enemy is Enemy5) 
                randomPoint -= 3;
            if(enemy is Enemy6) 
                randomPoint -= 2;
            if(enemy is Enemy7) 
                randomPoint -= 4;
            if(enemy is Enemy8) 
                randomPoint -= 1;
            if(enemy is Enemy9) 
                randomPoint -= 4;
            if(enemy is Enemy10) 
                randomPoint -= 3;
            if(enemy is Enemy11) 
                randomPoint -= 4;
            if(enemy is Enemy12) 
                randomPoint -= 5;

        }

        NumberOfEnemies = enemies.Count;
        
        for (int i = 0; i < NumberOfEnemies; i++)
        {
            yield return wfs;
            
            int xPos = Random.Range(0, 8);
            int yPos = Random.Range(5, 8);

            while (enemyLocations.ContainsKey(grid2D[xPos, yPos]))
            {
                xPos = Random.Range(0, 8);
                yPos = Random.Range(5, 8);

            }
            
            AudioManager.Instance.Play("CharacterSpawn");

            Vector2 initialPos = new Vector2((xPos + TileHolder.Instance.x) * TileHolder.Instance.s, (yPos + TileHolder.Instance.y) * TileHolder.Instance.s);
            Enemy newEnemy = Instantiate(enemies[i], initialPos, Quaternion.identity);
            newEnemy.EnemyPos = getTileAtPosition(xPos, yPos);
            enemyLocations[grid2D[xPos, yPos]] = newEnemy;
        }

        OnGameStarted?.Invoke();
        GameStarted = true;
    }
    
    IEnumerator Tutorial()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.5f);
        
        Enemy ucube = FightSceneManager.Instance.currLevelDifficulty.enemyPrefabs[0];

        yield return wfs;
        
        Vector2 initialPos1 = new Vector2((6 + TileHolder.Instance.x) * TileHolder.Instance.s, (6 + TileHolder.Instance.y) * TileHolder.Instance.s);
        Enemy newEnemy = Instantiate(ucube, initialPos1, Quaternion.identity);
        newEnemy.EnemyPos = getTileAtPosition(6, 6);
        enemyLocations[grid2D[6, 6]] = newEnemy;

        NumberOfEnemies = 1;
        
        OnGameStarted?.Invoke();
        GameStarted = true;
        
    }

    IEnumerator AddBoss()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.5f);

        yield return wfs;

        AudioManager.Instance.Play("CharacterSpawn");

        Enemy boss = FightSceneManager.Instance.currLevelDifficulty.enemyPrefabs[1];
        Enemy horse = FightSceneManager.Instance.currLevelDifficulty.enemyPrefabs[0];

        Vector2 initialPos1 = new Vector2((4 + TileHolder.Instance.x) * TileHolder.Instance.s, (7 + TileHolder.Instance.y) * TileHolder.Instance.s);
        Enemy newEnemy = Instantiate(boss, initialPos1, Quaternion.identity);
        newEnemy.EnemyPos = getTileAtPosition(4, 7);
        enemyLocations[grid2D[4, 7]] = newEnemy;

        yield return wfs;

        Vector2 initialPos2 = new Vector2((6 + TileHolder.Instance.x) * TileHolder.Instance.s, (6 + TileHolder.Instance.y) * TileHolder.Instance.s);
        Enemy newEnemy1 = Instantiate(horse, initialPos2, Quaternion.identity);
        newEnemy1.EnemyPos = getTileAtPosition(6, 7);
        enemyLocations[grid2D[6, 7]] = newEnemy1;

        yield return wfs;

        Vector2 initialPos3 = new Vector2((1 + TileHolder.Instance.x) * TileHolder.Instance.s, (6 + TileHolder.Instance.y) * TileHolder.Instance.s);
        Enemy newEnemy2 = Instantiate(horse, initialPos3, Quaternion.identity);
        newEnemy2.EnemyPos = getTileAtPosition(1, 7);
        enemyLocations[grid2D[1, 7]] = newEnemy2;

        yield return wfs;

        Vector2 initialPos4 = new Vector2((3 + TileHolder.Instance.x) * TileHolder.Instance.s, (5 + TileHolder.Instance.y) * TileHolder.Instance.s);
        Enemy newEnemy3 = Instantiate(horse, initialPos4, Quaternion.identity);
        newEnemy3.EnemyPos = getTileAtPosition(3, 6);
        enemyLocations[grid2D[3, 6]] = newEnemy3;

        NumberOfEnemies = 4;

        OnGameStarted?.Invoke();
        GameStarted = true;
    }

    public Tile getTileAtPosition(int i, int j)
    {
        if (grid2D == null)
            return null;
        
        if (i >= 0 && i < 8 && j >= 0 && j < 8) return grid2D[i, j];
        else return null;
    }

    public Enemy getEnemyAtTile(Tile tile)
    {
        if (enemyLocations.TryGetValue(tile, out var enemy))
        {
            return enemy;
        }

        return null;
    }

    public void CloseColliders()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(getTileAtPosition(i, j) != null)
                    getTileAtPosition(i, j).collider2D.enabled = false;
            }
        }
    }

    public void OpenColliders()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if(getTileAtPosition(i, j) != null)
                    getTileAtPosition(i, j).collider2D.enabled = true;
            }
        }
    }

    public void highlightTiles(int posX, int posY)
    {
        Grid.Instance.getTileAtPosition(posX, posY).highlight.SetActive(false);
        /*
         * MOVEMENT
         */
        if (Player.Instance.mode == Player.MODE.MOVEMENT)
        {
            switch (Player.Instance.move_pat)
            {
                // If the movement dice is in all ways.
                case Player.MOVEMENT.MOVE1:
                    Player.Instance.MoveInAllDirections(posX, posY, 1);
                    Player.Instance.step = 1;
                    break;

                case Player.MOVEMENT.MOVE2:
                    Player.Instance.MoveInAllDirections(posX, posY, 2);
                    Player.Instance.step = 2;
                    break;

                case Player.MOVEMENT.MOVE3:
                    Player.Instance.MoveInAllDirections(posX, posY, 3);
                    Player.Instance.step = 3;
                    break;

                case Player.MOVEMENT.MOVE4:
                    Player.Instance.MoveInAllDirections(posX, posY, 4);
                    Player.Instance.step = 4;
                    break;

                case Player.MOVEMENT.TELEPORT:
                    Player.Instance.TeleportToEdge(posX, posY);
                    break;

                case Player.MOVEMENT.LMOVE:
                    Player.Instance.MoveInL(posX, posY);
                    break;

                case Player.MOVEMENT.HOOK:
                    Player.Instance.HookEnemy(posX, posY);
                    break;
            }
        }
        /*
            * ATTACKING
            */

        else if (Player.Instance.mode == Player.MODE.ATTACK)
        {
            switch (Player.Instance.atk_pat)
            {
                // Single-tile attack in all ways.
                case Player.ATTACK_PATTERNS.SINGLE_STRIKE:
                case Player.ATTACK_PATTERNS.THREE_BY_THREE:
                case Player.ATTACK_PATTERNS.THRUST_2:
                case Player.ATTACK_PATTERNS.THRUST_3:
                case Player.ATTACK_PATTERNS.WHIRLWIND:
                case Player.ATTACK_PATTERNS.DAGGER:
                    Player.Instance.AttackInAllDirections(posX, posY, 1);
                    break;

                case Player.ATTACK_PATTERNS.SMITE:
                    Player.Instance.AttackCertainEnemy(posX, posY);
                    break;

                case Player.ATTACK_PATTERNS.CHARGE:
                    Player.Instance.AttackInAllDirections(posX, posY, 2);
                    break;

                case Player.ATTACK_PATTERNS.LONG_AOE:
                    Player.Instance.AttackInAllDirections(posX, posY, 3);
                    break;
            }
        }

        CloseColliders();
        OpenColliders();
    }


}
