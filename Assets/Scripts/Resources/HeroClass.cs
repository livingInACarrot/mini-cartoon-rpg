using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero", menuName = "Game/Hero Class")]
public class HeroClass : ScriptableObject
{
    public string className;
    public HeroType type;
    public int healthPerLvl;
    public Weapon startingWeapon;
    public RuntimeAnimatorController animatorController;
    public List<string> features;
}

public enum HeroType
{
    Bandit,
    Barbarian,
    Warrior
}