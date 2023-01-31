using UnityEngine;

public class BoneSpriteController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer[] sprites;
    
    [SerializeField] private Sprite[] brokenBones;
    [SerializeField] private Sprite[] defBones;

    [SerializeField] private Color closeColor;
     
    private void Start()
    {
        int currHealth = SkullData.Instance.GetHealtDatas();


        for (int i = 0; i < currHealth; i++)
        { 
            sprites[i].sprite = defBones[i];
            sprites[i].color = Color.white;
        }
        
        for (int i = currHealth; i < sprites.Length; i++)
        {
            sprites[i].sprite = brokenBones[i];
            sprites[i].color = closeColor;
        }

    }
 
    public void UpdateBones()
    {
        int currHealth = SkullData.Instance.GetHealtDatas();
        
        for (int i = 0; i < currHealth; i++)
        { 
            sprites[i].sprite = defBones[i];
            sprites[i].color = Color.white;
        }
 
        for (int i = currHealth; i < sprites.Length; i++)
        { 
            sprites[i].sprite = brokenBones[i];
            sprites[i].color = closeColor;
        }

    }
 
}
