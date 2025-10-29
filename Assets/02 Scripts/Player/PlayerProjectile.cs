using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float lifetime = 1.5f;

    private float spawnTime;
    public int Atk { get; private set; }

    private void OnEnable()
    {
        Atk = Managers.Game.playerData.playerStr;
        spawnTime = Time.time;
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (Time.time - spawnTime >= lifetime)
        {
            ReturnPool();
        }
    }
    void ReturnPool()
    {
        if (Managers.Pool != null)
        {
            Managers.Pool.ReturnPool(this);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            ReturnPool();
        }
    }
}
