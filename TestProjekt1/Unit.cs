class Unit
{
    public string name;
    public int HP = 10;
    public int DEF = 2;

    public string description;

    public type whatType;

    public Unit(string name, int hp, int def, type whatType, string description)
    {
        this.name = name;
        this.HP = hp;
        this.DEF = def;
        this.whatType = whatType;
        this.description = description;
    }

    public enum attackElement
    {
        physical,
        fire,
        holy,
        curse
    }
    public enum type
    {
        human,
        monster,
        livingPlant
    }
}
