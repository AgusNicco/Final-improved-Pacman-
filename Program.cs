class Program
{

    public static bool ContinueGame = true;
    public static Random rnd = new Random();
    public static readonly char EnemySkin = 'O';
    public static int EnemySpeed;
    public static int Score;
    public static int CursorX = 1;
    public static int CursorY = 1;
    public static ConsoleColor PrintColor = ConsoleColor.Red;

    public static string MapChosen;

    public static bool Won = false;

    public static bool IsPrinting = false;

    public static List<char[]> OriginalMap = new List<char[]> { };

    public class Map
    {
        private static readonly string[] save = File.ReadAllLines(MapChosen);
        private static readonly int NumberOfRows = save.Length;
        private static readonly int NumberOfColumns = save[0].Length;

        public static char[][] map = new char[NumberOfRows][];

        public List<Enemy> ListOfEnemies = new List<Enemy> { };

        private static void TurnToCharArray()
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                map[i] = save[i].ToCharArray();
            }
        }

        public static void PrintMap()
        {
            Console.Clear();
            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    Console.Write(map[i][j]);
                    Thread.Sleep(3);
                }
                Console.WriteLine();
            }
        }

        public static void ColorSpecialCharacters()
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == '.')
                    {
                        Console.CursorTop = i;
                        Console.CursorLeft = j;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('.');
                        Thread.Sleep(4);
                    }
                    if (map[i][j] == EnemySkin)
                    {
                        Console.CursorTop = i;
                        Console.CursorLeft = j;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(EnemySkin);
                        Thread.Sleep(4);
                    }
                    if (map[i][j] == '*')
                    {
                        Console.CursorTop = i;
                        Console.CursorLeft = j;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('*');
                        Thread.Sleep(4);
                    }
                }
            }

        }

        public static void DeleteCharacter(int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;

            PrintColor = ConsoleColor.Yellow;
            map[y][x] = ' ';
            Console.Write(map[y][x]);
            ReturnCursorToSave();
        }

        public static void PrintCharacter(char c, int x, int y)
        {
            map[y][x] = c;
            Console.CursorLeft = x;
            Console.CursorTop = y;

            Console.Write(c);
            if (y == CursorY && x == CursorX) EndGame();
            ReturnCursorToSave();
            if (map[CursorY][CursorX] == '*')
            {
                Won = true;
                EndGame();
            }
        }

        public Map(int NumberOfEnemies, int EnemySpeedX)
        {
            TurnToCharArray();

            for (int i = 0; i < NumberOfEnemies; i++)
            {
                int x = 13;
                int y = 11;

                Enemy Enemy = new Enemy(x, y);
                ListOfEnemies.Add(Enemy);
            }

            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == EnemySkin)
                    {
                        Enemy Enemy = new Enemy(j, i);
                        ListOfEnemies.Add(Enemy);
                    }
                }
            }

            EnemySpeed = EnemySpeedX;
        }
    }


    public class Enemy
    {
        public int x { get; set; }
        public int y { get; set; }

        public char SaveChar;

        public static bool IsActionrunning = false;

        public Action MoveEnemy;

        public void UpdatePositionTo(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public Enemy(int Xvalue, int Yvalue)
        {
            x = Xvalue;
            y = Yvalue;
            SaveChar = ' ';

            MoveEnemy = () =>
            {
                IsActionrunning = true;

                while (ContinueGame)
                {
                    switch (rnd.Next(1, 5))
                    {
                        case 1:
                            {
                                //north
                                while (TryMove(x, y - 1))
                                {
                                    while (true)
                                    {
                                        Thread.Sleep(rnd.Next(1, 21));
                                        if (!IsPrinting)
                                        {
                                            IsPrinting = true;
                                            Thread.Sleep(10);
                                            Map.DeleteCharacter(x, y);

                                            Map.PrintCharacter(EnemySkin, x, y - 1);
                                            IsPrinting = false;
                                            break;
                                        }
                                    }
                                    UpdatePositionTo(x, y - 1);
                                    Thread.Sleep(EnemySpeed);
                                }
                                break;
                            }

                        case 2:
                            {
                                //south
                                while (TryMove(x, y + 1))
                                {
                                    while (true)
                                    {
                                        Thread.Sleep(rnd.Next(1, 21));
                                        if (!IsPrinting)
                                        {
                                            IsPrinting = true;
                                            Map.DeleteCharacter(x, y);
                                            Map.PrintCharacter(EnemySkin, x, y + 1);
                                            IsPrinting = false;
                                            break;
                                        }
                                    }
                                    UpdatePositionTo(x, y + 1);
                                    Thread.Sleep(EnemySpeed);
                                }
                                break;
                            }

                        case 3:
                            {
                                //west
                                while (TryMove(x - 1, y))
                                {
                                    while (true)
                                    {
                                        Thread.Sleep(rnd.Next(1, 21));
                                        if (!IsPrinting)
                                        {
                                            IsPrinting = true;
                                            Map.DeleteCharacter(x, y);
                                            Map.PrintCharacter(EnemySkin, x - 1, y);
                                            IsPrinting = false;
                                            break;
                                        }
                                    }
                                    UpdatePositionTo(x - 1, y);
                                    Thread.Sleep(EnemySpeed);
                                }
                                break;
                            }

                        case 4:
                            {
                                //east
                                while (TryMove(x + 1, y))
                                {
                                    while (true)
                                    {
                                        Thread.Sleep(rnd.Next(1, 21));
                                        if (!IsPrinting)
                                        {
                                            IsPrinting = true;
                                            Map.DeleteCharacter(x, y);
                                            Map.PrintCharacter(EnemySkin, x + 1, y);
                                            IsPrinting = false;
                                            break;
                                        }
                                    }
                                    UpdatePositionTo(x + 1, y);
                                    Thread.Sleep(EnemySpeed);
                                }
                                break;
                            }
                    }
                }
            };
        }
    }

    public static bool TryMove(int x, int y)
    {
        if (Map.map[y][x] != '#') return true;
        else return false;
    }

    public static void ReturnCursorToSave()
    {
        Console.CursorLeft = CursorX;
        Console.CursorTop = CursorY;
    }

    public static void ReadKey()
    {
        while (ContinueGame)
        {
            ConsoleKey UserInput = Console.ReadKey(true).Key;

            if (UserInput == ConsoleKey.W || UserInput == ConsoleKey.UpArrow)
            {
                if (TryMove(CursorX, CursorY - 1))
                {
                    CursorY -= 1;
                }
            }

            if (UserInput == ConsoleKey.S || UserInput == ConsoleKey.DownArrow)
            {
                if (TryMove(CursorX, CursorY + 1))
                {
                    CursorY += 1;
                }
            }

            if (UserInput == ConsoleKey.A || UserInput == ConsoleKey.LeftArrow)
            {
                if (TryMove(CursorX - 1, CursorY))
                {
                    CursorX -= 1;
                }
            }

            if (UserInput == ConsoleKey.D || UserInput == ConsoleKey.RightArrow)
            {
                if (TryMove(CursorX + 1, CursorY))
                {
                    CursorX += 1;
                }
            }
        }
    }

    public static void EndGame()
    {
        ContinueGame = false;

        if (!Won)
        {
            Console.Clear();
            Console.WriteLine("\nGame over. You lost.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nYour score was {Score}.\n");
        }
        if (Won)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nGame over. You won!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nYour score was {Score}.\n");
        }
    }

    public static void ChooseDifficulty()
    {
        Console.Clear();

        Console.WriteLine("Use the keyboard to choose the level of diffuculty: \n");
        Console.WriteLine("A: Easy");
        Console.WriteLine("B: Normal");
        Console.WriteLine("C: Hard");
        Console.WriteLine("D: Impossible");

        bool IsUserStupid = true;

        while (IsUserStupid)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.A:
                    {
                        MapChosen = "map1.txt";
                        IsUserStupid = false;
                        break;
                    }
                    case ConsoleKey.B:
                    {
                        MapChosen = "map2.txt";
                        IsUserStupid = false;
                        break;
                    }
                    case ConsoleKey.C:
                    {
                        MapChosen = "map3.txt";
                        IsUserStupid = false;
                        break;
                    }
                    case ConsoleKey.D:
                    {
                        MapChosen = "map4.txt";
                        IsUserStupid = false;
                        break;
                    }
            }
        }


    }

    static void Main()
    {
        ChooseDifficulty();
        // string[] save = File.ReadAllLines(MapChosen);


        // for (int i = 0; i < save.Length; i++)
        // {
        //     OriginalMap.Add(save[i].ToCharArray());
        // }


        Map map = new Map(0, 200);

        Map.PrintMap();
        Map.ColorSpecialCharacters();


        Action TouchedSpecialCharacter = () =>
        {
            while (ContinueGame)
            {
                Thread.Sleep(50);
                if (Map.map[CursorY][CursorX] == '.')
                {
                    while (true)
                    {
                        if (!IsPrinting)
                        {
                            IsPrinting = true;
                            Map.map[CursorY][CursorX] = ' ';
                            //OriginalMap[CursorY][CursorX] = ' ';
                            Console.CursorTop = CursorY;
                            Console.CursorLeft = CursorX;
                            Console.Write(' ');
                            Console.CursorTop = CursorY;
                            Console.CursorLeft = CursorX;
                            IsPrinting = false;
                            Score += 1;
                        }
                        break;
                    }
                }
            }
        };


        Task.Run(TouchedSpecialCharacter);


        Console.ForegroundColor = ConsoleColor.Red;

        foreach (Enemy e in map.ListOfEnemies)
        {
            Task.Run(e.MoveEnemy);
            Thread.Sleep(3);
        }
        ReadKey();
    }
}