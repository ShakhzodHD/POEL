using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject PlayerPrefab;
    public GameObject[] WeaponsPrefabs;

    [Header("Sounds")]
    public AudioClip PistolShootSound;

    [Header("Inventory")]
    public int inventoryColumn;
    public int inventoryRow;
}
