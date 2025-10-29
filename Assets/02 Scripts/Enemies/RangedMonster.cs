using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : Enemy
{
    [SerializeField] private EnemyProjectile enemySmallPjtPrefab;
    [SerializeField] private EnemyProjectile enemyBigPjtPrefab;
    public override int Atk { get; protected set; } = 3;
    public override int Hp { get; protected set; } = 30;
    public override float Speed { get; protected set; } = 3.0f;
    private void Awake()
    {

    }
    protected override void Move()
    {

    }

    protected override void Attack()
    {

    }
}
