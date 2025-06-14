using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;

    [SerializeField] private Item item;
    [SerializeField] private int cost;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI detailsText;
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private bool isPlayerWeapon = false;

    public void Start()
    {
        image = GetComponent<Image>();
        image.sprite = item.sprite;

        if(!isPlayerWeapon)
            costText.text = cost.ToString();

        detailsPanel.layer = 1;
        SetText();
    }

    private void SetText()
    {
        detailsText.text=item.name + "\n" + "Dmg: " + item.dmg.ToString() + "\n" + "Dmg duration: " + item.damageDuration + "\n" + "Stamina: " + item.neededStamina + "\n" + "Mana: " + item.neededMana;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detailsPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailsPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Player.Instance.goldSystem.ReduceGold(cost);
        }
    }
}
