using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy Class")]
public class EnemyClass : ScriptableObject
{
    public string className;
    public EnemyType type;
    public int health;
    public int damage;
    public int strength;
    public int agility;
    public int stamina;
    public Weapon drop;
    public RuntimeAnimatorController animatorController;
    public string feature;
}

public enum EnemyType
{
    Goblin,
    Skeleton,
    Slime,
    Ghost,
    Golem,
    Dragon
}