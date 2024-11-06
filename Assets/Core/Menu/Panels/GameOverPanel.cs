using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _restart;
    private void Start()
    {
        _restart.onClick.AddListener(OnButtonRestartClick);
    }

    private void OnEnable()
    {
        //Boostrap.Instance.GameEvents.OnLevelLose?.Invoke();
    }

    private void OnButtonRestartClick()
    {
        //Boostrap.Instance.ScenesService.RestartLevel();
    }
}
