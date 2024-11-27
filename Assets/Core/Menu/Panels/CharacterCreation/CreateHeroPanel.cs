using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateHeroPanel : MonoBehaviour
{
    [SerializeField] private CharacterClass[] classes;

    [Header("Buttons")]
    [SerializeField] private RectTransform classPanel;
    [SerializeField] private RectTransform abilityPanel;

    [SerializeField] private Button createWarrior;
    [SerializeField] private Button createRanger;
    [SerializeField] private Button createMage;
    [SerializeField] private Button createUniversal;

    [SerializeField] private GameObject abilityPrefab;

    [SerializeField, Space] private Button back;
    [SerializeField, Space] private Text abilityTypeText;

    private CharacterClass selectedClass;
    private SelectionStage currentStage;
    private int idCharacters = 0;

    private void Start()
    {
        abilityTypeText.text = "Select Class";

        back.onClick.AddListener(OnButtonBackClick);

        createWarrior.onClick.AddListener(delegate { SelectClass(0); });
        createRanger.onClick.AddListener(delegate { SelectClass(1); });
        createMage.onClick.AddListener(delegate { SelectClass(2); });
        createUniversal.onClick.AddListener(delegate { SelectClass(3); });
    }

    private void OnButtonBackClick()
    {
        abilityTypeText.text = "Select Class";

        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.HeroPick);

        classPanel.gameObject.SetActive(true);
        abilityPanel.gameObject.SetActive(false);

        selectedClass = null;
        currentStage = SelectionStage.Major;
    }

    private void SelectClass(int classIndex)
    {
        ClearPanel();

        selectedClass = classes[classIndex];
        currentStage = SelectionStage.Major;

        classPanel.gameObject.SetActive(false);
        abilityPanel.gameObject.SetActive(true);

        ShowAbilityOptions();
    }

    private void ShowAbilityOptions()
    {
        ClearPanel();

        if (currentStage == SelectionStage.Passive)
        {
            abilityTypeText.text = "Select Passive Ability";
            foreach (PassiveAbility ability in selectedClass.startingPassiveAbilities)
            {
                GameObject abilityButton = Instantiate(abilityPrefab, abilityPanel);
                abilityButton.GetComponentInChildren<Text>().text = ability.abilityName;
                abilityButton.GetComponent<Button>().onClick.AddListener(() => SelectPassiveAbility(ability));
            }
        }
        else
        {
            List<ActiveAbility> abilitiesToShow = null;
            switch (currentStage)
            {
                case SelectionStage.Major:
                    abilitiesToShow = selectedClass.startingMajorAbilities.ToList();
                    abilityTypeText.text = "Select Major Ability";
                    break;
                case SelectionStage.Minor:
                    abilitiesToShow = selectedClass.startingMinorAbilities.ToList();
                    abilityTypeText.text = "Select Minor Ability";
                    break;
                case SelectionStage.Escape:
                    abilitiesToShow = selectedClass.startingEscapeAbilities.ToList();
                    abilityTypeText.text = "Select Escape Ability";
                    break;
            }

            foreach (ActiveAbility ability in abilitiesToShow)
            {
                GameObject abilityButton = Instantiate(abilityPrefab, abilityPanel);
                abilityButton.GetComponentInChildren<Text>().text = ability.abilityName;
                abilityButton.GetComponent<Button>().onClick.AddListener(() => SelectAbility(ability));
            }
        }
    }

    private void SelectAbility(ActiveAbility ability)
    {
        switch (currentStage)
        {
            case SelectionStage.Major:
                Boostrap.Instance.PlayerData.abilityMajor = ability;
                currentStage = SelectionStage.Minor;
                break;
            case SelectionStage.Minor:
                Boostrap.Instance.PlayerData.abilityMinor = ability;
                currentStage = SelectionStage.Escape;
                break;
            case SelectionStage.Escape:
                Boostrap.Instance.PlayerData.abilityEscape = ability;
                currentStage = SelectionStage.Passive;
                break;
        }

        ShowAbilityOptions();
    }

    private void SelectPassiveAbility(PassiveAbility ability)
    {
        Boostrap.Instance.PlayerData.abilityPassive = ability;
        CompleteCharacterCreation();
    }

    private void CompleteCharacterCreation()
    {
        var newCharacter = new Character(
            id: idCharacters,
            selectedClass.name,
            selectedClass,
            Boostrap.Instance.PlayerData.abilityMajor,
            Boostrap.Instance.PlayerData.abilityMinor,
            Boostrap.Instance.PlayerData.abilityEscape,
            Boostrap.Instance.PlayerData.abilityPassive,
            selectedClass.baseHealth,
            selectedClass.baseResource,
            selectedClass.baseSpeed,
            selectedClass.BaseStats,
            Boostrap.Instance.GameSettings.inventoryColumn,
            Boostrap.Instance.GameSettings.inventoryRow
        );

        idCharacters++;

        Boostrap.Instance.PlayerData.AddCharacter(newCharacter);
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.HeroPick);

        Boostrap.Instance.PlayerData.abilityMajor = null;
        Boostrap.Instance.PlayerData.abilityMinor = null;
        Boostrap.Instance.PlayerData.abilityEscape = null;
        Boostrap.Instance.PlayerData.abilityPassive = null;

        selectedClass = null;
        currentStage = SelectionStage.Major;
        abilityTypeText.text = "Select Class";

        ClearPanel();

        classPanel.gameObject.SetActive(true);
        abilityPanel.gameObject.SetActive(false);
    }
    private void ClearPanel()
    {
        foreach (Transform child in abilityPanel)
        {
            Destroy(child.gameObject);
        }
    }
    private enum SelectionStage
    {
        Major,
        Minor,
        Escape,
        Passive
    }
}
