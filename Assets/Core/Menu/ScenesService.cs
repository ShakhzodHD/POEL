using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesService : MonoBehaviour
{
    [SerializeField] private string _nameSceneHideout;
    public event Action OnLevelLoaded;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 0) return;
        OnLevelLoaded?.Invoke();
    }
    public void InitialLoad()
    {
        
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(Constants.MENU_SCENE_NAME);
    }
}
