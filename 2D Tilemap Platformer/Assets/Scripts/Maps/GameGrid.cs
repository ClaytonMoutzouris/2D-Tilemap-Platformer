using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using Algorithms;
using Newtonsoft.Json;

public enum TileMapLayersEnum { Ground, OneWay, Background, Foreground, Ladders, Spikes, Objects }
public class GameGrid : MonoBehaviour
{
    public static GameGrid instance;
    public Grid grid;
    public Tilemap[] tilemapLayers;
    public int mapSizeX = 10;
    public int mapSizeY = 10;
    public PathFinderFast mPathFinder;
    public byte[,] mGrid;

    public MapData currentMap;

    private void Start()
    {
        instance = this;
        InitPathFinder();
    }

    void OnDrawGizmos()
    {
        // Green
        if(tilemapLayers[0] != null)
        {
            Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
            Gizmos.DrawWireCube((Vector3)tilemapLayers[0].origin + (Vector3)tilemapLayers[0].size/2.0f, (Vector3)tilemapLayers[0].size);
        }

    }

    public void ResizeMaps()
    {
        foreach (Tilemap tilemap in tilemapLayers)
        {
            if (tilemap == null)
                continue;

            tilemap.origin = new Vector3Int(0, 0, 0);
            tilemap.size = new Vector3Int(mapSizeX, mapSizeY, 1);
            tilemap.ResizeBounds();

        }
    }

    public List<WorldTile> GetWorldTiles(int layerID)
    {
        List<WorldTile> tiles = new List<WorldTile>();



        foreach (Vector3Int pos in tilemapLayers[layerID].cellBounds.allPositionsWithin)
        {
            var lPos = new Vector3Int(pos.x, pos.y, pos.z);

            if (!tilemapLayers[layerID].HasTile(lPos))
            {
                continue;
            }

            TileBase tilebase = tilemapLayers[layerID].GetTile(lPos);
            WorldTile _tile;


            _tile = new WorldTile()
            {
                LocalPlace = lPos,
                TileID = tilebase.name,
                LayerID = (TileMapLayersEnum)layerID,
                SpawnObject = null,

            };

            if(tilebase is GambleObjectTileBase gTile)
            {
                if(gTile.spawnObject != null)
                {
                    _tile.SpawnObject = gTile.spawnObject.name;

                }
            }

            tiles.Add(_tile);

        }

        return tiles;
    }

    public List<WorldTile> GetWorldTiles()
    {
        List<WorldTile> tiles = new List<WorldTile>();

        for (int i = 0; i < tilemapLayers.Length; i++)
        {
            foreach (Vector3Int pos in tilemapLayers[i].cellBounds.allPositionsWithin)
            {
                var lPos = new Vector3Int(pos.x, pos.y, pos.z);

                if (!tilemapLayers[i].HasTile(lPos))
                {
                    continue;
                }

                TileBase tilebase = tilemapLayers[i].GetTile(lPos);


                WorldTile _tile = new WorldTile()
                {
                    LocalPlace = lPos,
                    TileID = tilebase.name,
                    LayerID = (TileMapLayersEnum)i,
                    SpawnObject = null,

                };

                //if its not what is it then?
                if (tilebase is GambleObjectTileBase gTile)
                {
                    if(gTile.spawnObject != null)
                    {
                        Debug.Log("Found an object tile");
                        _tile.SpawnObject = gTile.spawnObject.name;
                    }
                }

                tiles.Add(_tile);

            }
        }

        return tiles;
    }

    public void SetWorldTiles(int mapID, List<WorldTile> tiles, bool clearMap = true, bool editor = false)
    {
        if (clearMap)
        {
            tilemapLayers[mapID].ClearAllTiles();
        }

        Tile[] tileAsset = Resources.LoadAll<Tile>("Tiles");

        foreach (WorldTile tile in tiles)
        {
            for (int i = 0; i < tileAsset.Length; i++)
            {
                if (tileAsset[i].name == tile.TileID)
                {
                    if (tile.SpawnObject != null && !editor)
                    {
                        GameObject temp = Instantiate(Resources.Load("Prefabs/Entities/" + tile.SpawnObject) as GameObject, new Vector3(tile.LocalPlace.x, tile.LocalPlace.y, 0), Quaternion.identity);
                    } else
                    {
                        tilemapLayers[mapID].SetTile(tile.LocalPlace, tileAsset[i]);
                    }

                    i = tileAsset.Length;
                }
            }
        }
        Resources.UnloadUnusedAssets();
    }

    public void SetWorldTiles(List<WorldTile> tiles, bool clearMap = true, bool editor = false)
    {
        if (clearMap)
        {
            ClearMap();
        }

        Tile[] tileAsset = Resources.LoadAll<Tile>("Tiles");

        foreach (WorldTile tile in tiles)
        {
            for (int i = 0; i < tileAsset.Length; i++)
            {
                if (tileAsset[i].name == tile.TileID)
                {
                    //Debug.Log("Spawn object for tile " + tile.SpawnObject);

                    if (tile.SpawnObject != null && !editor)
                    {
                        //Debug.Log("Spawn object loaded");
                        GameObject temp = Instantiate(Resources.Load("Prefabs/Entities/" + tile.SpawnObject) as GameObject, new Vector3(tile.LocalPlace.x+ 0.5f, tile.LocalPlace.y + 0.5f, 0), Quaternion.identity);
                    }
                    else
                    {
                        tilemapLayers[(int)tile.LayerID].SetTile(tile.LocalPlace, tileAsset[i]);
                    }

                    i = tileAsset.Length;
                }
            }
        }
        Resources.UnloadUnusedAssets();
    }

    public void ClearMap()
    {
        foreach(Tilemap tilemap in tilemapLayers)
        {
            tilemap.ClearAllTiles();
        }
    }

