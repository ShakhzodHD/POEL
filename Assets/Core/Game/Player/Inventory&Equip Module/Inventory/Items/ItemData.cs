using UnityEngine;

public class ItemData : MonoBehaviour
{
    public ItemDefinition ItemDefinition
    {
        get
        {
            return currentItemDefinition;
        }
        set
        {
            if (currentItemDefinition != value)
            {
                currentItemDefinition = value;
            }
        }
    }
    private ItemDefinition currentItemDefinition;
}
