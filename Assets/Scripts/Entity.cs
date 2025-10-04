using UnityEngine;
using System;
using TMPro;

public class Entity : MonoBehaviour
{
    [HideInInspector] public int MaxHealth { get; protected set; }
    [HideInInspector] public int Strength { get; protected set; }
    [HideInInspector] public int Agility { get; protected set; }
    [HideInInspector] public int Stamina { get; protected set; }
    [HideInInspector] public int WeaponDamage { get; protected set; }

    [SerializeField] protected UnityEngine.UI.Slider hpSlider;
    [SerializeField] protected TMP_Text nameText;
    [SerializeField] protected GameObject hitSprite;
    protected Animator animator;
    protected int currentHealth;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int amount)
    {
        hpSlider.value -= amount;
        if (amount >= currentHealth)
        {
            Die();
            return;
        }
        currentHealth -= amount;
    }

    protected virtual void Die()
    {
        currentHealth = 0;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void PlayAnimation(State name)
    {
        animator.Play(Convert.ToString(name));
    }

    public void ShowHit(int damage)
    {
        TMP_Text text = hitSprite.GetComponentInChildren<TMP_Text>();
        if (text == null)
            return;
        text.text = "-" + damage.ToString();
        hitSprite.SetActive(true);
    }

    public void HideHit()
    {
        hitSprite.SetActive(false);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

public enum State
{
    idle,
    attack,
    damage,
    dodge,
    dead
}