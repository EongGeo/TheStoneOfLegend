using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerProjectile stonePrefab;

    public int Hp { get; private set; }
    public float Speed { get; private set; }

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float inputX, inputY;
    private float throwingCooltime = 0.3f;
    private bool canThrowing = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyPlayerData(Managers.Game.playerData);
    }
    private void Start()
    {
        Managers.Pool.CreatePool(stonePrefab, 10);
    }

    private void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if (inputX != 0)
        {
            if (inputX < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && canThrowing)
        {
            StartCoroutine(StoneThrowing());
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputX * Speed, inputY * Speed);
    }
    private IEnumerator StoneThrowing()
    {
        SoundManager.Instance.PlayStoneThrowingSFX();

        PlayerProjectile stone = Managers.Pool.GetFromPool(stonePrefab);
        stone.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        canThrowing = false;
        yield return new WaitForSeconds(throwingCooltime);
        canThrowing = true;
    }
    private void ApplyPlayerData(PlayerData data)
    {
        Hp = data.playerMaxHp;
        Speed = data.playerSpeed;
    }
    private void TakeDamage(int atk)
    {
        Hp -= atk;
        if (Hp <= 0) Die();
    }
    private void Die()
    {
        Managers.Stage.GameOver();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                TakeDamage(enemy.Atk);
            }
            if (collision.gameObject.TryGetComponent<EnemyProjectile>(out EnemyProjectile enemyProjectile))
            {
                TakeDamage(enemyProjectile.Atk);
            }
        }
    }
}
