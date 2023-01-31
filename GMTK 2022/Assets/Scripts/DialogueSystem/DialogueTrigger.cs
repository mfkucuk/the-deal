using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueTrigger : MonoBehaviour
{
    private static DialogueTrigger _instance;
    public static DialogueTrigger Instance { get { return _instance; } }

    [SerializeField] List<Dialogue> dialogueList;
    [SerializeField] List<int> effectTextIndex;

    private Queue<Dialogue> dialogueQueue;

    private int index = 0;

    public Action OnDialogudeTriggerInit;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
        dialogueQueue = new Queue<Dialogue>();

        int startIndex = 0;

        if (SceneData.Instance.GetCurrSceneName() == "GameScene")
            startIndex = DialogueData.Instance.GetSavedDialogueIndex();

        for(int i = startIndex; i < dialogueList.Count; i++)
            dialogueQueue.Enqueue(dialogueList[i]);
        
        OnDialogudeTriggerInit?.Invoke();

    }
    
    public void SetDialogue(Dialogue newDialogue, int effectDialogueHolderIndex)
    {
        dialogueList.Add(newDialogue);
        dialogueQueue.Enqueue(newDialogue);
        effectTextIndex.Add(effectDialogueHolderIndex);
    }

    public void TriggerDialogue()
    {
        if( (DialogueManager.Instance.isDialogueStarted || dialogueQueue.Count > 0) && !PuaseMenu.GameIsPaused)
        {
            if (!DialogueManager.Instance.isDialogueStarted)
            {
                DialogueManager.Instance.StartDialogue(dialogueQueue.Dequeue(), effectTextIndex[index++]);
            }
            else
                DialogueManager.Instance.DisplayNextSentence();
        }
    }   

}
