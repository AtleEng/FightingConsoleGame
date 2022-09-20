class GameManager
{
    public bool hasWon;
    float waitTime = 1;
    float _waitTime;
    bool isFighting;
    bool isShoping;
    bool isPlayerTurn;
    int roundCounter = 1;
    Random number = new Random();

    Unit player = new Unit("", 10, 2, Unit.type.human, "");
    Unit currentEnemy;

    Unit EnemyBandit = new Unit("Bandit", 10, 1, Unit.type.human, "Just a simple human trying to make a living");
    Unit EnemyWolf = new Unit("Wolf", 5, 1, Unit.type.monster, "A dreadful monster hound");
    Unit EnemyHeavy = new Unit("Big bandit", 15, 2, Unit.type.human, "Also trying to make a living but eats a bit more");

    Unit EnemyDemon = new Unit("Demon", 20, 2, Unit.type.monster, "From the depths of hell");
    Unit EnemyLivingTree = new Unit("Living Tree", 20, 2, Unit.type.livingPlant, "Made of wood, burn like wood");
    Unit EnemyVampire = new Unit("Vampire", 15, 1, Unit.type.monster, " This monster is half man, half bat and lives on blood");
    public void Start()
    {
        while (player.name == "")
        {
            Console.Clear();
            Console.WriteLine("Welcome player, please enter your name:");
            player.name = Console.ReadLine();
        }
        string s = RandomTitle();
        player.description = $"This is {player.name}, {s}";
    }

    void GameSystem()
    {

        if (roundCounter == 1)
        {
            StartTheBattle(EnemyVampire);
        }
        if (roundCounter == 2)
        {
            StartTheBattle(EnemyLivingTree);
        }
        if (roundCounter == 3)
        {
            StartTheBattle(EnemyVampire);
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
            TextHandler($"Hello {player.name}, I have some things to sell to you", true);
        }
    }

    void PlayerTurn()
    {
        int i = ChoiceTextHandler($"Its your turn to strike, what do you do?\n" +
        "Press 1 to slash with your sword \nPress 2 to throw a fireball \nPress 3 to throw Holywater" +
        "\nPress 4 to curse the enemy \nPress 5 to inspect enemy", 5, true);

        if (i == 1)
        {
            Attack(player, currentEnemy, 7, Unit.attackElement.physical);
        }
        else if (i == 2)
        {
            Attack(player, currentEnemy, 4, Unit.attackElement.fire);
        }
        else if (i == 3)
        {
            Attack(player, currentEnemy, 5, Unit.attackElement.holy);
        }
        else if (i == 4)
        {
            Attack(player, currentEnemy, 0, Unit.attackElement.curse);
        }
        else if (i == 5)
        {
            Inspect(currentEnemy);
        }

    }

    void EnemyTurn()
    {
        TextHandler($"The {currentEnemy.name} attacks", true);

        int i = number.Next(1, 4);
        if (i == 1)
        {
            Attack(currentEnemy, player, 3, Unit.attackElement.physical);
        }
        else if (i == 2)
        {
            Attack(currentEnemy, player, 2, Unit.attackElement.physical);
        }
        else if (i == 3)
        {
            Attack(currentEnemy, player, 3, Unit.attackElement.physical);
        }

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
        TextHandler($"A mighty {enemy.name} stands before you, it seems to be aggresive and suddently it leaps towards you", true);

        isFighting = true;
        isPlayerTurn = true;

        currentEnemy = enemy;
    }

    void Attack(Unit attacker, Unit victim, int dmg, Unit.attackElement element)
    {

        bool shouldCrit = false;
        bool ignoreEffect = false;

        int elementI = calcElement(attacker, victim, element);
        if (elementI == 1)
        {
            shouldCrit = true;
        }
        else if (elementI == 2)
        {
            ignoreEffect = true;
        }

        string message = "";
        if (!ignoreEffect)
        {

            //crit calculation
            if (number.Next(0, 10) == 0 || shouldCrit)
            {
                victim.HP -= dmg;
                message += $"{attacker.name} deal a critical hit!!!";
                message += $"\n{victim.name} took {dmg} damage";
            }
            else
            {
                //defence
                int _dmg = dmg;
                _dmg -= victim.DEF;
                if (dmg >= 1)
                {
                    victim.HP -= _dmg;
                    message += $"{victim.name} took {_dmg} damage";
                }
                else
                {
                    message += $"{victim.name} had too high defence, it takes zero damage.";
                }
            }
        }
        else
        {
            message = $"{victim.name} absorb all dmg";
        }

        TextHandler(message, true);

        //if killed
        if (victim.HP <= 0)
        {
            TextHandler($"{victim.name} died", true);
            if (victim == player)
            {
                LoseTheGame();
                return;
            }
            else
            {
                WinTheGame();
                return;
            }
        }
        else
        {
            TextHandler($"{victim.name} have {victim.HP} hp left.", false);
            ChangeTurn(isPlayerTurn);
        }
    }

    void LoseTheGame()
    {
        TextHandler("You lost idiot", true);
    }
    void WinTheGame()
    {
        isFighting = false;

        TextHandler("You won, wow", true);
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
        TextHandler($"{unit.name}\n{unit.description}", true);
        ChangeTurn(isPlayerTurn);
    }

    public void TextHandler(string theText, bool shouldClear)
    {
        if (shouldClear) Console.Clear();

        Console.WriteLine(theText);
        Console.ReadLine();

    }
    public int ChoiceTextHandler(string theText, int numberOfChoices, bool shouldClear)
    {
        if (shouldClear) Console.Clear();

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
}