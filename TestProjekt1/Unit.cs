class Unit
{
    public string name;
    public int curHealth;
    public int health = 10;
    public int defence = 2;
    public int damage = 1;
    public int dodge = 10;
    public bool isVampire;
    public bool isFireRes;
    public string description;

    public type whatType;

    public Unit(string name, int hp, int def, int atk, int dodge, bool isVampire, bool isFireRes, type whatType, string description)
    {
        this.name = name;
        this.health = hp;
        this.defence = def;
        this.damage = atk;
        this.dodge = dodge;
        this.whatType = whatType;
        this.description = description;

        curHealth = hp;
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
