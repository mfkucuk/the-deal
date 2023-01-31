using System.Collections;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Transform savePoS;
    [SerializeField] private GameObject savedText;

    private void Start()
    {
        if(GameSceneData.Instance.GetSavePointOpened() == 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        SkullData.Instance.SetSavedPos(savePoS.position.x, savePoS.position.y);
        InventoryData.Instance.SaveData();
        SkillData.Instance.SaveData();
        GameSceneData.Instance.SetSavePointOpened(0);
        DialogueData.Instance.SetSavedDialogueIndex(2);
        savedText.SetActive(true);

        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
    
}
