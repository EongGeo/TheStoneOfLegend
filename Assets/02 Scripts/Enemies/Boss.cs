using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private EnemyProjectile enemySmallPjtPrefab;
    [SerializeField] private EnemyProjectile enemyBigPjtPrefab;
    public override int Atk { get; protected set; } = 10;
    public override int Hp { get; protected set; } = 200;
    public override float Speed { get; protected set; } = 0.2f;
    private void Awake()
    {

    }
    private void Start()
    {
        Managers.Pool.CreatePool(enemySmallPjtPrefab, 30);
        Managers.Pool.CreatePool(enemyBigPjtPrefab, 10);
    }
    protected override void Move()
    {

    }

    protected override void Attack()
    {

    }
}
