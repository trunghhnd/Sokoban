using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class LevelLoader
{
    public static void Load(int levelIndex,
        Tilemap groundMap, Tilemap objectMap,
        TileBase wallTile, TileBase floorTile,
        TileBase targetTile, TileBase boxTile,
        GameObject player)
    {
        TextAsset file = Resources.Load<TextAsset>("Levels/Level" + levelIndex);
        if (file == null)
        {
            return;
        }

        groundMap.ClearAllTiles();
        objectMap.ClearAllTiles();

        string[] rows = file.text.Replace("\r", "").Split('\n');
        int height = rows.Length;
        int width = 0;
        foreach (var row in rows)
            width = Mathf.Max(width, row.Length);
        int offsetX = -Mathf.RoundToInt((width - 1) / 2f);
        int offsetY = -Mathf.RoundToInt((height - 1) / 2f);

        for (int r = 0; r < height; r++)
        {
            int y = height - 1 - r;
            for (int c = 0; c < rows[r].Length; c++)
            {
                char ch = rows[r][c];
                if (ch == ' ') continue;

                var cell = new Vector3Int(c + offsetX, y + offsetY, 0);

                switch (ch)
                {
                    case '#':
                        groundMap.SetTile(cell, wallTile);
                        break;
                    case '-':
                        groundMap.SetTile(cell, floorTile);
                        break;
                    case '.':
                        groundMap.SetTile(cell, targetTile);
                        break;
                    case '$':
                        groundMap.SetTile(cell, floorTile);
                        objectMap.SetTile(cell, boxTile);
                        break;
                    case '*':
                        groundMap.SetTile(cell, targetTile);
                        objectMap.SetTile(cell, boxTile);
                        break;
                    case '@':
                        groundMap.SetTile(cell, floorTile);
                        player.transform.position = groundMap.GetCellCenterWorld(cell);
                        break;
                }
            }
        }
    }
}