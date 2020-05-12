using System;
using System.Collections.Generic;
using System.Threading;


public struct Cell
{

    public bool current;
    public bool prepare;
    public int x;
    public int y;
    public Cell(int x, int y, bool isAlive)
    {
        this.x = x;
        this.y = y;
        this.current = false;
        this.prepare = false;
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
        int width = this.world.width;
        int height = this.world.height;

        Cell current_cell = this.world.cells[y][x];
        if (this.world.cells[(y + 1) % height][x].current)
        {
            alives++;
        }
        if (this.world.cells[(y - 1 + height) % height][x].current)
        {
            alives++;
        }
        if (this.world.cells[y][(x + 1) % width].current)
        {
            alives++;
        }
        if (this.world.cells[y][(x - 1 + width) % width].current)
        {
            alives++;
        }
        if (this.world.cells[(y - 1 + height) % height][(x - 1 + width) % width].current)
        {
            alives++;
        }
        if (this.world.cells[(y + 1) % height][(x + 1) % width].current)
        {
            alives++;
        }
        if (this.world.cells[(y - 1 + height) % height][(x + 1) % width].current)
        {
            alives++;
        }
        if (this.world.cells[(y + 1) % height][(x - 1 + width) % width].current)
        {
            alives++;
        }

        return alives;
    }
    private void prepare()
    {
        for (int lineno = 0; lineno < this.world.height; lineno++)
        {
            for (int colno = 0; colno < this.world.width; colno++)
            {
                int alives = count_alive_around(lineno, colno);
                ref Cell current_cell = ref this.world.cells[lineno][colno];
                if (!current_cell.current & alives == 3) {
                    current_cell.prepare = true;
                }
                else if (alives < 2 | alives > 3) {
                    current_cell.prepare = false;
                }
                else {
                    current_cell.prepare = current_cell.current;
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
                current_cell.current = current_cell.prepare;
            }
        }
    }

    private void render()
    {
        for (int lineno = 0; lineno < this.world.height; lineno++)
        {
            for (int colno = 0; colno < this.world.width; colno++)
            {
                Cell current_cell = this.world.cells[lineno][colno];
                if (current_cell.current)
                {
                    Console.Write("W");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.Write("\n");
        }
    }
    public void step()
    {
        prepare();
        commit();
        render();
    }
}


namespace Program
{
    class LifeRunner
    {
        static void Main(string[] args)
        {
            var game = new GOLRunner(30, 30);
            game.world.cells[1][3].current = true;
            game.world.cells[3][2].current = true;
            game.world.cells[3][3].current = true;
            game.world.cells[3][4].current = true;
            game.world.cells[2][4].current = true;
            while (true)
            {
                Console.Clear();
                game.step();
                Thread.Sleep(300);
            }
        }
    }
}
