using System;
using UnityEngine;

public class Boostrap : MonoBehaviour
{
    public static Boostrap Instance { get; private set; }
    public readonly GameEvents GameEvents = new GameEvents();
    public UIManager UIManager { get; private set; }
    public ScenesService ScenesService { get; private set; }
    public GameStates GameState { get; private set; } = GameStates.InMenu;
    public event Action<GameStates> OnGameStateChanged;
    public GameSettings GameSettings { get; private set; }
    public TopDownCamera TopDownCamera { get; private set; }
    public PlayerData PlayerData { get; private set; }
    public TimeScaleController TimeScaleController { get; private set; }
    public ExperienceSystem ExperienceSystem { get; set; }
    public InventoryManager InventoryManager { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Init();
        UIManager.Init();

        GameSettings = Resources.Load<GameSettings>(Constants.GAME_SETTINGS_RESOURCES_PATH);

        DontDestroyOnLoad(gameObject);
        ScenesService.OnLevelLoaded += OnLevelLoaded;
    }
    private void Init()
    {
        ScenesService = FindObjectOfType<ScenesService>();
        UIManager = FindObjectOfType<UIManager>();
        TopDownCamera = FindObjectOfType<TopDownCamera>();
        PlayerData = FindObjectOfType<PlayerData>();
        TimeScaleController = FindObjectOfType<TimeScaleController>();
        InventoryManager = FindObjectOfType<InventoryManager>();
    }
    private void OnLevelLoaded()
    {
        //GameManager = FindObjectOfType<GameManager>();
        ChangeGameState(GameState == GameStates.InProgress ? GameStates.InProgress : GameStates.InMenu);
    }
    public void ChangeGameState(GameStates gameState)
    {
        if (GameState == gameState && GameState != GameStates.InProgress) return;
        GameState = gameState;
        OnGameStateChanged?.Invoke(gameState);
        //if (GameState == GameStates.LevelComplete)
    }
}
