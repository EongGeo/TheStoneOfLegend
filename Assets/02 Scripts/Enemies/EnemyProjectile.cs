using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float lifetime = 2.0f;
    public int atk = 3;

    private Vector2 moveDir;
    private float spawnTime;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);

        if (Time.time - spawnTime >= lifetime)
        {
            ReturnPool();
        }
    }
    private void ReturnPool()
    {
        if (Managers.Pool != null)
        {
            Managers.Pool.ReturnPool(this);
        }
    }
    public void SetDirection(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            ReturnPool();
        }
    }
}
