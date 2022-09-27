GameManager gm = new GameManager();

gm.Start();
while (gm.hasWon != true)
{
    gm.Update();
}

Console.ReadLine();