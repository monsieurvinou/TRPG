using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] int width = 10;
    [SerializeField] int depth = 10;
    [SerializeField] int height = 8;
    [SerializeField] GameObject tileViewPrefab;
    [SerializeField] GameObject tileSelectionIndicatorPrefab;
    [SerializeField] Point pos;
    [SerializeField] LevelData levelData;
    Dictionary<Point, Tile> tiles = new Dictionary<Point, Tile>();
    Transform _marker;

    /// <summary>
    /// Grow the tiles of the entire board
    /// </summary>
    public void GrowAll()
    {
        Rect rectangle = new Rect(0, 0, width, depth);
        GrowRect(rectangle);
    }

    /// <summary>
    /// Shrink the tiles of the entire board
    /// </summary>
    public void ShrinkAll()
    {
        Rect rectangle = new Rect(0, 0, width, depth);
        ShrinkRect(rectangle);
    }

    /// <summary>
    /// Grow the tiles of an area
    /// </summary>
    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }

    /// <summary>
    /// Shrink the tiles of an area
    /// </summary>
    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    /// <summary>
    /// Grow the tile where the attribute "pos" is at
    /// </summary>
    public void Grow()
    {
        GrowSingle(pos);
    }

    /// <summary>
    /// Shrink the tile where the attribute "pos" is at
    /// </summary>
    public void Shrink()
    {
        ShrinkSingle(pos);
    }

    /// <summary>
    /// Update the marker position
    /// </summary>
    public void UpdateMarker()
    {
        Tile tile = tiles.ContainsKey(pos) ? tiles[pos] : null;
        marker.localPosition = tile != null ? tile.center : new Vector3(pos.x, 0, pos.y);
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
        board.tiles = new List<Vector3>(tiles.Count);
        foreach (Tile t in tiles.Values)
            board.tiles.Add(new Vector3(t.pos.x, t.height, t.pos.y));

        string fileName = string.Format("Assets/Resources/Boards/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, fileName);
    }

    /// <summary>
    /// Load a board
    /// </summary>
    public void Load()
    {
        if (levelData != null) {
            // we clear the board
            Clear();

            // we load every tile describe in the level data
            foreach (Vector3 tileVector in levelData.tiles)
            {
                Tile tile = Create();
                tile.Load(tileVector);
                tiles.Add(tile.pos, tile);
            }
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
    /// Grow all the tiles in the rectangle
    /// </summary>
    /// <param name="rect">The rectangle</param>
    void GrowRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    /// <summary>
    /// Shrink all the tiles in the rectangle
    /// </summary>
    /// <param name="rect">The rectangle</param>
    void ShrinkRect(Rect rect)
    {
        for (int y = (int)rect.yMin; y < (int)rect.yMax; ++y)
        {
            for (int x = (int)rect.xMin; x < (int)rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    /// <summary>
    /// Instantiate a tile prefab linked to the script at the attribute "pos" position
    /// </summary>
    /// <returns>The tile instantiated</returns>
    Tile Create()
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
    Tile GetOrCreate(Point p)
    {
        if (tiles.ContainsKey(p))
            return tiles[p];

        Tile t = Create();
        t.Load(p, 0);
        tiles.Add(p, t);

        return t;
    }

    /// <summary>
    /// Grow the tile at the position
    /// </summary>
    /// <param name="p">The position</param>
    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if (t.height < height)
            t.Grow();
    }

    /// <summary>
    /// Shrink the tile at the position
    /// </summary>
    /// <param name="p">The position</param>
    void ShrinkSingle(Point p)
    {
        if (!tiles.ContainsKey(p))
            return;

        Tile t = tiles[p];
        t.Shrink();

        if (t.height <= 0)
        {
            tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    /// <summary>
    /// Lazy getter of the marker
    /// </summary>
    Transform marker
    {
        get
        {
            if (_marker == null)
            {
                GameObject instance = Instantiate(tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
            }
            return _marker;
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
