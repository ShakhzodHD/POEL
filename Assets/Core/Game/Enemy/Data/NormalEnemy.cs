public class NormalEnemy : BaseEnemy
{
    public NormalEnemy(string name, float movementSpeed, HealthSystem health)
    {
        Name = name;
        Speed = movementSpeed;
        Health = health;
    }
}
