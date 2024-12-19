using System;

public class HealthSystem
{
    public float MaxHealth { get; private set; }

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Math.Clamp(value, 0, MaxHealth);
            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }
        }
    }

    private float currentHealth;
    private bool isDead;

    public event Action OnDeath;

    public HealthSystem(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        CurrentHealth -= damage;
    }

    public void Heal(float healAmount)
    {
        if (isDead) return;
        CurrentHealth += healAmount;
    }

    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        Console.WriteLine("Character died");
    }
}
