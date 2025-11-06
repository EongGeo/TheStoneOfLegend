using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBFS : MonoBehaviour
{
    [Header("바닥맵")]
    public Tilemap groundTilemap;

    [Header("거리맵")]
    public int[,] distanceMap; //BFS로 계산된 거리 정보를 저장하는 2차원 배열

    [Header("디버그용")]
    public TileBase debugTile;
    public Tilemap debugTilemap;

    private Vector2Int offset; //타일맵 cellBounds의 최소 좌표 → 배열 인덱스 보정용

    [SerializeField] private Transform player;

    private void Awake()
    {
        InitializeGrid();
    }
    
    public void InitializeGrid() //타일맵의 cellBounds를 기준으로 distanceMap 배열 초기화
    {
        if (groundTilemap == null)
        {
            return;
        }

        BoundsInt bounds = groundTilemap.cellBounds;
        offset = new Vector2Int(bounds.xMin, bounds.yMin);
        distanceMap = new int[bounds.size.x, bounds.size.y];
    }

    public Vector2Int WorldToIndex(Vector3 worldPos) //월드 좌표 → distanceMap 인덱스로 변환
    {
        Vector3Int cell = groundTilemap.WorldToCell(worldPos);
        return new Vector2Int(cell.x - offset.x, cell.y - offset.y);
    }

    public Vector2Int ToIndex(Vector2Int cell) //타일맵 셀 좌표 → distanceMap 인덱스로 변환
    {
        return new Vector2Int(cell.x - offset.x, cell.y - offset.y);
    }

    public Vector3 IndexToWorld(Vector2Int index) //distanceMap 인덱스 → 월드 좌표로 변환
    {
        Vector3Int cell = new Vector3Int(index.x + offset.x, index.y + offset.y, 0);
        return groundTilemap.GetCellCenterWorld(cell);
    }

    public bool IsInside(Vector2Int index) //distanceMap 배열 범위 안에 있는지 확인
    {
        return index.x >= 0 && index.x < distanceMap.GetLength(0) &&
               index.y >= 0 && index.y < distanceMap.GetLength(1);
    }

    public bool IsWalkable(Vector2Int index) //해당 인덱스가 실제로 타일이 깔린(이동 가능한) 칸인지 확인
    {
        Vector3Int cell = new Vector3Int(index.x + offset.x, index.y + offset.y, 0);
        return groundTilemap.HasTile(cell);
    }

    public void BuildDistanceMap(Vector2Int startCell) //BFS로 거리맵(distanceMap) 생성(BFS 알고리즘으로 모든 칸의 거리를 계산)
    {
        int width = distanceMap.GetLength(0);
        int height = distanceMap.GetLength(1);

        //모든 칸의 거리값을 -1로 초기화
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                distanceMap[x, y] = -1;

        if (!IsInside(startCell) || !IsWalkable(startCell)) return; //시작점이 유효하지 않으면 종료

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        distanceMap[startCell.x, startCell.y] = 0;
        q.Enqueue(startCell);

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right,
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1) };

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
        //VisualizeDistanceMap();
    }

    public Vector3? GetValidSpawnPosition() //distanceMap 상에서 유효한 스폰 위치를 반환
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

    private void VisualizeDistanceMap() //distanceMap을 타일맵에 색깔로 표시 (디버그용)

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
                if (dist < 0) continue;

                // distanceMap 인덱스를 다시 월드 좌표로 변환
                Vector3Int cell = new Vector3Int(x + offset.x, y + offset.y, 0);

                // 디버그 타일 칠하기
                debugTilemap.SetTile(cell, debugTile);

                // 거리값에 따라 색상 보정 (최소 초록 ~ 최대 빨강)
                float t = Mathf.Clamp01(dist / 20f); // 20칸 이상은 최댓값으로
                Color c = Color.Lerp(Color.green, Color.red, t);
                debugTilemap.SetTileFlags(cell, TileFlags.None);
                debugTilemap.SetColor(cell, c);
            }
        }
    }
}
