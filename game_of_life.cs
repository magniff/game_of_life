using System;
using System.Threading;
using CommandLine;


public struct Cell
{
    // currently alive
    public bool isAlive;
    // place to store decision on whether the cell will be alive tomorrow
    public bool willBeAlive;
    // position x
    public int x;
    // position y
    public int y;
    public Cell(int x, int y, bool isAlive = true)
    {
        this.x = x;
        this.y = y;
        this.isAlive = isAlive;
        this.willBeAlive = isAlive;
    }
}


public struct World
{
    public readonly int width;
    public readonly int height;
    public Cell[][] cells;
    public World(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.cells = new Cell[height][];
        for (int y_pos = 0; y_pos < height; y_pos++)
        {
            Cell[] cells_line = new Cell[width];
            for (int x_pos = 0; x_pos < width; x_pos++)
            {
                cells_line[x_pos] = new Cell(x: x_pos, y: y_pos, isAlive: false);
            }
            this.cells[y_pos] = cells_line;
        }
    }
}

class GOLRunner
{
    public World world;
    public GOLRunner(int width, int height)
    {
        this.world = new World(width, height);
    }

    public int count_alive_around(int y, int x)
    {
        int alives = 0;

        // handy aliases
        int width = world.width;
        int height = world.height;
        ref Cell[][] cells = ref world.cells;
        ref Cell current_cell = ref world.cells[y][x];

        // Checking 8 corner tiles around with respect to the tor topology
        if (cells[(y + 1) % height][x].isAlive)
        {
            alives++;
        }
        if (cells[(y - 1 + height) % height][x].isAlive)
        {
            alives++;
        }
        if (cells[y][(x + 1) % width].isAlive)
        {
            alives++;
        }
        if (cells[y][(x - 1 + width) % width].isAlive)
        {
            alives++;
        }
        if (cells[(y - 1 + height) % height][(x - 1 + width) % width].isAlive)
        {
            alives++;
        }
        if (cells[(y + 1) % height][(x + 1) % width].isAlive)
        {
            alives++;
        }
        if (cells[(y - 1 + height) % height][(x + 1) % width].isAlive)
        {
            alives++;
        }
        if (cells[(y + 1) % height][(x - 1 + width) % width].isAlive)
        {
            alives++;
        }
        return alives;
    }

    private void prepare()
    {
        for (int lineno = 0; lineno < world.height; lineno++)
        {
            for (int colno = 0; colno < world.width; colno++)
            {
                int alives = count_alive_around(lineno, colno);
                ref Cell current_cell = ref world.cells[lineno][colno];

                if (!current_cell.isAlive & alives == 3)
                {
                    // Breeding case
                    current_cell.willBeAlive = true;
                }
                else if (alives < 2 | alives > 3)
                {
                    // {Over,under}population case
                    current_cell.willBeAlive = false;
                }
                else
                {
                    // Default case
                    current_cell.willBeAlive = current_cell.isAlive;
                }
            }
        }
    }

    private void commit()
    {
        for (int lineno = 0; lineno < this.world.height; lineno++)
        {
            for (int colno = 0; colno < this.world.width; colno++)
            {
                ref var current_cell = ref this.world.cells[lineno][colno];
                current_cell.isAlive = current_cell.willBeAlive;
            }
        }
    }

    private string render()
    {
        string output = "";
        for (int lineno = 0; lineno < world.height; lineno++)
        {
            for (int colno = 0; colno < world.width; colno++)
            {
                Cell current_cell = world.cells[lineno][colno];
                if (current_cell.isAlive)
                {
                    output += "W";
                }
                else
                {
                    output += ".";
                }
            }
            output += "\n";
        }
        return output;
    }

    public string step()
    {
        prepare();
        commit();
        return render();
    }
}


namespace Program
{
    class LifeRunner
    {
        public class Options
        {
            [Option('w', "width", Required = true, HelpText = "Width of the GOL universe.")]
            public int Width { get; set; }
            [Option('h', "height", Required = true, HelpText = "Height of the GOL universe.")]
            public int Height { get; set; }
            [Option('i', "init", Required = true, HelpText = "Number of initially alive cells.")]
            public int Init { get; set; }
            [Option('s', "step", Required = false, HelpText = "Simulation step length in milliseconds.", Default = 20)]
            public int Step { get; set; }
        }
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(
                    parsed =>
                    {
                        int generation = 0;
                        var rand = new Random();
                        var game = new GOLRunner(width: parsed.Width, height: parsed.Height);
                        for (int counter = 0; counter < parsed.Init; counter++)
                        {
                            int x = rand.Next() % game.world.width;
                            int y = rand.Next() % game.world.height;
                            game.world.cells[y][x].isAlive = true;
                        }
                        while (true)
                        {
                            Console.Clear();
                            Console.Write(game.step());
                            Console.Write(generation);
                            generation++;
                            Thread.Sleep(parsed.Step);
                        }
                    }
                );
        }
    }
}
