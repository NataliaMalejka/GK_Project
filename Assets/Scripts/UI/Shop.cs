using UnityEngine;
using UnityEngine.UIElements;

public class Shop : Dialogues
{
    [SerializeField] GameObject shopPanel;
    private bool isShopActive = false;

    protected override void Start()
    {
        base.Start();
        shopPanel.SetActive(false);
    }

    protected override void EndDialogue()
    { 
        base.EndDialogue();

        if(!isShopActive)
        {
            shopPanel.SetActive(true);
            isShopActive = true;
            Time.timeScale = 0f;

            isDialogue = true;
        }
        else
        {
            shopPanel.SetActive(false);
            isShopActive = false;
            Time.timeScale = 1f;

            isDialogue = false;
        }
        
    }
}
