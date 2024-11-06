using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("PREFABS")]
    public GameObject PlayerPrefab;
    public GameObject[] WeaponsPrefabs;

    [Header("SOUNDS")]
    public AudioClip PistolShootSound;
}
