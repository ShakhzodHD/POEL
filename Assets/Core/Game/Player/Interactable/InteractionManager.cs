using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private Transform currentPlayerTransform;
    public void Initialize(Transform playerTransform)
    {
        currentPlayerTransform = playerTransform;
    }
    public void DropItem(ItemDefinition item)
    {
        if (item.physicalObj != null)
        {
            var itemObj = Instantiate(item.physicalObj);
            itemObj.transform.position = currentPlayerTransform.position;
            itemObj.GetComponent<ItemData>().ItemDefinition = item;
        }
    }
}
