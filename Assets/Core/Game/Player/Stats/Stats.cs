using System.Collections.Generic;

public class Stats
{
    public Dictionary<Stat, float> stats;
    public Stats(Dictionary<Stat, float> initialStats)
    {
        stats = new Dictionary<Stat, float>(initialStats);
    }
    public Stats Clone()
    {
        return new Stats(new Dictionary<Stat, float>(stats));
    }
    public void IncreaseStat(Stat stat, float value)
    {
        if (stats.ContainsKey(stat))
        {
            stats[stat] += value;
        }
    }
    public void DecreaseStat(Stat stat, float value)
    {
        if (stats.ContainsKey(stat))
        { 
            stats[stat] -= value;
            if (stats[stat] < 0) stats[stat] = 0;
        }
    }
}