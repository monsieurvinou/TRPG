using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public string fileName = "UNNAMED_LEVEL";
    [SerializeField] GameObject marker;
    [SerializeField] int width = 10;
    [SerializeField] int depth = 10;
    [SerializeField] int height = 8;
    [SerializeField] GameObject tileViewPrefab;
    protected Point[] positions;
    public LevelData fileToLoad;
    
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();

    /// <summary>
    /// Grow the tiles of the entire board
    /// </summary>
    public void GrowAll()
    {
        Rect rectangle = new Rect(0, 0, width, depth);
        LevelRect(rectangle);
    }

    /// <summary>
    /// Shrink the tiles of the entire board
    /// </summary>
    public void ShrinkAll()
    {
        Rect rectangle = new Rect(0, 0, width, depth);
        LevelRect(rectangle, -1);
    }

    /// <summary>
    /// Grow the tiles of an area
    /// </summary>
    public void GrowArea()
    {
        Rect r = RandomRect();
        LevelRect(r);
    }

    /// <summary>
    /// Shrink the tiles of an area
    /// </summary>
    public void ShrinkArea()
    {
        Rect r = RandomRect();
        LevelRect(r, -1);
    }

    /// <summary>
    /// Grow the tile where the attribute "pos" is at
    /// </summary>
    public void Grow()
    {
        LevelMultiple(positions);
    }

    /// <summary>
    /// Shrink the tile where the attribute "pos" is at
    /// </summary>
    public void Shrink()
    {
        LevelMultiple(positions, -1);
    }

    /// <summary>
    /// Update the marker position
    /// </summary>
    public void UpdateMarker()
    {
        if (marker != null)
        {
            if (this.positions != null)
            {
                Point position = this.positions.Length > 0 ? this.positions[0] : new Point(0, 0);
                Tile tile = tiles.ContainsKey(position) ? tiles[position] : null;
                marker.transform.localPosition = tile != null ? tile.center : new Vector3(position.x, 0, position.y);
            }
        }
    }

    /// <summary>
    /// Clear the board
    /// </summary>
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        tiles.Clear();
    }

    /// <summary>
    /// Save the current board
    /// </summary>
    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Boards";
        if (!Directory.Exists(filePath))
            CreateSaveDirectory();

        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.tiles = new List<Vector3>(this.tiles.Count);
        foreach (Tile tileToSave in this.tiles.Values)
        {
            board.tiles.Add(new Vector3(tileToSave.pos.x, tileToSave.height, tileToSave.pos.y));
        }

        string fileName = string.Format("Assets/Resources/Boards/{1}.asset", filePath, this.fileName);
        AssetDatabase.CreateAsset(board, fileName);
    }

    /// <summary>
    /// Load a board
    /// </summary>
    public void Load()
    {
        if (fileToLoad != null) {
            // we clear the board
            Clear();

            // we load every tile describe in the level data
            foreach (Vector3 tileVector in fileToLoad.tiles)
            {
                Tile tile = Create();
                tile.Load(tileVector);
                tiles.Add(tile.pos, tile);
            }

            this.fileName = fileToLoad.name;
        }
    }

    /// <summary>
    /// Update the array of positions to grow / shrink
    /// </summary>
    /// <param name="positions">An array of positions</param>
    public void setPositions(Point[] positions)
    {
        this.positions = positions;
    }

    /// <summary>
    /// Change the height of multiple tiles
    /// </summary>
    /// <param name="positions">An array of positions of tiles</param>
    /// <param name="direction">Positive for Grow, negative for Shrink</param>
    void LevelMultiple(Point[] positions, int direction = 1)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            LevelSingle(positions[i], direction);
        }
    }

    /// <summary>
    /// Create the directory if it does not exist
    /// </summary>
    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets", "Resources");
        filePath += "/Levels";
        if (!Directory.Exists(filePath))
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Level all the tiles in the rectangle
    /// </summary>
    /// <param name="rect">The rectangle</param>
    /// <param name="direction">Positive for Grow, negative for Shrink</param>
    void LevelRect(Rect rect, int direction = 1)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point point = new Point(x, y);
                LevelSingle(point, direction);
            }
        }
    }

    /// <summary>
    /// Instantiate a tile prefab linked to the script at the attribute "pos" position
    /// </summary>
    /// <returns>The tile instantiated</returns>
    protected Tile Create()
    {
        GameObject instance = Instantiate(tileViewPrefab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    /// <summary>
    /// Determine if a tile exist at the point. If not, instantiate one.
    /// </summary>
    /// <param name="p">The point</param>
    /// <returns>The tile instantiated or already existing</returns>
    protected Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    /// <summary>
    /// Level a single tile
    /// </summary>
    /// <param name="point">Position of th tile</param>
    /// <param name="direction">Positive for Grow, negative for Shrink</param>
    protected void LevelSingle(Point point, int direction = 1)
    {
        if (direction > 0)
        {
            Tile tile = GetOrCreate(point);

            if (tile.height < this.height)
            {
                tile.Grow();
            }
        }
        else
        {
            if (tiles.ContainsKey(point))
            {
                Tile tile = tiles[point];
                tile.Shrink();

                if (tile.height <= 0)
                {
                    tiles.Remove(point);
                    DestroyImmediate(tile.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Create a rectangle within the board limit
    /// </summary>
    /// <returns>The rectangle</returns>
    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, depth);
        int w = UnityEngine.Random.Range(1, width - x + 1);
        int h = UnityEngine.Random.Range(1, depth - y + 1);
        return new Rect(x, y, w, h);
    }
}
