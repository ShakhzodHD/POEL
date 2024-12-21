using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;
    private HealthSystem healthSystem;
    public void Initialize(HealthSystem health)
    {
        healthSystem = health;
        healthSystem.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI(healthSystem.CurrentHealth / healthSystem.MaxHealth);
    }
    private void UpdateHealthUI(float healthPercentage)
    {
        Debug.Log($"Обновление UI: {healthPercentage}");
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = healthPercentage;
        }
    }
    public void Unsubscribe()
    {
        healthSystem.OnHealthChanged -= UpdateHealthUI;
    }
}
