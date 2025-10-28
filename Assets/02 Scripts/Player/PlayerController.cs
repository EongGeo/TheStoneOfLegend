using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int Str {  get; set; }
    public int Hp { get; set; }
    public float Speed { get; set; }


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float inputX, inputY;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyPlayerData(Managers.Game.playerData);
    }
    private void Start()
    {

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
        if (inputY != 0)
        {
            if (inputY < 0)
            {
                spriteRenderer.flipY = true;
            }
            else
            {
                spriteRenderer.flipY = false;
            }
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputX * Speed, inputY * Speed);
    }
    public void ApplyPlayerData(PlayerData data)
    {
        Str = data.playerStr;
        Hp = data.playerMaxHp;
        Speed = data.playerSpeed;
    }
}
