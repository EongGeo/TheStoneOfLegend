using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract int Atk { get; protected set; }
    public abstract int Hp { get; protected set; }
    public abstract float Speed { get; protected set; }
    public LayerMask obstacleMask;
    protected HpBar hpBar;
    protected int maxHp;
    protected void TakeDamage(int str)
    {
        Hp -= str;
        UpdateHpBar();
        if (Hp <= 0) Die();
    }
    protected void UpdateHpBar()
    {
        hpBar.UpdateHp(Hp, maxHp);
    }
    protected void Die()
    {
        Managers.Spawn.EnemyDie();
        Destroy(gameObject);
    }
    protected void OnDestroy()
    {
        if (hpBar != null) Destroy(hpBar.gameObject);
    }
    protected abstract void Attack();
    protected abstract void Move();
    protected bool TryMoveSmart(Vector3 dir)
    {
        Vector3[] evadeDirs = new Vector3[]
        {
            dir,
            Quaternion.Euler(0, 0, 45) * dir,
            Quaternion.Euler(0, 0, -45) * dir,
            Quaternion.Euler(0, 0, 90) * dir,
            Quaternion.Euler(0, 0, -90) * dir
        };

        foreach (var d in evadeDirs)
        {
            if (TryMove(d)) return true;
        }

        return false;
    }
    protected bool TryMove(Vector3 dir)
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col == null)
        {
            transform.position += dir * (Speed * Time.deltaTime);
            return true;
        }

        float radius = col.radius * transform.localScale.x * 0.95f;
        float distance = Speed * Time.deltaTime;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, dir, distance, obstacleMask);

        if (hit.collider == null)
        {
            transform.position += dir * distance;
            return true;
        }
        return false;
    }
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
