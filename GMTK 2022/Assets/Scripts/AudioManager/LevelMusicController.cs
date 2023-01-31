using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.Play("FightMusic");
    }

}