    //This method is not currently being used.
    public void SetRoom(RoomData room, int x, int y, bool editor = false)
    {
        Tile[] tileAsset = Resources.LoadAll<Tile>("Tiles");

        foreach (WorldTile tile in room.tiles)
        {
            for (int i = 0; i < tileAsset.Length; i++)
            {
                if (tileAsset[i].name == tile.TileID)
                {
                    Vector3Int tileMapPos = new Vector3Int(tile.LocalPlace.x + GambleConstants.RoomSizeX * x, tile.LocalPlace.y + GambleConstants.RoomSizeY * y, tile.LocalPlace.z);

                    if (tile.SpawnObject != null && !editor)
                    {
                        Debug.Log("Spawn object for tile " + tile.SpawnObject);

                        //Debug.Log("Spawn object loaded");
                        GameObject temp = Instantiate(Resources.Load("Prefabs/Entities/" + tile.SpawnObject) as GameObject, new Vector3(tileMapPos.x + 0.5f, tileMapPos.y + 0.5f, 0), Quaternion.identity);
                    }
                    else
                    {
                        tilemapLayers[(int)tile.LayerID].SetTile(tileMapPos, tileAsset[i]);
                    }

                    i = tileAsset.Length;
                }
            }
        }

        Resources.UnloadUnusedAssets();

    }

    public void ClearTiles()
    {
        for (int i = 0; i < tilemapLayers.Length; i++)
        {
            tilemapLayers[i].ClearAllTiles();
        }
    }

    public bool IsGround(int x, int y)
    {
        return tilemapLayers[(int)TileMapLayersEnum.OneWay].HasTile(new Vector3Int(x, y, 0)) || tilemapLayers[(int)TileMapLayersEnum.Ground].HasTile(new Vector3Int(x, y, 0));
    }

    public bool IsOneWayPlatform(int x, int y)
    {
        return tilemapLayers[(int)TileMapLayersEnum.OneWay].HasTile(new Vector3Int(x, y, 0));
    }

    public void InitPathFinder()
    {
        SetGrid();

        mPathFinder = new PathFinderFast(mGrid, this);

        mPathFinder.Formula = HeuristicFormula.Manhattan;
        //if false then diagonal movement will be prohibited
        mPathFinder.Diagonals = false;
        //if true then diagonal movement will have higher cost
        mPathFinder.HeavyDiagonals = false;
        //estimate of path length
        mPathFinder.HeuristicEstimate = 6;
        mPathFinder.PunishChangeDirection = false;
        mPathFinder.TieBreaker = false;
        mPathFinder.SearchLimit = 10000;
        mPathFinder.DebugProgress = false;
        mPathFinder.DebugFoundPath = false;
    }

    public void SetGrid()
    {
        mGrid = new byte[Mathf.NextPowerOfTwo(mapSizeX * GambleConstants.RoomSizeX), Mathf.NextPowerOfTwo(mapSizeY * GambleConstants.RoomSizeY)];

        for(int x = 0; x < mapSizeX*GambleConstants.RoomSizeX; x++)
        {
            for (int y = 0; y < mapSizeY * GambleConstants.RoomSizeY; y++)
            {
                mGrid[x, y] = (byte)(IsGround(x,y) ? 0 : 1);
            }
        }
    }

    public bool AnySolidBlockInRectangle(Vector2Int start, Vector2Int end)
    {
        int startX, startY, endX, endY;

        if (start.x <= end.x)
        {
            startX = start.x;
            endX = end.x;
        }
        else
        {
            startX = end.x;
            endX = start.x;
        }

        if (start.y <= end.y)
        {
            startY = start.y;
            endY = end.y;
        }
        else
        {
            startY = end.y;
            endY = start.y;
        }

        for (int y = startY; y <= endY; ++y)
        {
            for (int x = startX; x <= endX; ++x)
            {
                if (IsGround(x,y))
                    return true;
            }
        }

        return false;
    }

    public bool AnySolidBlockInStripe(int x, int y0, int y1)
    {
        int startY, endY;

        if (y0 <= y1)
        {
            startY = y0;
            endY = y1;
        }
        else
        {
            startY = y1;
            endY = y0;
        }

        for (int y = startY; y <= endY; ++y)
        {
            if (IsGround(x, y))
                return true;
        }

        return false;
    }

    public Vector2 GetMapSize()
    {
        Vector2 gridBounds = Vector2.zero;

        foreach(Tilemap map in tilemapLayers)
        {
            int x = map.size.x;
            int y = map.size.y;

            if(x > gridBounds.x)
            {
                gridBounds.x = x;
            }

            if (y > gridBounds.y)
            {
                gridBounds.y = y;
            }
        }

        return gridBounds;
    }

    public void LoadMap(string path)
    {

        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);

            RoomData loadData = JsonConvert.DeserializeObject<RoomData>(loadJson);

            SetWorldTiles(loadData.tiles, true);

            Debug.Log("Loaded Grid bounds: " + GetMapSize());

            Vector2 bounds = GetMapSize();
            mapSizeX = (int)bounds.x;
            mapSizeY = (int)bounds.y;

            InitPathFinder();

        }
        else
        {
            Debug.LogError("Map not found.");
        }
    }

    public void SetMap(MapData data, bool editor = false)
    {
        ClearMap();

        mapSizeX = data.mapSizeX;
        mapSizeY = data.mapSizeY;
        ResizeMaps();

        for (int x = 0; x < data.numRoomsX; x++)
        {
            for (int y = 0; y < data.numRoomsY; y++)
            {
                SetRoom(data.rooms[x, y], x, y, editor);
            }
        }


        currentMap = data;
        InitPathFinder();
    }
}
