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
            GameManager.Instance.LoadCostList();
            shopPanel.SetActive(true);
            isShopActive = true;
            GameManager.Instance.PauseGame();

            isDialogue = true;
        }
        else
        {
            shopPanel.SetActive(false);
            isShopActive = false;
            GameManager.Instance.PlayGame();

            isDialogue = false;

            GameManager.Instance.SaveCostList();
            GameManager.Instance.SaveGame();
        }
        
    }
}
