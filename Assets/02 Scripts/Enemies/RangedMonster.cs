using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : Enemy
{
    [SerializeField] private EnemyProjectile enemySmallPjtPrefab;

    public override int Atk { get; protected set; } = 3;
    public override int Hp { get; protected set; } = 30;
    public override float Speed { get; protected set; } = 3.0f;

    [SerializeField] private Transform player;
    [SerializeField] private GridBFS grid;
    [SerializeField] private HpBar hpBarPrefab;

    private Vector3Int lastPlayerCell;
    private bool canAttack = true;

    private void Awake()
    {
        grid = FindObjectOfType<GridBFS>(); //하나의 distanceMap 공유
        if (grid == null) Debug.LogError("RangedMonster: GridManager를 씬에서 찾을 수 없습니다!");

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogError("RangedMonster: Player 태그 오브젝트를 찾을 수 없습니다!");
    }

    private void Start()
    {
        maxHp = Hp;
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();

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

    private void Update()
    {
        if (grid == null || player == null || grid.distanceMap == null) return;

        // 플레이어 셀이 바뀌면 거리맵 갱신
        Vector3Int currentPlayerCell = grid.groundTilemap.WorldToCell(player.position);
        if (currentPlayerCell != lastPlayerCell)
        {
            lastPlayerCell = currentPlayerCell;
            grid.BuildDistanceMap(grid.ToIndex((Vector2Int)currentPlayerCell));
        }
        Move();
    }
    protected override void Move()
    {
        Vector3Int cell = grid.groundTilemap.WorldToCell(transform.position);
        Vector2Int pos = grid.ToIndex((Vector2Int)cell);
        if (!grid.IsInside(pos)) return;

        int currentDist = grid.distanceMap[pos.x, pos.y];
        if (currentDist < 0)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            TryMoveSmart(dirToPlayer);
            return;
        }

        int targetDist = 5;
        Vector2Int bestNext = pos;
        int bestDist = currentDist;

        foreach (var dir in new Vector2Int[] {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right,
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1)
        })
        {
            Vector2Int next = pos + dir;
            if (!grid.IsInside(next)) continue;

            int nextDist = grid.distanceMap[next.x, next.y];
            if (nextDist < 0) continue;

            if (currentDist > targetDist)
            {
                if (nextDist < bestDist)
                {
                    bestDist = nextDist;
                    bestNext = next;
                }
            }
            else if (currentDist < targetDist)
            {
                if (nextDist > bestDist)
                {
                    bestDist = nextDist;
                    bestNext = next;
                }
            }
        }

        if (bestNext != pos && bestDist != currentDist)
        {
            Vector3 targetWorld = grid.IndexToWorld(bestNext);
            Vector3 dir = (targetWorld - transform.position).normalized;

            if (!TryMoveSmart(dir))
            {
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                TryMoveSmart(dirToPlayer);
            }
        }
        else
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            TryMoveSmart(dirToPlayer);
        }

        if (currentDist == targetDist)
        {
            Attack();
        }
    }
    protected override void Attack()
    {
        if(canAttack) StartCoroutine(ShootToPlayer());
    }
    private IEnumerator ShootToPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;

        EnemyProjectile small = Managers.Pool.GetFromPool(enemySmallPjtPrefab);
        small.SetDirection(dir);
        small.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        canAttack = false;
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
    }
}
