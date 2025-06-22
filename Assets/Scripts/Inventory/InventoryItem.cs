using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;

    [SerializeField] public Item item;
    [SerializeField] private int cost;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI detailsText;
    [SerializeField] private Image detailsPanel;
    [SerializeField] private bool isPlayerWeapon = false;

    public void Start()
    {
        image = GetComponent<Image>();
        LoadItem();

        if (!isPlayerWeapon)
            LoadCost();

        SetText();
    }

    private void LoadItem()
    {
        image.sprite = item.sprite;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        LoadItem();
    }

    private void LoadCost()
    {
        costText.text = cost.ToString();
    }

    public void SetCost(int cost)
    {
        this.cost = cost;
    }

        

    private void SetText()
    {
        detailsText.text=item.name + "\n" + "Dmg: " + item.dmg.ToString() + "\n" + "Dmg duration: " + item.damageDuration + "\n" + "Stamina: " + item.neededStamina + "\n" + "Mana: " + item.neededMana;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detailsPanel.raycastTarget = false;
        detailsPanel.transform.SetParent(transform.root);
        detailsPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailsPanel.gameObject.SetActive(false);
        detailsPanel.raycastTarget = false;
        detailsPanel.transform.SetParent(this.gameObject.transform);
    }

    private void OnDisable()
    {
        detailsPanel.gameObject.SetActive(false);
        detailsPanel.raycastTarget = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(Player.Instance.goldSystem.ReduceGold(cost))
            {
                Player.Instance.hudUpdater.updateGold(Player.Instance.goldSystem.GetGoldAmount());

                SlotManager.Instance.SetPlayerWeapon(item);

                SetCost(0);
                LoadCost();
              
            }
        }
    }
}
