using UnityEngine;
using UnityEngine.UIElements;

public class Shop : Dialogues
{
    [SerializeField] GameObject shopPanel;

    protected override void Start()
    {
        base.Start();
        shopPanel.SetActive(false);
    }

    protected override void EndDialogue()
    {
        base.EndDialogue();
        shopPanel.SetActive(true);
    }
}
