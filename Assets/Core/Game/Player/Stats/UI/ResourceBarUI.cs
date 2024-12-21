using UnityEngine;
using UnityEngine.UI;

public class ResourceBarUI : MonoBehaviour
{
    [SerializeField] private Image resourceFillImage;
    private ResourceSystem resourceSystem;
    public void Initialize(ResourceSystem resource)
    {
        resourceSystem = resource;
        resourceSystem.OnResourceChanged += UpdateResourceUI;
        UpdateResourceUI (resourceSystem.CurrentResource / resourceSystem.MaxResource);
    }
    private void UpdateResourceUI(float resourcePercentage)
    {
        Debug.Log($"Resource bar UI: {resourcePercentage}");
        if (resourceFillImage != null)
        {
            resourceFillImage.fillAmount = resourcePercentage;
        }
    }
    public void Unsubscribe()
    {
        resourceSystem.OnResourceChanged -= UpdateResourceUI;
    }
}
