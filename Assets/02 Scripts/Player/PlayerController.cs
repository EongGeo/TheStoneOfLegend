using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerProjectile stonePrefab;
    [SerializeField] private float pushedForce = 7.0f;

    public int Hp { get; private set; }
    public float Speed { get; private set; }

    public enum State { Idle, Attack, Walk}
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private StateMachine stateMachine;
    private Animator anim;

    private float inputX, inputY;
    private float throwingCooltime = 0.3f;
    private bool canThrowing = true;
    private bool isPushed = false;
    private Vector2 lastMoveDir = Vector2.up;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateMachine = gameObject.AddComponent<StateMachine>();
        anim = GetComponent<Animator>();

        ApplyPlayerData(Managers.Game.playerData);

        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));
        stateMachine.AddState(State.Walk, new WalkState(this));

        stateMachine.InitState(State.Idle);
    }
    private void Start()
    {
        Managers.Pool.CreatePool(stonePrefab, 10);
    }
    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        Vector2 inputDir = new Vector2(inputX, inputY);

        if (inputDir != Vector2.zero)
        {
            lastMoveDir = inputDir.normalized;

            if (inputX < 0) spriteRenderer.flipX = true;
            else if (inputX > 0) spriteRenderer.flipX = false;
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private string GetDirectionAnim(string baseName)
    {
        if (Mathf.Abs(lastMoveDir.y) > Mathf.Abs(lastMoveDir.x))
        {
            return lastMoveDir.y > 0 ? $"Up{baseName}" : $"Down{baseName}";
        }
        else
        {
            return $"Side{baseName}";
        }
    }
    private class PlayerState : BaseState
    {
        protected PlayerController owner;

        protected Rigidbody2D rb
        {
            get { return owner.rb; }
        }
        protected Animator anim
        {
            get { return owner.anim; }
        }
        protected float inputX
        {
            get { return owner.inputX; }
        }
        protected float inputY
        {
            get { return owner.inputY; }
        }
        protected bool canThrowing
        {
            get { return owner.canThrowing; }
        }
        protected string GetDirectionAnim(string baseName)
        {
            return owner.GetDirectionAnim(baseName);
        }
        protected void StoneThrowing()
        {
            owner.StartCoroutine(owner.StoneThrowing());
        }
        public PlayerState(PlayerController owner)
        {
            this.owner = owner;
        }
    }
    private class IdleState : PlayerState
    {
        public IdleState(PlayerController owner) : base(owner) { }
        public override void Enter()
        {
            rb.velocity = Vector2.zero;
            anim.Play(GetDirectionAnim("Idle"));
        }
        public override void Transition()
        {
            if (Mathf.Abs(inputX) > 0 || Mathf.Abs(inputY) > 0)
            {
                ChangeState(State.Walk);
            }
            if (Input.GetKeyDown(KeyCode.Space) && canThrowing)
            {
                ChangeState(State.Attack);
            }
        }
    }
    private class AttackState : PlayerState
    {
        public AttackState(PlayerController owner) : base(owner) { }
        public override void Enter()
        {
            StoneThrowing();
            anim.Play(GetDirectionAnim("Attack"));
        }
        public override void Transition()
        {
            if (Mathf.Abs(inputX) > 0 || Mathf.Abs(inputY) > 0)
            {
                ChangeState(State.Walk);
            }
            if (inputX == 0 && inputY == 0)
            {
                ChangeState(State.Idle);
            }
        }
    }
    private class WalkState : PlayerState
    {
        public WalkState(PlayerController owner) : base(owner) { }
        public override void Enter()
        {
            anim.Play(GetDirectionAnim("Walk"));
        }
        public override void Transition()
        {
            if (Input.GetKeyDown(KeyCode.Space) && canThrowing)
            {
                ChangeState(State.Attack);
            }
            if (inputX == 0 && inputY == 0)
            {
                ChangeState(State.Idle);
            }
        }
    }
    private void Move()
    {
        if (!isPushed) rb.velocity = new Vector2(inputX * Speed, inputY * Speed);
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
