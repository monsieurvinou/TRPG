using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    public Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

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
}
