using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Dialogues : MonoBehaviour, IUpdateObserver
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshPro textComponent;
    [SerializeField] private string[] lines;
    public float textSpeed;
    private int index;

    private bool inTrigger;
    private bool isDialogue;

    void Start()
    {
        inTrigger = false;
        isDialogue = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            buttonPanel.SetActive(true);
            inTrigger = true;           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IPlayer player = collision.GetComponent<IPlayer>();

        if (player != null)
        {
            buttonPanel.SetActive(false);
            inTrigger = false;
            EndDialogue();
        }      
    }

    private void OnEnable()
    {
        UpdateManager.AddToList(this);
    }

    private void OnDisable()
    {
        UpdateManager.RemoveFromList(this);
    }

    public void ObserveUpdate()
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

    private void StartDialogue()
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

    private void NextLine()
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

    private void EndDialogue()
    {
        isDialogue = false;

        dialoguePanel.SetActive(false);
    }
}
