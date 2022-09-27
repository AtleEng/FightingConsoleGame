class GameManager
{
    public bool hasWon;
    float waitTime = 1;
    float _waitTime;
    bool isFighting;
    bool isShoping;
    bool isPlayerTurn;
    int roundCounter = 0;
    Random number = new Random();

    Unit player = null;
    Unit currentEnemy = null;

    Unit EnemyBandit = new Unit("Bandit", 100, 15, 15, 10, false, false, Unit.type.human, "Just a simple human trying to make a living");
    Unit EnemyWolf = new Unit("Wolf", 50, 5, 20, 30, false, false, Unit.type.monster, "A dreadful monster hound");
    Unit EnemyHeavy = new Unit("Big bandit", 200, 20, 15, 0, false, false, Unit.type.human, "Also trying to make a living but eats a bit more");

    Unit EnemyDemon = new Unit("Demon", 200, 25, 40, 25, false, true, Unit.type.monster, "From the depths of hell");
    Unit EnemyLivingTree = new Unit("Living Tree", 300, 0, 30, 0, false, false, Unit.type.livingPlant, "Made of wood, burn like wood");
    Unit EnemyVampire = new Unit("Vampire", 150, 0, 25, 30, true, false, Unit.type.monster, " This monster is half man, half bat and lives on blood");

    Item boots = new Item("Boots", 0, 10, 0, 30, false, false, "A pair of sturdy boots, makes you able to dodge an attack", 15);
    Item vampireRing = new Item("Vampire Ring", 30, -10, 0, 0, true, false, "It feels alive, heal 50% of your attacks damage but lowering your defence", 25);
    Item swordShaperner = new Item("Sharpening stone", 0, 0, 10, 0, false, false, "A flat stone used for sharpening your blade", 12);
    Item helmet = new Item("Helmet", 0, 20, 0, 0, false, false, "A hard helemt made of iron, increse your defense", 17);

    List<Item> shopInventory = new List<Item>();
    List<Item> playerInventory = new List<Item>();
    public void Start()
    {
        player = new Unit("", 1, 1, 1, 1, false, false, Unit.type.human, "");

        FillShop();
        playerInventory.Clear();
        playerInventory.Add(boots);
        playerInventory.Add(vampireRing);

        while (player.name == "")
        {
            Console.Clear();
            Console.WriteLine("Welcome player, please enter your name:");
            player.name = Console.ReadLine();
        }
        string s = RandomTitle();
        player.description = $"This is {player.name}, {s}";
        SetPlayerStats();
        GameSystem();
    }

    void GameSystem()
    {
        roundCounter++;
        if (roundCounter == 1)
        {
            StartTheBattle(EnemyBandit);
        }
        if (roundCounter == 2)
        {
            StartTheBattle(EnemyWolf);
        }
        if (roundCounter == 3)
        {
            StartTheBattle(EnemyHeavy);
        }
        if (roundCounter == 4)
        {
            hasWon = true;
            TextHandler("You killed everybody och stand victorius", true);
        }
    }

    public void Update()
    {
        if (isFighting)
        {
            if (isPlayerTurn)
            {
                PlayerTurn();
            }
            else
            {
                EnemyTurn();
            }

        }
        else if (isShoping)
        {
            ChoiceTextHandler($"Hello {player.name}, I have some things to sell to you", 4, true);
        }
    }

    void PlayerTurn()
    {
        int i = ChoiceTextHandler($"Its your turn to strike, what do you do?\n" +
        "Press 1 to slash with your sword \nPress 2 to throw a fireball \nPress 3 to throw Holywater" +
        "\nPress 4 to curse the enemy \nPress 5 to inspect enemy", 5, true);

        if (i == 1)
        {
            Attack(player, currentEnemy, 50, Unit.attackElement.physical);
        }
        else if (i == 2)
        {
            Attack(player, currentEnemy, 40, Unit.attackElement.fire);
        }
        else if (i == 3)
        {
            Attack(player, currentEnemy, 30, Unit.attackElement.holy);
        }
        else if (i == 4)
        {
            Attack(player, currentEnemy, 10, Unit.attackElement.curse);
        }
        else if (i == 5)
        {
            Inspect(currentEnemy);
        }

    }

    void EnemyTurn()
    {
        TextHandler($"The {currentEnemy.name} attacks", true);

        Attack(currentEnemy, player, currentEnemy.damage, Unit.attackElement.physical);
    }

    void ChangeTurn(bool pt)
    {
        Console.Beep(400, 100);
        if (pt == true)
        {
            isPlayerTurn = false;
        }
        else
        {
            isPlayerTurn = true;
        }
    }
    public void StartTheBattle(Unit enemy)
    {
        TextHandler($"A {enemy.name} attack", true);

        isFighting = true;
        isPlayerTurn = true;

        currentEnemy = enemy;
    }
    public void StartTheShop()
    {
        TextHandler("Someone is trying to sell you somthing", true);
        isShoping = true;
    }
    void Attack(Unit attacker, Unit victim, int dmg, Unit.attackElement element)
    {

        bool shouldCrit = false;
        bool ignoreEffect = false;

        int elementI = calcElement(attacker, victim, element);
        //crit calculation
        if (number.Next(0, 10) == 0 || elementI == 1)
        {
            shouldCrit = true;
        }
        else if (elementI == 2)
        {
            ignoreEffect = true;
        }
        string message = "";

        if (number.Next(0, 100) < victim.dodge)
        {
            ignoreEffect = true;
        }
        if (!ignoreEffect)
        {
            if (element == Unit.attackElement.curse)
            {
                if (shouldCrit)
                {
                    message += $"{attacker.name} deal a critical hit!!!";
                    message += $"\n{victim.name} was cursed -10 dmg";
                    if (victim.damage > 10)
                    {
                        victim.damage -= 10;
                    }
                    else
                    {
                        victim.damage = 5;
                    }
                }
                else
                {
                    message += $"\n{victim.name} was cursed -5 dmg";
                    if (victim.damage > 5)
                    {
                        victim.damage -= 5;
                    }
                    else
                    {
                        victim.damage = 5;
                    }
                }
            }
            else
            {
                if (shouldCrit)
                {
                    victim.health -= dmg;
                    message += $"{attacker.name} deal a critical hit!!!";
                    message += $"\n{victim.name} took {dmg} damage";
                }
                else
                {
                    //defence
                    int _dmg = dmg;
                    _dmg -= victim.defence;
                    if (_dmg >= 5)
                    {
                        victim.health -= _dmg;
                        message += $"{victim.name} took {_dmg} damage";
                    }
                    else
                    {
                        victim.health -= 5;
                        message += $"{victim.name} had too high defence, only takes 5 damage.";
                    }
                }
            }
        }
        else
        {
            message = $"{victim.name} dodge all dmg";
        }

        TextHandler(message, true);

        //if killed
        if (victim.health <= 0)
        {
            TextHandler($"{victim.name} died", true);
            if (victim == player)
            {
                LoseTheGame();
                return;
            }
            else
            {
                WinTheFight();
                return;
            }
        }
        else
        {
            TextHandler($"{victim.name} have {victim.health} hp left.", false);
            ChangeTurn(isPlayerTurn);
        }
    }

    void LoseTheGame()
    {
        TextHandler("You lost idiot", true);
        Start();
    }
    void WinTheFight()
    {
        isFighting = false;

        TextHandler("You killed your enemy", true);
        GameSystem();
    }

    int calcElement(Unit attacker, Unit defender, Unit.attackElement attackElement)
    {

        if (defender.whatType == Unit.type.monster && attackElement == Unit.attackElement.holy)
        {
            //crit
            return 1;
        }
        if (defender.whatType == Unit.type.livingPlant && attackElement == Unit.attackElement.fire)
        {
            //crit
            return 1;
        }
        if (defender.whatType == Unit.type.monster && attackElement == Unit.attackElement.curse)
        {
            //be imune
            return 2;
        }
        if (defender.whatType == Unit.type.livingPlant && attackElement == Unit.attackElement.holy)
        {
            //be imune
            return 2;
        }
        return 0;
    }

    void Inspect(Unit unit)
    {
        TextHandler($"{unit.name} HP: {unit.curHealth} DEF: {unit.defence}\n{unit.description}", true);
        ChangeTurn(isPlayerTurn);
    }

    public void TextHandler(string theText, bool shouldClear)
    {
        if (shouldClear)
        {
            Console.Clear();
            ShowStats();
        }

        Console.WriteLine(theText);
        Console.ReadLine();

    }
    public int ChoiceTextHandler(string theText, int numberOfChoices, bool shouldClear)
    {
        if (shouldClear) Console.Clear();
        ShowStats();

        int result = 0;

        Console.WriteLine(theText);
        string s = Console.ReadLine();

        for (int i = 0; i < numberOfChoices + 1; i++)
        {
            if (s == i.ToString())
            {
                result = i;
                return (result);
            }
        }
        return (0);
    }
    string RandomTitle()
    {
        int r = number.Next(0, 10);
        string title = "";

        List<string> s = new List<string>();
        s.Add("the brave");
        s.Add("the fat");
        s.Add("the fearless");
        s.Add("the warrior");
        s.Add("the handsome");
        s.Add("the ugly");
        s.Add("the swift");
        s.Add("the lonely");
        s.Add("the second (the first died)");
        s.Add("the crazy");

        title = s[r];

        return title;
    }
    void ShowStats()
    {
        Console.WriteLine($"{player.name} HP:{player.health} DEF:{player.defence}");
        string s = desplayPlayerInventory();
        Console.WriteLine(s + "\n");
    }
    string desplayPlayerInventory()
    {
        string allItems = "";
        for (int i = 0; i < playerInventory.Count; i++)
        {
            allItems += $" [{playerInventory[i].name}]";
        }

        return allItems;
    }
    void FillShop()
    {
        shopInventory.Add(boots);
        shopInventory.Add(vampireRing);
        shopInventory.Add(swordShaperner);
        shopInventory.Add(helmet);
    }
    void SetPlayerStats()
    {
        int totalHP = 100;
        int totalDEF = 20;
        int totalDMG = 0;
        int totalDodge = 10;
        bool isVampire = false;
        bool isFireRes = false;

        for (int i = 0; i < playerInventory.Count; i++)
        {
            totalHP += playerInventory[i].healthModifier;
            totalDEF += playerInventory[i].defenceModifier;
            totalDMG += playerInventory[i].attackModifier;
            totalDodge += playerInventory[i].dodgeModifier;
            if (playerInventory[i].isVampire)
            {
                isVampire = true;
            }
            if (playerInventory[i].isFireRes)
            {
                isFireRes = true;
            }
        }
        player.health = totalHP;
        player.defence = totalDEF;
        player.damage = totalDMG;
        player.dodge = totalDodge;

        player.isVampire = isVampire;
        player.isFireRes = isFireRes;
    }

}