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
    [SerializeField] private Image detailsPanel;
    [SerializeField] private bool isPlayerWeapon = false;

    public void Start()
    {
        image = GetComponent<Image>();
        image.sprite = item.sprite;

        if(!isPlayerWeapon)
            costText.text = cost.ToString();

        SetText();
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
        Debug.Log("disable");
        detailsPanel.gameObject.SetActive(false);
        detailsPanel.raycastTarget = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(Player.Instance.goldSystem.ReduceGold(cost))
            {
                PlayerWeapon weapon = Player.Instance.controller.GetWeapon();

                weapon.SetSprite(item.sprite);
                weapon.SetDmg(item.dmg);
                weapon.SetDamageDuration(item.damageDuration);
                weapon.setNeededMana(item.neededMana);
                weapon.SetNeededStamina(item.neededStamina);
            }
        }
    }
}
