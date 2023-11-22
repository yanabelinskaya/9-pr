using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

enum Border
{
    MaxRight = 40,
    MaxBottom = 20
}

class SnakeGame
{
    private List<int[]> snake;
    private int[] food;
    private Direction direction;
    private bool gameover;

    public SnakeGame()
    {
        Console.WriteLine("Добро пожаловать в игру Змейка!");
        Console.WriteLine("Цель игры: собирать еду (⚽), чтобы увеличить длину змейки.");
        Console.WriteLine("Управление и правила: используйте стрелки (вверх, вниз, влево, вправо). При столкновение со стенкой игра заканчивается");
        Console.WriteLine("Нажмите 'да', чтобы начать игру, или 'нет', чтобы завершить программу.");

        string startResponse = Console.ReadLine().ToLower();

        while (startResponse == "да")
        {
            StartGame();
            startResponse = Console.ReadLine().ToLower();
        }

        Console.WriteLine("Спасибо за игру. До свидания!");
    }

    private void StartGame()
    {
        snake = new List<int[]> { new int[] { 5, 5 } }; // начальная позиция змейки
        food = GenerateFood();
        direction = Direction.Right;
        gameover = false;

        Thread thread = new Thread(new ThreadStart(DrawGame));
        thread.Start();

        while (!gameover)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            HandleInput(keyInfo);
        }

        EndGame();
    }

    private void DrawGame()
    {
        while (!gameover)
        {
            Console.Clear();
            DrawBorder();
            DrawSnake();
            DrawFood();
            Move();
            CheckCollision();
            Thread.Sleep(100); // скорость змейки
        }
    }

    private void DrawBorder()
    {
        Console.SetCursorPosition(0, 0);

        for (int i = 0; i <= (int)Border.MaxRight; i++)
        {
            Console.Write("─");
        }

        for (int i = 1; i <= (int)Border.MaxBottom; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("│");
            Console.SetCursorPosition((int)Border.MaxRight, i);
            Console.Write("│");
        }

        Console.SetCursorPosition(0, (int)Border.MaxBottom);
        for (int i = 0; i <= (int)Border.MaxRight; i++)
        {
            Console.Write("─");
        }
    }

    private void DrawSnake()
    {
        foreach (var segment in snake)
        {
            Console.SetCursorPosition(segment[0], segment[1]);
            Console.Write("■");
        }
    }

    private void DrawFood()
    {
        Console.SetCursorPosition(food[0], food[1]);
        Console.Write("⚽");
    }

    private void Move()
    {
        int[] head = snake.First().ToArray();

        switch (direction)
        {
            case Direction.Up:
                head[1]--;
                break;
            case Direction.Down:
                head[1]++;
                break;
            case Direction.Left:
                head[0]--;
                break;
            case Direction.Right:
                head[0]++;
                break;
        }

        if (head[0] <= 0 || head[0] >= (int)Border.MaxRight || head[1] <= 0 || head[1] >= (int)Border.MaxBottom)
        {
            gameover = true;
        }

        snake.Insert(0, head);

        if (head[0] == food[0] && head[1] == food[1])
        {
            food = GenerateFood();
        }
        else
        {
            snake.RemoveAt(snake.Count - 1);
        }
    }

    private void EndGame()
    {
        Console.Clear();
        Console.WriteLine("Игра завершена!");
        Console.WriteLine($"Вы собрали {snake.Count - 1} еды");

        Console.WriteLine("Хотите сыграть еще раз? (да/нет)");
    }

    private void CheckCollision()
    {
        int[] head = snake.First().ToArray();

        if (head[0] <= 0 || head[0] >= (int)Border.MaxRight || head[1] <= 0 || head[1] >= (
        int)Border.MaxBottom)
        {
            gameover = true;
        }

        if (snake.Count != snake.Distinct().Count())
        {
            gameover = true;
        }
    }

    private int[] GenerateFood()
    {
        Random random = new Random();
        int x = random.Next(1, (int)Border.MaxRight);
        int y = random.Next(1, (int)Border.MaxBottom);

        return new int[] { x, y };
    }

    private void HandleInput(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                direction = Direction.Up;
                break;
            case ConsoleKey.DownArrow:
                direction = Direction.Down;
                break;
            case ConsoleKey.LeftArrow:
                direction = Direction.Left;
                break;
            case ConsoleKey.RightArrow:
                direction = Direction.Right;
                break;
        }
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
}

class Program
{
    static void Main()
    {
        SnakeGame game = new SnakeGame();
    }
}
