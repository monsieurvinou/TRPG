using UnityEngine;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    Color defaultSelectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);


    /// <summary>
    /// Load a LevelData and instanciate all the tiles
    /// </summary>
    /// <param name="data">The level data</param>
    public void Load(LevelData data)
    {
        for (int i = 0; i < data.tiles.Count; ++i)
        {
            GameObject tilePrefabInstance = Instantiate(this.tilePrefab) as GameObject;
            Tile tile = tilePrefabInstance.GetComponent<Tile>();
            tile.Load(data.tiles[i]);
            this.tiles.Add(tile.pos, tile);
        }
    }

    /// <summary>
    /// Search all the tiles that can be traveled to
    /// </summary>
    /// <param name="start">Tile of start</param>
    /// <param name="addTile">Method to add tiles to the queue</param>
    /// <returns></returns>
    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile)
    {
        List<Tile> retValue = new List<Tile>();
        retValue.Add(start);

        ClearSearch();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.distance = 0;
        checkNow.Enqueue(start);
        Point[] dirs = new Point[4]
        {
            new Point(0, 1),
            new Point(0, -1),
            new Point(1, 0),
            new Point(-1, 0)
        };

        while (checkNow.Count > 0)
        {
            Tile tile = checkNow.Dequeue();

            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(tile.pos + dirs[i]);

                if (next != null && next.distance > tile.distance + 1)
                {
                    if (addTile(tile, next))
                    {
                        next.distance = tile.distance + 1;
                        next.prev = tile;
                        checkNext.Enqueue(next);
                        retValue.Add(next);
                    }
                }
            }

            if (checkNow.Count == 0)
                SwapReference(ref checkNow, ref checkNext);
        }

        return retValue;
    }

    /// <summary>
    /// Get a tile at a position
    /// </summary>
    /// <param name="point">The coordinate</param>
    /// <returns>The tile or null</returns>
    public Tile GetTile(Point point)
    {
        return tiles.ContainsKey(point) ? tiles[point] : null;
    }

    /// <summary>
    /// Add coloration to the selectables tiles
    /// </summary>
    /// <param name="tiles">List of tiles to color</param>
    public void SelectTiles(List<Tile> tiles, Color? color = null)
    {
        Color selectedTileColor = color ?? defaultSelectedTileColor;
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
    }

    /// <summary>
    /// Add coloration to all the non selectable tiles
    /// </summary>
    /// <param name="tiles">List of tiles to color</param>
    public void DeSelectTiles(List<Tile> tiles, Color? color = null)
    {
        Color deSelectedTileColor = color ?? defaultTileColor;
        for (int i = tiles.Count - 1; i >= 0; --i)
            tiles[i].GetComponent<Renderer>().material.SetColor("_Color", deSelectedTileColor);
    }

    /// <summary>
    /// Clear & clean the search attributes
    /// </summary>
    void ClearSearch()
    {
        foreach (Tile tile in tiles.Values)
        {
            tile.prev = null;
            tile.distance = int.MaxValue;
        }
    }

    /// <summary>
    /// Swap reference between 2 queues
    /// </summary>
    /// <param name="queueTileA"></param>
    /// <param name="queueTileB"></param>
    void SwapReference(ref Queue<Tile> queueTileA, ref Queue<Tile> queueTileB)
    {
        Queue<Tile> temp = queueTileA;
        queueTileA = queueTileB;
        queueTileB = temp;
    }
}
