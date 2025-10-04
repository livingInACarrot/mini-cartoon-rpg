using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite icon;
    public int damage;
    public DamageType damageType;
}

public enum DamageType
{
    Chop,
    Crush,
    Stab
}
