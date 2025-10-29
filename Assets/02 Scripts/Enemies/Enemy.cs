using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract int Atk { get; protected set; }
    public abstract int Hp { get; protected set; }
    public abstract float Speed { get; protected set; }

    public void TakeDamage(int str)
    {
        Hp -= str;
        if (Hp <= 0) Die();
    }
    protected void Die()
    {
        Managers.Spawn.EnemyDie();
        Destroy(gameObject);
    }
    protected abstract void Attack();
    protected abstract void Move();

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            if (collision.gameObject.TryGetComponent<PlayerProjectile>(out PlayerProjectile stone))
            {
                SoundManager.Instance.PlayEnemyHitSFX();
                TakeDamage(stone.Atk);
            }
        }
    }
}
