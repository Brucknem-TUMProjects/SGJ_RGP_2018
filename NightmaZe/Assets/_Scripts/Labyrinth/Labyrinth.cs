using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/**
 * --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 *
 *                      Unverändert übernommen aus der Projektbeschreibung,
 *
 *                      Nicht als eigener Verdienst zu werten.
 *
 * --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
 */


public class Labyrinth 
{
    // Anmerkung: Dieser Labyrinthgenerator ist prozedural programmiert
    // und aeusserst unzureichend kommentiert und dokumentiert. Er soll
    // keine Anregungen fuer die Umsetzung des Projekts bieten, sondern
    // dient ausschliesslich der Labyrintherzeugung.

    public Labyrinth(int seed)
    {
        random = new System.Random(seed);
        Init(1); 
    }

    public enum Tile
    {
        WALL, ENTRY, EXIT, STATIC_TRAP, DYNAMIC_TRAP, KEY, FREE, SPAWN
    }

    private static System.Random random;

    public class Properties
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int InNr { get; set; }
        public int OutNr { get; set; }
        public int StaticNr { get; set; }
        public int DynamicNr { get; set; }
        public int KeyNr { get; set; }

        public double Density { get; set; }


        public Properties(int width, int height, int inNr, int outNr, int staticNr, int dynamicNr, int keyNr, int density)
        {
            Width = width;
            Height = height;
            InNr = inNr;
            OutNr = outNr;
            StaticNr = staticNr;
            DynamicNr = dynamicNr;
            KeyNr = keyNr;
            Density = density;
        }

