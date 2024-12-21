using System;

public class ResourceSystem
{
    public float MaxResource { get; private set; }
    public float CurrentResource
    {
        get => currentResource;
        set
        {
            float oldHealth = currentResource;

            currentResource = Math.Clamp(value, 0, MaxResource);

            if (oldHealth != currentResource)
            {
                OnResourceChanged?.Invoke(currentResource / MaxResource);
            }
        }
    }
    private float currentResource;

    public event Action<float> OnResourceChanged;
    public ResourceSystem(float maxResource)
    {
        MaxResource = maxResource;
        currentResource = maxResource;
    }

    public bool Consume(float amount)
    {
        if (CurrentResource >= amount)
        {
            CurrentResource -= amount;
            return true;
        }
        return false;
    }

    public void Restore(float amount)
    {
        CurrentResource += amount;
        if (CurrentResource > MaxResource) CurrentResource = MaxResource;
    }
}
