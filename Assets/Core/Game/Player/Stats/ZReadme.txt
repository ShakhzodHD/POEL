To add a new attribute to the stats, you need to:

Go to the enum Stat.
Add the new stat attribute.
In the scriptable objects (CharacterClass), add this attribute and assign values to it.


How to increase or decrease stat
In Player.cs
character.Stats.IncreaseStat(Stat.test, 1);
character.Stats.DecreaseStat(Stat.test, 9);