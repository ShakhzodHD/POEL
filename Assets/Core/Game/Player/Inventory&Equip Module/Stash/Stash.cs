public class Stash
{
    public InventoryManager stashManager;
    public InventoryProvider stashProvider;
    public Stash(InventoryManager manager, InventoryProvider provider)
    {
        stashManager = manager;
        stashProvider = provider;
    }
}
