using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBFS : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap groundTilemap; // 바닥
    public Tilemap[] wallTilemaps;// 벽

    [Header("Distance Map")]
    public int[,] distanceMap;

    [Header("Debug Tiles")]
    public TileBase debugTile;
    public Tilemap debugTilemap; // Ground와 겹쳐서 표시될 Tilemap

    private Vector2Int offset;

    [SerializeField] private Transform player;

    void Awake()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        if (groundTilemap == null)
        {
            Debug.LogError("GridManager: groundTilemap이 비어있습니다.");
            return;
        }

        BoundsInt bounds = groundTilemap.cellBounds;
        offset = new Vector2Int(bounds.xMin, bounds.yMin);
        distanceMap = new int[bounds.size.x, bounds.size.y];

        Debug.Log($"[GridManager] Initialized offset={offset}, size={bounds.size}");
    }

    public Vector2Int WorldToIndex(Vector3 worldPos)
    {
        Vector3Int cell = groundTilemap.WorldToCell(worldPos);
        return new Vector2Int(cell.x - offset.x, cell.y - offset.y);
    }

    public Vector2Int ToIndex(Vector2Int cell)
    {
        return new Vector2Int(cell.x - offset.x, cell.y - offset.y);
    }

    public Vector3 IndexToWorld(Vector2Int index)
    {
        Vector3Int cell = new Vector3Int(index.x + offset.x, index.y + offset.y, 0);
        return groundTilemap.GetCellCenterWorld(cell);
    }

    public bool IsInside(Vector2Int index)
    {
        return index.x >= 0 && index.x < distanceMap.GetLength(0) &&
               index.y >= 0 && index.y < distanceMap.GetLength(1);
    }

    public bool IsWalkable(Vector2Int index)
    {
        Vector3Int cell = new Vector3Int(index.x + offset.x, index.y + offset.y, 0);
        return groundTilemap.HasTile(cell);
    }

    public void BuildDistanceMap(Vector2Int startCell)
    {
        int width = distanceMap.GetLength(0);
        int height = distanceMap.GetLength(1);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                distanceMap[x, y] = -1;

        if (!IsInside(startCell) || !IsWalkable(startCell)) return;

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        distanceMap[startCell.x, startCell.y] = 0;
        q.Enqueue(startCell);

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            int curDist = distanceMap[cur.x, cur.y];

            foreach (var dir in dirs)
            {
                Vector2Int next = cur + dir;
                if (!IsInside(next)) continue;
                if (distanceMap[next.x, next.y] != -1) continue;

                if (IsWalkable(next))
                {
                    distanceMap[next.x, next.y] = curDist + 1;
                    q.Enqueue(next);
                }
            }
        }
        VisualizeDistanceMap();
    }

    public Vector3? GetValidSpawnPosition()
    {
        Vector2Int index = WorldToIndex(transform.position);
        if (!IsInside(index)) return null;
        if (distanceMap[index.x, index.y] >= 0)
        {
            Vector3Int cell = new Vector3Int(index.x + offset.x, index.y + offset.y, 0);
            return groundTilemap.GetCellCenterWorld(cell);
        }
        return null;
    }


    public void VisualizeDistanceMap()
    {
        if (debugTilemap == null || distanceMap == null) return;

        debugTilemap.ClearAllTiles();

        int width = distanceMap.GetLength(0);
        int height = distanceMap.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int dist = distanceMap[x, y];
                if (dist < 0) continue; // 방문 안 된 칸은 스킵

                // distanceMap 인덱스를 다시 월드 좌표로 변환
                Vector3Int cell = new Vector3Int(x + offset.x, y + offset.y, 0);

                // 디버그 타일 칠하기
                debugTilemap.SetTile(cell, debugTile);

                // 색상 보정 (거리값에 따라 점점 어두워지게)
                float t = Mathf.Clamp01(dist / 20f); // 20칸 이상은 최댓값으로
                Color c = Color.Lerp(Color.green, Color.red, t);
                debugTilemap.SetTileFlags(cell, TileFlags.None);
                debugTilemap.SetColor(cell, c);
            }
        }
    }
}
