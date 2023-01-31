using System.Collections;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }

    [SerializeField] private DialogueHolder[] dialogueHolders;
    public DialogueHolder activeDialogueHolder;

    [SerializeField] private float fastWriteSpeed = 0.04f;
    private bool isCoroutineEnd = true;
    private bool fastWrite = false;

    private int dialogueIndex = 0;
    private int maxSize;

    [SerializeField] private string startDialogueAnimationStateName;
    [SerializeField] private string endDialogueAnimationStateName;

    public Action<Dialogue> OnStartDialogueActions;
    public Func<int, RealDialogue> OnCustomDialogueActions;
    public Action OnOneDialogueEndActions;
    public Action OnEndDialogueActions;

    public bool isDialogueStarted = false;

    public bool DialogueStopGame { get; set; } = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void StartDialogue(Dialogue dialogue, int activeTextIndexInScene)
    {
        isDialogueStarted = true;
        maxSize = dialogue.sentences.Length;
        
        SetActiveTextInScene(activeTextIndexInScene);

        if (activeDialogueHolder.GetComponent<Animator>() != null)
            activeDialogueHolder.GetComponent<Animator>().Play(startDialogueAnimationStateName, 0);

        OnStartDialogueActions?.Invoke(dialogue);
        StartDialogueCustomActions();

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isCoroutineEnd == false)
        {
            fastWrite = true;
            return;
        }

        if (dialogueIndex == maxSize)
        {
            EndDialogue();
            return; 
        }

        RealDialogue realDialogue =  OnCustomDialogueActions?.Invoke(dialogueIndex);

        activeDialogueHolder.audioSource.clip = realDialogue.textAudios[dialogueIndex];
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(realDialogue));

    }

    IEnumerator TypeSentence(RealDialogue realDialogue)
    {
        if(!realDialogue.overWrite[dialogueIndex])
            activeDialogueHolder.dialogueHolderText.text = String.Empty;

        isCoroutineEnd = false;
       
        if(activeDialogueHolder.audioSource.loop == true)
            activeDialogueHolder.audioSource.Play();

        WaitForSecondsRealtime wfs = new WaitForSecondsRealtime(realDialogue.textWriteSpeeds[dialogueIndex]);
        WaitForSecondsRealtime wfsFast = new WaitForSecondsRealtime(fastWriteSpeed);


        foreach (char letter in realDialogue.sentences[dialogueIndex])
        {

            activeDialogueHolder.dialogueHolderText.text += letter;

            if (letter != ' ')
            {
                if (activeDialogueHolder.audioSource.loop == false)
                    activeDialogueHolder.audioSource.Play();
         
                if(fastWrite)
                    yield return wfsFast;
                else
                    yield return wfs;
            }

        }

        dialogueIndex++;
        fastWrite = false;
        isCoroutineEnd = true;
        OnOneDialogueEndActions?.Invoke();
    }

    private void EndDialogue()
    {
        isDialogueStarted = false;
        dialogueIndex = 0;

        if (activeDialogueHolder.GetComponent<Animator>() != null)
            activeDialogueHolder.GetComponent<Animator>().Play(endDialogueAnimationStateName, 0);

        OnEndDialogueActions?.Invoke();
        EndDialogueCustomActions();

    }

    private void SetActiveTextInScene(int index)
    {
        activeDialogueHolder.UnSubsActions();
        activeDialogueHolder = dialogueHolders[index];
        activeDialogueHolder.SubsActions();
    }

    public void SkipDialogue()
    {
        isCoroutineEnd = true;
        StopAllCoroutines();
        EndDialogue();
    }

    private void StartDialogueCustomActions() { }

    private void EndDialogueCustomActions()
    {
        if (SceneData.Instance.GetCurrSceneName() == "GameScene")
        {
            DialogueData.Instance.SetCurrDialogueIndex(DialogueData.Instance.GetCurrDialogueIndex() + 1); 
        }
    }


}