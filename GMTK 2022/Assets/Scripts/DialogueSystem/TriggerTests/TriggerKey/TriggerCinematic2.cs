using UnityEngine;
using TMPro;

public class TriggerCinematic2 : MonoBehaviour
{
    [SerializeField] private DialogueHolder dialogueHolder;

    [SerializeField] private GameObject skipButton;
    
    //[SerializeField] private GameObject backGroundAnim;
    [SerializeField] private TMP_Text dialogueText;

    private void Start()
    {
        dialogueHolder.HolderOnStartDialogueActions += HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions += HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions += HolderOnEndDialogueActions;
    }

    private void OnDestroy()
    {
        dialogueHolder.HolderOnStartDialogueActions -= HolderOnStartDialogueActions;
        dialogueHolder.HolderOnCustomDialogueActions -= HolderOnCustomDialogueActions;
        dialogueHolder.HolderOnEndDialogueActions -= HolderOnEndDialogueActions;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            DialogueTrigger.Instance.TriggerDialogue();

    }
    
    private void HolderOnStartDialogueActions()
    {
        skipButton.SetActive(true);
    }
    
    private void HolderOnCustomDialogueActions(RealDialogue realDialogue, int index)
    {
        if (index == 4)
            dialogueText.color = Color.black;
    }
    
    private void HolderOnEndDialogueActions()
    {
        skipButton.SetActive(false);
        //backGroundAnim.SetActive(false);
    }
    

}
