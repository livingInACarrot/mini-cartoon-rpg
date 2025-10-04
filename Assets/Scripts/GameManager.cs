using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float PRE_FIGHT_DELAY = 2f;
    private const float POST_FIGHT_DELAY = 3f;
    private const int VICTORY_STREAK = 5;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject heroChoicePanel;
    [SerializeField] private GameObject heroButtonsPanel;
    [SerializeField] private GameObject weaponChoicePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private EnemyClass[] enemyPrefabs;

    private Hero hero;
    private Enemy enemy;
    private CombatManager combatManager;
    private int winStreak = 0;
    private int enemyIndex = 0;

    public void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        combatManager = GetComponent<CombatManager>();

        StartGame();
    }

    /// <summary>
    /// Hides all panels
    /// </summary>
    private void HideAllPanels()
    {
        startPanel.SetActive(false);
        heroChoicePanel.SetActive(false);
        heroButtonsPanel.SetActive(false);
        weaponChoicePanel.SetActive(false);
        deathPanel.SetActive(false);
        victoryPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Starts a new game
    /// </summary>
    public void StartGame()
    {
        HideAllPanels();
        enemyIndex = 0;
        Shuffle(enemyPrefabs);
        startPanel.SetActive(true);
        heroButtonsPanel.SetActive(true);
    }

    /// <summary>
    /// Resets everything and starts a new game
    /// </summary>
    public void RestartGame()
    {
        winStreak = 0;
        ResetHeroButtons(heroButtonsPanel);
        hero.Reset();
        StartGame();
    }

    /// <summary>
    /// Restarting the game with all coroutines stopped
    /// </summary>
    private IEnumerator SafeRestartCoroutine()
    {
        yield return null;
        RestartGame();
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes the game after pause
    /// </summary>
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Restarts the game after pause
    /// </summary>
    public void RestartFromPauseGame()
    {
        Time.timeScale = 1;
        hero.HideHit();
        enemy.HideHit();
        StopAllCoroutines();
        combatManager.StopAllCoroutines();
        StartCoroutine(SafeRestartCoroutine());
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    /// <summary>
    /// Executes after a hero button first click
    /// </summary>
    public void ChooseStartingHero(HeroClass heroClass)
    {
        hero.InitializeClass(heroClass);
        hero.SetWeapon(heroClass.startingWeapon);
        ChangeWeaponsSprites(heroClass.startingWeapon, heroClass.startingWeapon);
        HideAllPanels();
        StartCoroutine(StartRound());
    }

    /// <summary>
    /// Executes after a hero button click
    /// </summary>
    public void ChooseHero(HeroClass heroClass)
    {
        hero.InitializeClass(heroClass);
        HideAllPanels();
        StartCoroutine(StartRound());
    }

    /// <summary>
    /// Sets all hero buttons and hero texts to default in hero choice panel
    /// </summary>
    public void ResetHeroButtons(GameObject panel)
    {
        HeroButton[] buttons = panel.GetComponentsInChildren<HeroButton>();
        foreach (var button in buttons)
        {
            button.Reset();
        }
    }

    /// <summary>
    /// Chooses an enemy for current round
    /// </summary>
    private void ChooseEnemy()
    {
        enemy.InitializeEnemy(enemyPrefabs[enemyIndex]);
        ++enemyIndex;
    }

    /// <summary>
    /// Executes on a weapon button click
    /// </summary>
    public void ChooseWeapon(WeaponButton weaponButton)
    {
        hero.SetWeapon(weaponButton.GetWeapon());
        weaponChoicePanel.SetActive(false);
        heroChoicePanel.SetActive(true);
        heroButtonsPanel.SetActive(true);
    }

    /// <summary>
    /// Represends a full game round from choosing an enemy till finish
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRound()
    {
        // Preparation phase
        UpdateRound();
        ChooseEnemy();

        hero.PlayAnimation(State.idle);
        enemy.PlayAnimation(State.idle);

        yield return new WaitForSeconds(PRE_FIGHT_DELAY);

        //Combat phase
        yield return StartCoroutine(combatManager.Fight());

        yield return new WaitForSeconds(POST_FIGHT_DELAY);

        // After fight phase
        if (hero.IsDead())
        {
            deathPanel.SetActive(true);
            yield break;
        }

        ++winStreak;
        if (winStreak >= VICTORY_STREAK)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            ChangeWeaponsSprites(hero.Weapon, enemy.Race.drop);
            weaponChoicePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Replaces weapon sprites on a weapon choice panel to our old weapon and a weapon dropped from defended enemy
    /// </summary>
    private void ChangeWeaponsSprites(Weapon oldWeapon, Weapon dropWeapon)
    {
        WeaponButton[] weaponButtons = weaponChoicePanel.GetComponentsInChildren<WeaponButton>();
        weaponButtons[0].SetWeapon(oldWeapon);
        weaponButtons[1].SetWeapon(dropWeapon);
    }

    /// <summary>
    /// Randomly shuffles an array of elements
    /// </summary>
    public static void Shuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            (array[randomIndex], array[i]) = (array[i], array[randomIndex]);
        }
    }

    /// <summary>
    /// Updates round number on the screen
    /// </summary>
    private void UpdateRound()
    {
        var roundText = GameObject.FindGameObjectWithTag("Round").GetComponentInChildren<TMP_Text>();
        roundText.text = $"Раунд {winStreak + 1}";
    }
}