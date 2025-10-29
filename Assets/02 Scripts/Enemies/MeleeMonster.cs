using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : Enemy
{
    public override int Atk { get; protected set; } = 5;
    public override int Hp { get; protected set; } = 40;
    public override float Speed { get; protected set; } = 2.0f;

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
