using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;

    public int dmg;
    public int damageDuration;
    public float neededStamina;
    public int neededMana;
}
