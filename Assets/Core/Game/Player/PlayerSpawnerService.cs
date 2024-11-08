using UnityEngine;

public class PlayerSpawnerService : MonoBehaviour
{
    public Player Player { get; private set; }

    private void Awake()
    {
        Player = Instantiate(Boostrap.Instance.GameSettings.PlayerPrefab, transform.position, transform.rotation, transform).GetComponent<Player>();
    }
}
