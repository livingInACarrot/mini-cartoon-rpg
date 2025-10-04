using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour
{
    private const int MAX_LEVEL = 3;
    private const string LEVEL_SUFFIX = " ур.";
    private const string MAX_LEVEL_SUFFIX = "max";
    private const string HEALTH_BONUS_FORMAT = "\n<b>Здоровье: +{0}</b>";

    [SerializeField] private HeroClass heroClass;
    private Hero hero;
    private GameManager gameManager;
    private TMP_Text levelText;
    private TMP_Text featureText;
    private Button button;

    public void Start()
    {
        InitializeComponents();
        UpdateButtonState();
    }

    private void InitializeComponents()
    {
        hero = GameObject.FindWithTag("Player").GetComponent<Hero>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        button = GetComponent<Button>();

        var texts = GetComponentsInChildren<TMP_Text>();
        if (texts.Length >= 2)
        {
            levelText = texts[0];
            featureText = texts[1];
        }

        UpdateButtonText();
    }

    public void OnClick()
    {
        if (gameManager == null) return;

        if (hero.Classes.Count == 0)
            gameManager.ChooseStartingHero(heroClass);
        else
            gameManager.ChooseHero(heroClass);

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (hero.Classes.TryGetValue(heroClass, out int currentLevel))
        {
            if (currentLevel >= MAX_LEVEL)
                SetMaxLevelState();
            else
                UpdateButtonText(currentLevel);
        }
        else
        {
            UpdateButtonText();
        }
    }

    private void UpdateButtonText(int currentLevel = 0)
    {
        if (levelText != null)
            levelText.text = $"{currentLevel + 1}{LEVEL_SUFFIX}";

        if (featureText != null)
        {
            string feature = currentLevel < heroClass.features.Count
                ? heroClass.features[currentLevel]
                : string.Empty;

            featureText.text = feature + string.Format(HEALTH_BONUS_FORMAT, heroClass.healthPerLvl);
        }
    }

    private void SetMaxLevelState()
    {
        if (levelText != null) levelText.text = MAX_LEVEL_SUFFIX + LEVEL_SUFFIX;
        if (featureText != null) featureText.text = string.Empty;
        button.interactable = false;
    }

    public void Reset()
    {
        button.interactable = true;
        UpdateButtonText();
    }
}