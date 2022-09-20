
GameManager gm = new GameManager(player);

gm.Start();
while (gm.hasWon != true)
{
    gm.Update();
}

Console.ReadLine();


