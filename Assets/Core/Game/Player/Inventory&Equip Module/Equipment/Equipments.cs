public class Equipments
{
    public InventoryManager[] equipmentManages;
    public InventoryProvider[] equipmentProviders;
    public Equipments(InventoryManager[] managers, InventoryProvider[] providers)
    {
        equipmentManages = managers;
        equipmentProviders = providers;
    }
}
