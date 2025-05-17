using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>();
    private int currentWeaponIndex = 0;
    private Weapon currentWeapon;

    private void Start()
    {
        if (weapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }
    
    public void Switcher_Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            SwitchWeapon(1);
        }
        else if (scroll < 0f)
        {
            SwitchWeapon(-1);
        }
    }

    private void SwitchWeapon(int direction)
    {
        int newWeaponIndex = (currentWeaponIndex + direction + weapons.Count) % weapons.Count;
        EquipWeapon(newWeaponIndex);
    }
    
    private void EquipWeapon(int index)
    {
        currentWeaponIndex = index;
        currentWeapon = weapons[currentWeaponIndex];
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
