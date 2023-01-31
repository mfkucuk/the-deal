using UnityEngine;

public class TriggerInventory : MonoBehaviour
{
    private bool canTriggerDialogue = false;
    private void Start()
    {
        DialogueTrigger.Instance.OnDialogudeTriggerInit += OnDialogudeTriggerInit;
    }

    private void OnDialogudeTriggerInit()
    {
        if (GameSceneData.Instance.GetInventoryOpened() == 0)
        {
            canTriggerDialogue = true;
            DialogueTrigger.Instance.TriggerDialogue();
            GameSceneData.Instance.SetInventoryOpened(1);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        DialogueTrigger.Instance.OnDialogudeTriggerInit -= OnDialogudeTriggerInit;
    }

    private void OnDestroy()
    {
        DialogueTrigger.Instance.OnDialogudeTriggerInit -= OnDialogudeTriggerInit;
    }

    private void Update()
    {
        if(canTriggerDialogue && Input.GetKeyDown(KeyCode.Space))
            DialogueTrigger.Instance.TriggerDialogue();
    }

}