using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerProjectile stonePrefab;
    [SerializeField] private float pushedForce = 7.0f;
    [SerializeField] private HpBar hpBarPrefab;

    public int Hp { get; private set; }
    public float Speed { get; private set; }

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private HpBar hpBar;

    private float inputX, inputY;
    private float throwingCooltime = 0.375f;
    private bool canThrowing = true;
    private bool isPushed = false;
    private Vector2 lastMoveDir = Vector2.up;
    private Vector2 inputXY;
    private int maxHp;

    private static readonly int moveXHash = Animator.StringToHash("MoveX");
    private static readonly int moveYHash = Animator.StringToHash("MoveY");
    private static readonly int isWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int attackTriggerHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        ApplyPlayerData(Managers.Game.playerData);
    }
    private void Start()
    {
        Managers.Pool.CreatePool(stonePrefab, 20);
        maxHp = Hp;
        hpBar = Instantiate(hpBarPrefab);
        hpBar.Init(transform);
        UpdateHpBar();
    }
    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        inputXY = new Vector2(inputX, inputY);

        if (inputXY != Vector2.zero)
        {
            lastMoveDir = inputXY.normalized;

            if (inputX < 0) spriteRenderer.flipX = true;
            else if (inputX > 0) spriteRenderer.flipX = false;
        }

        anim.SetFloat(moveXHash, lastMoveDir.x);
        anim.SetFloat(moveYHash, lastMoveDir.y);
        anim.SetBool(isWalkingHash, inputXY != Vector2.zero);

        if (Input.GetKeyDown(KeyCode.Space) && canThrowing)
        {
            anim.SetTrigger(attackTriggerHash);
            StartCoroutine(StoneThrowing());
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (!isPushed) rb.velocity = inputXY.normalized * Speed;
    }
    private IEnumerator StoneThrowing()
    {
        SoundManager.Instance.PlayStoneThrowingSFX();

        PlayerProjectile stone = Managers.Pool.GetFromPool(stonePrefab);
        stone.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        Vector2 throwDir = new Vector2(inputX, inputY);
        if (throwDir == Vector2.zero) throwDir = lastMoveDir; 

        stone.SetDirection(throwDir);

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
        UpdateHpBar();
        if (Hp <= 0) Die();
    }
    private void UpdateHpBar()
    {
        hpBar.UpdateHp(Hp, maxHp);
    }
    private void Die()
    {
        Managers.Stage.GameOver();
    }
    private void OnDestroy()
    {
        if (hpBar != null) Destroy(hpBar.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                SoundManager.Instance.PlayPlayerHitSFX();
                TakeDamage(enemy.Atk);
                Vector2 pushDir = (transform.position - collision.transform.position).normalized;
                StartCoroutine(PushPlayer(pushDir));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyPjt"))
        {
            if (collision.gameObject.TryGetComponent<EnemyProjectile>(out EnemyProjectile enemyProjectile))
            {
                SoundManager.Instance.PlayPlayerHitSFX();
                TakeDamage(enemyProjectile.atk);
            }
        }
    }
    private IEnumerator PushPlayer(Vector2 dir)
    {
        isPushed = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * pushedForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);
        isPushed = false;
    }

}
