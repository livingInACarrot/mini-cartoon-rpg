using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    private Weapon weapon;

    public void SetWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        GetComponent<Image>().sprite = newWeapon.icon;

        TMP_Text text = GetComponentInChildren<TMP_Text>();
        string type = newWeapon.damageType switch
        {
            DamageType.Chop => "�������",
            DamageType.Crush => "��������",
            DamageType.Stab => "�������",
            _ => "",
        };
        text.text = $"����: {weapon.damage}\n��� �����:\n{type}";
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }
}
