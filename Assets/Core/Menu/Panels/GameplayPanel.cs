using UnityEngine;
using UnityEngine.UI;

public class GameplayPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _pause;
    public void Init()
    {
        Debug.Log("Инициализирован геймплейная панель");
    }
    private void Start()
    {
        _pause.onClick.AddListener(OnButtonPauseClick);
    }
    private void OnButtonPauseClick()
    {
        Boostrap.Instance.TimeScaleController.PauseGame();
        Boostrap.Instance.UIManager.ChangeMenuState(MenuStates.Pause);
    }
}
