using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MyTilemapProvider : TileProvider
{
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    private static int width;
    private static int height;
    private BoundsInt groundBound;

    public MyTilemapProvider(BoundsInt bounds, Tilemap ground, Tilemap walls) : base(width, height)
    {
        groundTilemap = ground;
        wallTilemap = walls;
        groundBound = bounds;
    }

    public override bool TileInBounds(int x, int y)
    {
        // Check if x is within the bounds of the ground tilemap
        bool inXBounds = x >= groundBound.x && x < groundBound.x + groundBound.size.x;

        // Check if y is within the vertical bounds of the tilemap
        bool inYBounds = y >= groundBound.y && y < groundBound.y + groundBound.size.y;

        return inXBounds && inYBounds;
    }

    public override bool IsTileWalkable(int x, int y)
    {
        // Convert grid coordinates (x, y) to world position
        Vector3Int tilePosition = new Vector3Int(x, y, 0);

        // Check if tile exists in the bounds
        if (!TileInBounds(x, y))
        {
            return false;
        }

        // Check if the tile is walkable (on ground but not on walls)
        bool isGround = groundTilemap.HasTile(tilePosition);
        bool isWall = wallTilemap.HasTile(tilePosition);

        // Walkable if it's on the ground and not a wall
        return isGround && !isWall;
    }
}
