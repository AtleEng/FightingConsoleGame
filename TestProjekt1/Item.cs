class Item
{
    public string name;
    public int healthModifier;
    public int defenceModifier;
    public int attackModifier;
    public int dodgeModifier;
    public bool isVampire;
    public bool isFireRes;

    public string description;
    public int price;

    public Item(string name, int healthModifier, int defenceModifier, int attackModifier, int dodgeModifier, bool isVampire, bool isFireRes, string description, int price)
    {
        this.name = name;
        this.healthModifier = healthModifier;
        this.defenceModifier = defenceModifier;
        this.attackModifier = attackModifier;
        this.dodgeModifier = dodgeModifier;
        this.isVampire = isVampire;
        this.isFireRes = isFireRes;

        this.description = description;
        this.price = price;
    }
}
