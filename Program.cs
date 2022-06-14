class Program
{
    public static bool ContinueGame = true;
    public static Random rnd = new Random();
    public static readonly char EnemySkin = 'O';
    public static int EnemySpeed;
    public static byte CursorX = 1;
    public static byte CursorY = 1;

    public static bool IsPrinting = false;



    public class Map
    {
        private static readonly string[] save = File.ReadAllLines("map2.txt");
        private static readonly byte NumberOfRows = (byte)save.Length;
        private static readonly byte NumberOfColumns = (byte)save[0].Length;

        public static char[][] map = new char[NumberOfRows][];

        public List<Enemy> ListOfEnemies = new List<Enemy> { };

        private static void TurnToCharArray()
        {
            for (byte i = 0; i < NumberOfRows; i++)
            {
                map[i] = save[i].ToCharArray();
            }
        }

        public static void PrintMap()
        {
            Console.Clear();
            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            for (byte i = 0; i < NumberOfRows; i++)
            {
                for (byte j = 0; j < map[i].Length; j++)
                {
                    Console.Write(map[i][j]);
                }
                Console.WriteLine();
            }
        }

        public static void DeleteCharacter(byte x, byte y)
        {
            map[y][x] = ' ';
            Console.CursorLeft = x;
            Console.CursorTop = y;

            Console.Write(' ');
            ReturnCursorToSave();
        }

        public static void PrintCharacter(char c, int x, int y)
        {

            map[y][x] = c;
            Console.CursorLeft = x;
            Console.CursorTop = y;

            Console.Write(c);
            ReturnCursorToSave();

        }

        public Map(byte NumberOfEnemies, int EnemySpeedX)
        {
            TurnToCharArray();

            for (byte i = 0; i < NumberOfEnemies; i++)
            {
                int x = 13;// (byte)rnd.Next(5, NumberOfColumns - 1);
                int y = 11;//(byte)rnd.Next(2, NumberOfRows - 1);

                Enemy Enemy = new Enemy((byte)x, (byte)y);
                ListOfEnemies.Add(Enemy);
            }

            EnemySpeed = EnemySpeedX;
        }
    }



    public class Enemy
    {
        public byte x { get; set; }
        public byte y { get; set; }

        public static bool IsActionrunning = false;

        public Action MoveEnemy;

        public void UpdatePositionTo(int X, int Y)
        {
            x = (byte)X;
            y = (byte)Y;
        }

        public Enemy(byte Xvalue, byte Yvalue)
        {
            x = Xvalue;
            y = Yvalue;

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

    private static int SaveCursorX, SaveCursorY;
    public static void SaveCursorPosition()
    {
        SaveCursorX = Console.CursorLeft;
        SaveCursorY = Console.CursorTop;
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

        Console.Clear();
        Console.WriteLine("\nGame over\n");
    }
    static void Main()
    {
        Map map = new Map(12, 175);

        Map.PrintMap();

        Action TouchedEnemy = () =>
        {
            while (ContinueGame)
            {
                while (true)
                {
                    Thread.Sleep(rnd.Next(1, 21));
                    if (!IsPrinting)
                    {
                        foreach (Enemy e in map.ListOfEnemies)
                        {
                            if (CursorX == e.x && CursorY == e.y)
                            {
                                EndGame();
                            }
                        }
                        break;
                    }
                }
            }
        };

        Task.Run(TouchedEnemy);
        Console.ForegroundColor = ConsoleColor.Red;

        foreach (Enemy e in map.ListOfEnemies)
        {
            Task.Run(e.MoveEnemy);
            Thread.Sleep(13);
        }
        ReadKey();
    }
}