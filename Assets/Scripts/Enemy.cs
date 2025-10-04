using TMPro;
using UnityEngine;

public class Enemy : Entity
{
    private const string FEATURE_SUFFIX = "<b>Особенность персонажа:</b>\n";

    [HideInInspector] public EnemyClass Race;

    [SerializeField] private TMP_Text featureText;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeEnemy(EnemyClass enemyClass)
    {
        Race = enemyClass;
        Strength = enemyClass.strength;
        Agility = enemyClass.agility;
        Stamina = enemyClass.stamina;
        WeaponDamage = enemyClass.damage;
        animator.runtimeAnimatorController = enemyClass.animatorController;
        MaxHealth = enemyClass.stamina + enemyClass.health;
        currentHealth = MaxHealth;
        hpSlider.maxValue = MaxHealth;
        hpSlider.value = currentHealth;
        nameText.text = enemyClass.className;
        featureText.text = FEATURE_SUFFIX + enemyClass.feature;
    }
}
