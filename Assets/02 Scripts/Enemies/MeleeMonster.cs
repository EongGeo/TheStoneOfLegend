using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : Enemy
{
    public override int Atk { get; protected set; } = 5;
    public override int Hp { get; protected set; } = 40;
    public override float Speed { get; protected set; } = 2.0f;

    [SerializeField] private Transform player;
    [SerializeField] private GridBFS grid;
    // 벽만 감지하도록 레이어마스크 추가
    public LayerMask obstacleMask;

    private Vector3Int lastPlayerCell;

    void Awake()
    {
        grid = FindObjectOfType<GridBFS>();
        if (grid == null)
            Debug.LogError("MeleeMonster: GridManager를 씬에서 찾을 수 없습니다!");

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogError("MeleeMonster: Player 태그 오브젝트를 찾을 수 없습니다!");
    }

    void Start()
    {
        if (grid == null || player == null) return;

        // 초기 거리맵 생성
        lastPlayerCell = grid.groundTilemap.WorldToCell(player.position);
        grid.BuildDistanceMap(grid.ToIndex((Vector2Int)lastPlayerCell));

        // 스폰 보정
        Vector2Int myIndex = grid.WorldToIndex(transform.position);
        if (!grid.IsInside(myIndex) || grid.distanceMap[myIndex.x, myIndex.y] == -1)
        {
            Vector3? corrected = grid.GetValidSpawnPosition();
            if (corrected.HasValue) transform.position = corrected.Value;
            else { Destroy(gameObject); return; }
        }
    }

    void Update()
    {
        if (grid == null || player == null || grid.distanceMap == null) return;

        // 플레이어 셀이 바뀌면 거리맵 갱신
        Vector3Int currentPlayerCell = grid.groundTilemap.WorldToCell(player.position);
        if (currentPlayerCell != lastPlayerCell)
        {
            lastPlayerCell = currentPlayerCell;
            grid.BuildDistanceMap(grid.ToIndex((Vector2Int)currentPlayerCell));
        }

        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector3Int cell = grid.groundTilemap.WorldToCell(transform.position);
        Vector2Int pos = grid.ToIndex((Vector2Int)cell);
        if (!grid.IsInside(pos)) return;

        int currentDist = grid.distanceMap[pos.x, pos.y];

        if (currentDist < 0)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            TryMove(dirToPlayer);
            return;
        }

        Vector2Int bestNext = pos;
        int bestDist = currentDist;

        foreach (var dir in new Vector2Int[] {
            new Vector2Int(1,0), new Vector2Int(-1,0),
            new Vector2Int(0,1), new Vector2Int(0,-1)
        })
        {
            Vector2Int next = pos + dir;
            if (grid.IsInside(next))
            {
                int nextDist = grid.distanceMap[next.x, next.y];
                if (nextDist >= 0 && nextDist < bestDist)
                {
                    bestDist = nextDist;
                    bestNext = next;
                }
            }
        }

        if (bestNext != pos && bestDist > 0)
        {
            Vector3 targetWorld = grid.IndexToWorld(bestNext);
            Vector3 dir = (targetWorld - transform.position).normalized;

            if (!TryMove(dir))
            {
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                TryMove(dirToPlayer);
            }
        }
        else
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            TryMove(dirToPlayer);
        }
    }

    bool TryMove(Vector3 dir)
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col == null)
        {
            transform.position += dir * (Speed * Time.deltaTime);
            return true;
        }

        float radius = col.radius * transform.localScale.x * 0.9f;
        float distance = Speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, dir, distance, obstacleMask);

        if (hit.collider == null)
        {
            transform.position += dir * distance;
            return true;
        }
        return false;
    }

    protected override void Move()
    {

    }
    protected override void Attack()
    {

    }
}
