class Program
{
    public static bool ContinueGame = true;
    public static Random rnd = new Random();
    public static readonly char EnemySkin = 'O';
    public static readonly int EnemySpeed = 500;

    public class Map
    {
        private static readonly string[] save = File.ReadAllLines("map.txt");
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
            map[x][y] = ' ';
            SaveCursorPosition();
            Console.CursorLeft = x;
            Console.CursorTop = y;

            Console.Write(' ');
            ReturnCursorToSave();
        }

        public static void PrintCharacter(char c, int x, int y)
        {
            map[x][y] = c;
            SaveCursorPosition();
            Console.CursorLeft = x;
            Console.CursorTop = y;

            Console.Write(c);
            ReturnCursorToSave();
        }

        public Map(byte NumberOfEnemies, int EnemySpeed)
        {
            TurnToCharArray();

            for (byte i = 0; i < NumberOfEnemies; i++)
            {
                byte x = 2;// (byte)rnd.Next(5, NumberOfColumns - 1);
                byte y = 8;//(byte)rnd.Next(2, NumberOfRows - 1);

                Enemy Enemy = new Enemy(x, y);
                ListOfEnemies.Add(Enemy);
            }
        }
    }

    public class Enemy
    {
        public static byte x { get; set; }
        public static byte y { get; set; }
        public byte PreviousX = x, PreviousY = y;

        public static bool IsActionrunning = false;


        public Action MoveEnemy = () =>
        {
            IsActionrunning = true;

            while (ContinueGame)
            {
                switch (rnd.Next(1, 5))
                {
                    case 1:
                        {
                            //north
                            while (TryEnemyMove(x, y - 1))
                            {
                                Map.DeleteCharacter(x, y);
                                Map.PrintCharacter(EnemySkin, x, y - 1);
                                UpdatePositionTo(x, y - 1);
                                Thread.Sleep(EnemySpeed);
                            }
                            break;
                        }

                    case 2:
                        {
                            //south
                            while (TryEnemyMove(x, y + 1))
                            {
                                Map.DeleteCharacter(x, y);
                                Map.PrintCharacter(EnemySkin, x, y + 1);
                                UpdatePositionTo(x, y + 1);
                                Thread.Sleep(EnemySpeed);
                            }
                            break;
                        }

                    case 3:
                        {
                            //west
                            while (TryEnemyMove(x - 1, y))
                            {
                                Map.DeleteCharacter(x, y);
                                Map.PrintCharacter(EnemySkin, x - 1, y);
                                UpdatePositionTo(x - 1, y);
                                Thread.Sleep(EnemySpeed);
                            }
                            break;
                        }

                    case 4:
                        {
                            //east
                            while (TryEnemyMove(x + 1, y))
                            {
                                Map.DeleteCharacter(x, y);
                                Map.PrintCharacter(EnemySkin, x + 1, y);
                                UpdatePositionTo(x + 1, y);
                                Thread.Sleep(EnemySpeed);
                            }
                            break;
                        }
                }
            }
        };

        public static void UpdatePositionTo(int X, int Y)
        {
            x = (byte) X;
            y = (byte) Y;
        }

        public Enemy(byte Xvalue, byte Yvalue)
        {
            x = Xvalue;
            y = Yvalue;
        }
    }

    public static bool TryEnemyMove(int x, int y)
    {
        if (Map.map[x][y] != '#') return true;
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
        Console.CursorLeft = SaveCursorX;
        Console.CursorTop = SaveCursorY;
    }

    static void Main()
    {
        Map map = new Map(1, 5);

        // int i = 1;
        // foreach (Enemy e in map.ListOfEnemies)
        // {
        //     Console.WriteLine($"Enemy {i}:    X: {e.x} y: {e.y} ");
        //     i++;
        // }

        //Map.PrintMap();
        Map.PrintMap();

        foreach (Enemy e in map.ListOfEnemies)
        {
            Task.Run(e.MoveEnemy);
            Thread.Sleep(1000);
            Task.Run(e.MoveEnemy);
        }
        while(true);
    }






    
}