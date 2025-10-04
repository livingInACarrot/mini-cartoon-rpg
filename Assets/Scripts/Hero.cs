using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Hero : Entity
{
    public Weapon Weapon { get; protected set; }
    public Dictionary<HeroClass, int> Classes = new();

    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private GameObject weaponSprite;
    private HeroMultiClassAnimator multiAnimator;

    protected override void Awake()
    {
        base.Awake();
        multiAnimator = GetComponent<HeroMultiClassAnimator>();
    }

    public void InitializeClass(HeroClass heroClass)
    {
        // Initializing hero class
        if (Classes.ContainsKey(heroClass)) {
            Classes[heroClass] += 1;
        } else {
            Classes.Add(heroClass, 1);
            if (Classes.Count == 1)
            {
                WeaponDamage = heroClass.startingWeapon.damage;
                Strength = Random.Range(1, 4);
                Agility = Random.Range(1, 4);
                Stamina = Random.Range(1, 4);
                MaxHealth = Stamina;
            }
        }

        // Level bonus
        if (heroClass.type == HeroType.Bandit && Classes[heroClass] == 2)
            ++Agility;
        else if (heroClass.type == HeroType.Warrior && Classes[heroClass] == 3)
            ++Strength;
        else if (heroClass.type == HeroType.Barbarian && Classes[heroClass] == 3)
            ++Stamina;

        // Animation
        animator.runtimeAnimatorController = multiAnimator.UpdateMultiClassAnimator(heroClass.animatorController,
            Classes.Keys.Select(heroClass => heroClass.type).ToList());

        // Health
        MaxHealth += heroClass.healthPerLvl;
        currentHealth = MaxHealth;
        hpSlider.maxValue = MaxHealth;
        hpSlider.value = currentHealth;

        // Name
        nameText.text = "";
        foreach (var c in Classes)
        {
            nameText.text += $"{c.Key.className} {c.Value}ур.\n";
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this.Weapon = weapon;
        weaponSprite.GetComponent<SpriteRenderer>().sprite = weapon.icon;
        weaponNameText.text = weapon.weaponName;
        WeaponDamage = weapon.damage;
    }

    public void Reset()
    {
        Classes.Clear();
    }

    protected override void Die()
    {
        base.Die();
    }

}
