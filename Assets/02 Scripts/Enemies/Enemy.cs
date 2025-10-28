using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int Atk {  get; private set; }
    public int Hp { get; private set; }
    public float Speed { get; private set; }

    public void TakeDamage(int str)
    {
        Hp -= str;
    }
    public void Die()
    {
        if (Hp <= 0) Managers.Spawn.EnemyDie();
    }
    protected abstract void Attack();
    protected abstract void Move();
}
