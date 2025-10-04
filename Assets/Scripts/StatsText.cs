using TMPro;
using UnityEngine;

public class StatsText : MonoBehaviour
{
    [SerializeField] private Entity entity;
    private TMP_Text bodyText;
    private TMP_Text hpText;

    public void Start()
    {
        TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
        bodyText = texts[0];
        hpText = texts[1];
    }

    public void Update()
    {
        hpText.text = entity.GetCurrentHealth().ToString();
        bodyText.text = $"����..................................{entity.Strength} (+{entity.WeaponDamage})\r\n" +
            $"��������...........................{entity.Agility}\r\n" +
            $"��������...........................{entity.MaxHealth - entity.Stamina} (+{entity.Stamina})\r\n";
    }
}
