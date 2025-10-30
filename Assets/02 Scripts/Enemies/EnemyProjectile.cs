using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float lifetime = 2.0f;

    private Vector2 moveDir;
    private float spawnTime;
    public int Atk { get; private set; }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Wall"))
        {
            ReturnPool();
        }
    }
}
