using System.Collections;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private const float ATTACK_DURATION = 0.7f;
    private readonly float ATTACK_COOLDOWN = 1f;

    private Hero hero;
    private Enemy enemy;
    private bool _playerAttacks = false;
    private int _moveCount = 1;

    public void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    /// <summary>
    /// Describes a fight between characters from the very beginning till death
    /// </summary>
    public IEnumerator Fight()
    {
        _moveCount = 1;
        if (hero.Agility >= enemy.Agility)
            _playerAttacks = true;

        while (!hero.IsDead() && !enemy.IsDead())
        {
            yield return new WaitForSeconds(ATTACK_COOLDOWN);

            int damage = CalculateDamage();

            if (_playerAttacks)
                yield return StartCoroutine(Attack(hero, enemy, damage));
            else
                yield return StartCoroutine(Attack(enemy, hero, damage));

            _playerAttacks = !_playerAttacks;
            ++_moveCount;
        }
    }

    /// <summary>
    /// Calculates damage that will be dealt in this current attack
    /// </summary>
    /// <returns>damage amount</returns>
    private int CalculateDamage()
    {
        int damage;
        if (_playerAttacks) {
            damage = hero.Strength + hero.WeaponDamage;
            // Enemy defend abilities
            if (enemy.Race.type == EnemyType.Skeleton && hero.Weapon.damageType == DamageType.Crush)
                damage *= 2;
            else if (enemy.Race.type == EnemyType.Slime && hero.Weapon.damageType == DamageType.Chop)
                damage -= hero.WeaponDamage;
            else if (enemy.Race.type == EnemyType.Golem)
                damage -= enemy.Stamina;
            // Hero attack bonuses
            if (hero.Classes.Any(race => race.Key.type == HeroType.Bandit) && hero.Agility > enemy.Agility)
                ++damage;
            if (hero.Classes.Any(race => race.Key.type == HeroType.Bandit && race.Value == 3))
                damage += (_moveCount - 1) / 2;
            if (hero.Classes.Any(race => race.Key.type == HeroType.Warrior) && _moveCount <= 2)
                damage += hero.WeaponDamage;
            if (hero.Classes.Any(race => race.Key.type == HeroType.Barbarian)) {
                if (_moveCount <= 6)
                    damage += 2;
                else
                    --damage;
            }

        } else {
            damage = enemy.Strength + enemy.WeaponDamage;
            // Enemy attack abilities
            if (enemy.Race.type == EnemyType.Dragon && _moveCount % 3 == 0)
                damage += 3;
            else if (enemy.Race.type == EnemyType.Ghost && enemy.Agility > hero.Agility)
                damage += 1;
            // Hero defend bonuses
            if (hero.Classes.Any(race => race.Key.type == HeroType.Warrior && race.Value >= 2) && hero.Strength > enemy.Strength)
                damage -= 3;
            if (hero.Classes.Any(race => race.Key.type == HeroType.Barbarian && race.Value >= 2))
                damage -= hero.Stamina;
        }
        return Mathf.Max(damage, 0);
    }

    /// <summary>
    /// Controlling one current attack: animations and damage
    /// </summary>
    private IEnumerator Attack(Entity attacking, Entity defending, int damage)
    {
        int chance = Random.Range(1, attacking.Agility + defending.Agility + 1);
        if (chance <= defending.Agility)
        {
            damage = 0;
            defending.PlayAnimation(State.dodge);
        } else {
            defending.TakeDamage(damage);
            defending.PlayAnimation(State.damage);
        }

        defending.ShowHit(damage);

        attacking.PlayAnimation(State.attack);

        yield return new WaitForSeconds(ATTACK_DURATION);

        attacking.PlayAnimation(State.idle);
        defending.PlayAnimation(State.idle);

        if (defending.IsDead())
            defending.PlayAnimation(State.dead);

        defending.HideHit();
    }
}
