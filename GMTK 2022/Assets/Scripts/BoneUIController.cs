using UnityEngine;
using UnityEngine.UI;

public class BoneUIController : MonoBehaviour
{

    [SerializeField] Image[] bones;

    private void Start()
    {
        if (GameSceneData.Instance.GetGameSceneTutorial() == 1)
        {
            this.gameObject.SetActive(false);
        }
        
        Init();
    }

    private void Init()
    {
        int currHealth = SkullData.Instance.GetHealtDatas();


        for (int i = 0; i < currHealth; i++)
        {
            bones[i].enabled = true;
        }

        for (int i = currHealth; i < bones.Length; i++)
        {
            bones[i].enabled = false;
        }
    }

    public void UpdateBones()
    {
        int currHealth = SkullData.Instance.GetHealtDatas();
        
        for (int i = 0; i < currHealth; i++)
        {
            bones[i].enabled = true;
        }

        for (int i = currHealth; i < bones.Length; i++)
        { 
            bones[i].enabled = false;
        }

    }

}