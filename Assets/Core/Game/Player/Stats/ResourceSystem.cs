public class ResourceSystem
{
    public float MaxResource { get; private set; }
    public float CurrentResource { get; private set; }

    public ResourceSystem(float maxResource)
    {
        MaxResource = maxResource;
        CurrentResource = maxResource;
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
