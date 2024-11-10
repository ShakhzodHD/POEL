[System.Serializable]
public class Stats
{
    public int Strength;
    public int Agility;
    public int Intelligence;
    public Stats(int strength, int agility, int inteleng)
    {
        Strength = strength;
        Agility = agility;
        Intelligence = inteleng;    
    }
    public Stats Clone()
    {
        return new Stats(Strength, Agility, Intelligence);
    }
}
