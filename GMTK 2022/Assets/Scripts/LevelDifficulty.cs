using UnityEngine;

[CreateAssetMenu(fileName = "LevelDifficulty", menuName = "ScriptableObjects/LevelDifficultyScriptableObject", order = 1)]
public class LevelDifficulty : ScriptableObject
{
    public int DifficultyLevel;

    public Enemy[] enemyPrefabs;

    public int timer;

    public int totalPointMin;
    public int totalPointMax;
    
    public bool timerCanInfinty;

}
