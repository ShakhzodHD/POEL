using System;
public class HealthSystem
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public HealthSystem(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Console.WriteLine("Смерть");
        }
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
    }
}
