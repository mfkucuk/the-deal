using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
 
 public class Bone : MonoBehaviour
 {
     
     [SerializeField] bool mod;
 
     [SerializeField] Image[] bones;
     [SerializeField] SpriteRenderer[] sprites;
     
     private void Start()
     {
         int currHealth = SkullData.Instance.GetHealtDatas();
 
         if (mod)
         {
             for (int i = 0; i < currHealth; i++)
             {
                 bones[i].enabled = true;
                
             }
 
             for (int i = currHealth; i < bones.Length; i++)
             {
                 bones[i].enabled = false;
             }
         }
         else
         {
             for (int i = 0; i < currHealth; i++)
             {
                 sprites[i].enabled = true;
             }
 
             for (int i = currHealth; i < sprites.Length; i++)
             {
                 sprites[i].enabled = false;
             }
         }
 
 
     }
 
     public void UpdateBones()
     {
         int currHealth = SkullData.Instance.GetHealtDatas();
 
         if (mod)
         {
             for (int i = 0; i < currHealth; i++)
             {
                 bones[i].enabled = true;
             }
 
             for (int i = currHealth; i < bones.Length; i++)
             {
                 bones[i].enabled = false;
             }
         }
         else
         {
             for (int i = 0; i < currHealth; i++)
             {
                 sprites[i].enabled = true;
             }
 
             for (int i = currHealth; i < sprites.Length; i++)
             {
                 sprites[i].enabled = false;
             }
         }
 
     }
 
 }
