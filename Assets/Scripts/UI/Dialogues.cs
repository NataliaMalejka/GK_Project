using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Dialogues : MonoBehaviour, IUpdateObserver
{
    [SerializeField] protected GameObject buttonPanel;
    [SerializeField] protected GameObject dialoguePanel;

    [SerializeField] protected TextMeshPro textComponent;
    [SerializeField] protected string[] lines;
    public float textSpeed;
    protected int index;

    protected bool inTrigger;
    protected bool isDialogue;

    protected virtual void Start()
    {
        inTrigger = false;
        isDialogue = false;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            buttonPanel.SetActive(true);
            inTrigger = true;           
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            buttonPanel.SetActive(false);
            inTrigger = false;
            isDialogue = false;
            dialoguePanel.SetActive(false);
        }      
    }

    protected void OnEnable()
    {
        UpdateManager.AddToList(this);
    }

    protected void OnDisable()
    {
        UpdateManager.RemoveFromList(this);
    }

    public virtual void ObserveUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(inTrigger)
            {
                if(!isDialogue)
                {
                    isDialogue = true;
                    buttonPanel.SetActive(false);
                    dialoguePanel.SetActive(true);
                    StartDialogue();
                }
                else if (textComponent.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }         
        }
    }

    protected void StartDialogue()
    {
        index = 0;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    protected void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    protected virtual void EndDialogue()
    {
        isDialogue = false;

        dialoguePanel.SetActive(false);
    }
}