        public Properties(int difficulty) : this(
            random.Next(50, 100) * difficulty,
            random.Next(30, 50) * difficulty,
            random.Next(4, 5) * difficulty,
            random.Next(4, 5) * difficulty,
            random.Next(3, 4) * difficulty,
            random.Next(3, 4) * difficulty,
            2 * difficulty,
            random.Next(1, difficulty))
        { }
    }

    private Properties properties;

    int[][] points;
    private Tile[,] tiles;
    bool[,] occupied;

    public Tile[,] GetTiles()
    {
        return tiles;
    }

    public Properties GetProperties()
    {
        return properties;
    }

    public void Init(int difficulty)
    {
        Init(new Properties(difficulty));
    }

    public void Init(int width, int height, int tileSize, int inNr, int outNr, int staticNr, int dynamicNr, int keyNr, int density)
    {
        Init(new Properties(width, height, inNr, outNr, staticNr, dynamicNr, keyNr, density));
    }

    public void Init(Properties properties)
    {
        this.properties = properties;

        //TODO evtl seed random
        tiles = new Tile[properties.Height, properties.Width];
        occupied = new bool[properties.Height, properties.Width];
        for (int x = properties.Height - 1; x > -1; x--)
        {
            for (int y = 0; y < properties.Width; y++)
            {
                tiles[x, y] = Tile.WALL;
                occupied[x, y] = false;
            }
        }
        int idx = 1, nrRand = (int)((3.0 / properties.Density) * (6 + Math.Abs(random.Next(0, (2 * properties.Width + 2 * properties.Height) + 1))));
        points = new int[properties.InNr + properties.OutNr + properties.StaticNr + properties.DynamicNr + properties.KeyNr + nrRand][];
        if (points.Length >= (properties.Height - 2) * (properties.Width - 3))
        {
            //Debug.Log("Field too small. Choose larger one!");
            //throw new System.Exception("field too small");
        }
        // In+Out
        points[0] = new int[] { Modnar(1, properties.Height - 2), 0 };
        MarkIO(points[0][0], points[0][1], Tile.ENTRY);
        for (int i = 1; i < properties.InNr + properties.OutNr; i++, idx++)
        {
            points[idx] = Advance(points[idx - 1][0], points[idx - 1][1], Modnar(1, properties.Width + properties.Height));
            MarkIO(points[idx][0], points[idx][1], i < properties.InNr ? Tile.ENTRY : Tile.EXIT);
        }
        // Keys, Traps, random points
        while (idx < points.Length)
        {
            do
            {
                points[idx] = new int[] { Modnar(1, properties.Height - 2), Modnar(1, properties.Width - 2) };
            } while (tiles[points[idx][0], points[idx][1]] != Tile.WALL);
            if (idx < properties.InNr + properties.OutNr + properties.KeyNr)
            {
                tiles[points[idx][0], points[idx][1]] = Tile.KEY;
            }
            else if (idx < properties.InNr + properties.OutNr + properties.KeyNr + properties.StaticNr)
            {
                tiles[points[idx][0], points[idx][1]] = Tile.STATIC_TRAP;
            }
            else if (idx < properties.InNr + properties.OutNr + properties.KeyNr + properties.StaticNr + properties.DynamicNr)
            {
                tiles[points[idx][0], points[idx][1]] = Tile.DYNAMIC_TRAP;
            }
            else
            {
                tiles[points[idx][0], points[idx][1]] = Tile.FREE;
            }
            idx++;
        }
        occupied[points[points.Length - 1][0], points[points.Length - 1][1]] = true;

        Compute();
    }

    private void Compute()
    {
        int idx = Modnar(1, points.Length - 1);
        bool[] used = new bool[points.Length];
        for (int i = 0; i < used.Length; i++)
        {
            used[i] = false;
        }
        for (int i = 0; i < points.Length; i++)
        {
            idx = (idx + Modnar(1, points.Length - 1)) % points.Length;
            while (used[idx])
            {
                idx = (idx + 1) % points.Length;
            }
            used[idx] = true;
            int x = Math.Min(properties.Height - 2, Math.Max(1, points[idx][0])),
                    y = Math.Min(properties.Width - 2, Math.Max(1, points[idx][1]));
            int[] cp = ConnectionPoint(x, y);
            Connect(x, y, cp[0], cp[1]);
        }
        //System.IO.File.WriteAllText(@"D:\test.txt", ToString());
    }

    //Computation methods
    private int[] Nxt(int x, int y)
    {
        bool bad = tiles[x, y] != Tile.WALL;
        if (x == 0)
        {
            if (y == 1)
            {
                return bad ? Nxt(1, 0) : new int[] { 1, 0 };
            }
            return bad ? Nxt(x, y - 1) : new int[] { x, y - 1 };
        }
        else if (x == properties.Height - 1)
        {
            if (y == properties.Width - 2)
            {
                return bad ? Nxt(properties.Height - 2, properties.Width - 1) : new int[] { properties.Height - 2, properties.Width - 1 };
            }
            return bad ? Nxt(x, y + 1) : new int[] { x, y + 1 };
        }
        else if (y == 0)
        {
            if (x == properties.Height - 2)
            {
                return bad ? Nxt(properties.Height - 1, 1) : new int[] { properties.Height - 1, 1 };
            }
            return bad ? Nxt(x + 1, y) : new int[] { x + 1, y };
        }
        else
        {
            if (x == 1)
            {
                return bad ? Nxt(0, properties.Width - 2) : new int[] { 0, properties.Width - 2 };
            }
            return bad ? Nxt(x - 1, y) : new int[] { x - 1, y };
        }
    }

    private int[] Advance(int x, int y, int num)
    {
        for (int i = 0; i < num; i++)
        {
            int[] xy = Nxt(x, y);
            x = xy[0];
            y = xy[1];
        }
        for (int i = 0; i < 2 * (properties.Height + properties.Width); i++)
        {
            if (tiles[x, y] == Tile.WALL)
            {
                return new int[] { x, y };
            }
            int[] xy = Nxt(x, y);
            x = xy[0];
            y = xy[1];
            xy = Nxt(x, y);
            x = xy[0];
            y = xy[1];
        }
        return new int[] { -1, -1 };
    }

    private void MarkIO(int x, int y, Tile c)
    {
        tiles[x, y] = c;
        if (x == 0)
        {
            tiles[1, y] = Tile.FREE;
        }
        else if (x == properties.Height - 1)
        {
            tiles[properties.Height - 2, y] = Tile.FREE;
        }
        else if (y == 0)
        {
            tiles[x, 1] = Tile.FREE;
        }
        else
        {
            tiles[x, properties.Width - 2] = Tile.FREE;
        }
    }

    private int Modnar(int low, int high)
    {
        if (low >= high)
        {
            return low;
        }
        return Math.Abs(random.Next(0, 1 + high - low)) + low;
    }

    private bool Chance(int percentage)
    {
        return Math.Abs(random.Next(0, 100)) < percentage;
    }

    private int[] ConnectionPoint(int x, int y)
    {
        for (int dist = 1; dist < 10000; dist++)
        {
            for (int xx = x - dist < 1 ? 1 : x - dist; xx < properties.Height - 1 && xx - x < dist + 1; xx++)
            {
                if (y + dist < properties.Width && occupied[xx, y + dist])
                {
                    int[] res = { xx, y + dist };
                    return res;
                }
                if (y - dist > 0 && occupied[xx, y - dist])
                {
                    int[] res = { xx, y - dist };
                    return res;
                }
            }
            for (int yy = y - dist < 1 ? 1 : y - dist; yy < properties.Width - 1 && yy - y < dist + 1; yy++)
            {
                if (x + dist < properties.Height && occupied[x + dist, yy])
                {
                    int[] res = { x + dist, yy };
                    return res;
                }
                if (x - dist > 0 && occupied[x - dist, yy])
                {
                    int[] res = { x - dist, yy };
                    return res;
                }
            }
        }
        return new int[] { x, y };
    }

    private void DirX(int y, int x1, int x2)
    {
        if (x1 == x2)
        {
            return;
        }
        if (tiles[x1, y] == Tile.WALL)
        {
            tiles[x1, y] = Tile.FREE;
        }
        if (!occupied[x1, y])
        {
            occupied[x1, y] = true;
        }
        DirX(y, x1 + 1, x2);
    }

    private void DirY(int x, int y1, int y2)
    {
        if (y1 == y2)
        {
            return;
        }
        if (tiles[x, y1] == Tile.WALL)
        {
            tiles[x, y1] = Tile.FREE;
        }
        if (!occupied[x, y1])
        {
            occupied[x, y1] = true;
        }
        DirY(x, y1 + 1, y2);
    }

    private void Connect(int x1, int y1, int x2, int y2)
    {
        int choice = 0, minX = Math.Min(x1, x2), maxX = Math.Max(x1, x2), minY = Math.Min(y1, y2), maxY = Math.Max(y1, y2);
        if (y1 == y2)
        {
            DirX(y1, minX, maxX);
            return;
        }
        if (x1 == x2)
        {
            DirY(x1, minY, maxY);
            return;
        }
        if (tiles[x1, y1] == Tile.KEY)
        {
            //if (field[x1,y1]==rP) field[x1,y1]=Tile.FREE;
            choice = Chance(50) ? 1 : 2;
        }
        if (choice == 1)
        {
            if (Chance(50))
            {
                DirX(minY, minX, maxX);
                if (x1 < x2 == y1 < y2)
                {
                    DirY(maxX, minY, maxY);
                    if (tiles[maxX, minY] == Tile.WALL)
                    {
                        tiles[maxX, minY] = Tile.FREE;
                    }
                }
                else
                {
                    DirY(minX, minY, maxY);
                    if (tiles[minX, minY] == Tile.WALL)
                    {
                        tiles[minX, minY] = Tile.FREE;
                    }
                }
            }
            else
            {
                DirX(maxY, minX, maxX);
                if (x1 < x2 == y1 < y2)
                {
                    DirY(minX, minY, maxY);
                    if (tiles[minX, maxY] == Tile.WALL)
                    {
                        tiles[minX, maxY] = Tile.FREE;
                    }
                }
                else
                {
                    DirY(maxX, minY, maxY);
                    if (tiles[maxX, maxY] == Tile.WALL)
                    {
                        tiles[maxX, maxY] = Tile.FREE;
                    }
                }
            }
        }
        else if (choice == 2 || tiles[x1, y1] == Tile.STATIC_TRAP || tiles[x1, y1] == Tile.DYNAMIC_TRAP || tiles[x1, y1] == Tile.FREE)
        {
            DirX(minY, minX, maxX);
            DirX(maxY, minX, maxX);
            DirY(minX, minY, maxY);
            DirY(maxX, minY, maxY);
            if (tiles[x1, y2] == Tile.WALL)
            {
                tiles[x1, y2] = Tile.FREE;
            }
            if (tiles[x2, y1] == Tile.WALL)
            {
                tiles[x2, y1] = Tile.FREE;
            }
        }
    }

    public override string ToString()
    {
        string s = "";

        for (int x = properties.Height - 1; x > -1; x--)
        {
            for (int y = 0; y < properties.Width; y++)
            {
                s += tiles[x, y] + "";
            }
            s += "\n";
        }

        return s;
    }
}