using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bones2 : MonoBehaviour
{

    [SerializeField] private SpriteRenderer[] bones;
    private void Start()
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
