using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using Algorithms;

public enum TileMapLayersEnum { Ground, OneWay, Background, Foreground }
public class GameGrid : MonoBehaviour
{
    public static GameGrid instance;
    public Grid grid;
    public Tilemap[] tilemaps;
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
        if(tilemaps[0] != null)
        {
            Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
            Gizmos.DrawWireCube(tilemaps[0].origin+ tilemaps[0].size/2, tilemaps[0].size);
        }

    }

    public void ResizeMaps()
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap == null)
                continue;

            tilemap.origin = new Vector3Int(0, 0, 0);
            tilemap.size = new Vector3Int(mapSizeX, mapSizeY, 1);
            tilemap.ResizeBounds();

        }
    }

    public List<WorldTile> GetWorldTiles(int mapID)
    {
        List<WorldTile> tiles = new List<WorldTile>();



        foreach (Vector3Int pos in tilemaps[mapID].cellBounds.allPositionsWithin)
        {
            var lPos = new Vector3Int(pos.x, pos.y, pos.z);

            if (!tilemaps[mapID].HasTile(lPos))
            {
                continue;
            }

            WorldTile _tile = new WorldTile()
            {
                LocalPlace = lPos,
                TileID = tilemaps[mapID].GetTile(lPos).name,
            };

            tiles.Add(_tile);

        }

        return tiles;
    }

    public void SetWorldTiles(int mapID, List<WorldTile> tiles, bool clearMap = true)
    {
        if (clearMap)
        {
            tilemaps[mapID].ClearAllTiles();
        }

        Tile[] tileAsset = Resources.LoadAll<Tile>("Tiles");

        foreach (WorldTile tile in tiles)
        {

            for (int i = 0; i < tileAsset.Length; i++)
            {
                if (tileAsset[i].name == tile.TileID)
                {
                    tilemaps[mapID].SetTile(tile.LocalPlace, tileAsset[i]);
                    i = tileAsset.Length;
                }
            }
        }
        Resources.UnloadUnusedAssets();
    }

    //This method is not currently being used.
    public void SetRoom(RoomData room, int x, int y)
    {
        Tile[] tileAsset = Resources.LoadAll<Tile>("Tiles");

        foreach (TilemapLayerSaveData layerData in room.mapLayers)
        {
            foreach (WorldTile tile in layerData.tiles)
            {

                for (int i = 0; i < tileAsset.Length; i++)
                {
                    if (tileAsset[i].name == tile.TileID)
                    {
                        //Careful, these 10s should not be hardcoded
                        Vector3Int tileMapPos = new Vector3Int(tile.LocalPlace.x + GambleConstants.RoomSizeX * x, tile.LocalPlace.y + GambleConstants.RoomSizeY * y, tile.LocalPlace.z);
                        tilemaps[layerData.layerIndex].SetTile(tileMapPos, tileAsset[i]);
                        i = tileAsset.Length;
                    }
                }
            }
        }

        Resources.UnloadUnusedAssets();

    }

    public void SetMap(MapData data)
    {
        ClearTiles();

        mapSizeX = data.mapSizeX;
        mapSizeY = data.mapSizeY;
        ResizeMaps();
        foreach (RoomData room in data.rooms)
        {
            foreach (TilemapLayerSaveData layerData in room.mapLayers)
            {
                SetWorldTiles(layerData.layerIndex, layerData.tiles, false);
            }
        }

        currentMap = data;
        InitPathFinder();
    }

    public void ClearTiles()
    {
        for (int i = 0; i < tilemaps.Length; i++)
        {
            tilemaps[i].ClearAllTiles();
        }
    }

    public bool IsGround(int x, int y)
    {
        return tilemaps[(int)TileMapLayersEnum.OneWay].HasTile(new Vector3Int(x, y, 0)) || tilemaps[(int)TileMapLayersEnum.Ground].HasTile(new Vector3Int(x, y, 0));
    }

    public bool IsOneWayPlatform(int x, int y)
    {
        return tilemaps[(int)TileMapLayersEnum.OneWay].HasTile(new Vector3Int(x, y, 0));
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
}
