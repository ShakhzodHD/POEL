using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _settings;
    [SerializeField] private Button _play;
    private void Start()
    {
        _settings.onClick.AddListener(OnButtonSettingsClick);
        _play.onClick.AddListener(OnButtonPlayClick);
    }
    private void OnButtonSettingsClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Settings);
    }

    private void OnButtonPlayClick()
    {
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.HeroPick);
    }
}
