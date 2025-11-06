using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public override int Atk { get; protected set; } = 10;
    public override int Hp { get; protected set; } = 200;
    public override float Speed { get; protected set; } = 0.2f;

    [SerializeField] private EnemyProjectile enemySmallPjtPrefab;
    [SerializeField] private EnemyProjectile enemyBigPjtPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private GridBFS grid;
    [SerializeField] private HpBar hpBarPrefab;

    private Vector3Int lastPlayerCell;
    private SpriteRenderer sr;
    private bool canAllRoundAttack = true;
    private bool canLinearAttack = false;
    private bool accelerationMode = false;

    private void Awake()
    {
        grid = FindObjectOfType<GridBFS>(); //하나의 distanceMap 공유
        if (grid == null) Debug.LogError("Boss: GridManager를 씬에서 찾을 수 없습니다!");

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogError("Boss: Player 태그 오브젝트를 찾을 수 없습니다!");

        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        maxHp = Hp;
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();

        Managers.Pool.CreatePool(enemyBigPjtPrefab, 40);
        Managers.Pool.CreatePool(enemySmallPjtPrefab, 60);

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
        Attack();
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
    }
    protected override void Attack()
    {
        if (canAllRoundAttack) StartCoroutine(AllRoundAttack());
        if (canLinearAttack) StartCoroutine(LinearAttack());
        if (accelerationMode) StartCoroutine(AccelerationModeOn());
    }
    private IEnumerator AllRoundAttack() 
    {
        canAllRoundAttack = false;
        float angleStep = 18f;

        for (int i = 0; i < 20; i++)
        {
            float angle = i * angleStep;
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

            EnemyProjectile big = Managers.Pool.GetFromPool(enemyBigPjtPrefab);
            big.SetDirection(dir);
            big.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(5.0f);
        canLinearAttack = true;
    }
    private IEnumerator LinearAttack() 
    {
        canLinearAttack = false;
        for (int i = 0; i < 4; i++)
        {
            for (int k = 0; k < 5; k++)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                EnemyProjectile small = Managers.Pool.GetFromPool(enemySmallPjtPrefab);
                small.SetDirection(dir);
                small.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2.5f);
        accelerationMode = true;
    }
    private IEnumerator AccelerationModeOn() 
    {
        accelerationMode = false;
        sr.color = Color.red;
        Speed *= 20f;
        yield return new WaitForSeconds(3.0f);
        sr.color = Color.white;
        Speed /= 20f;
        canAllRoundAttack = true;
    }
}
