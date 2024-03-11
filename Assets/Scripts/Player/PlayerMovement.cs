using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public bool hitByCar;
    public bool lose;
    public PlayerLogic playerLogic;

    private float speed = 3f;
    [SerializeField] private float maxSpeed = 2f;
    //[SerializeField] private float accel = 0.5f;
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private float bounceForce = 8f;
    [SerializeField] private float drag = 2f;
    public AnimationCurve playerMovementCurve;

    [SerializeField] Animator anim;

    private Rigidbody2D rb;
    private Vector2 playerInput;

    private bool stopMovement;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLogic = GetComponent<PlayerLogic>();
    }
    private void Update()
    {
        
        anim.SetBool("Walk", playerInput.sqrMagnitude > 0);

        if(playerInput.sqrMagnitude > 0)
        {
            float orientation = Mathf.Atan2(playerInput.y, playerInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, orientation - 90);
        }

        if (lose) { playerInput = Vector2.zero; }
    }
    private void FixedUpdate()
    {
        float playerX = Input.GetAxisRaw("Horizontal");
        float playerY = Input.GetAxisRaw("Vertical");

        //disable diagonal movement
        //if (playerX != 0 && playerY != 0)
        //{
        //    if (Mathf.Abs(playerX) > Mathf.Abs(playerY))
        //    {
        //        playerY = 0;
        //        rb.velocity = new Vector2(playerX, 0);
        //    }

        //    else
        //    {
        //        playerX = 0;
        //        rb.velocity = new Vector2(0, playerY);
        //    }

        //}

        playerInput = new Vector2(playerX, playerY).normalized;

        stopMovement = playerLogic.InFactory();
        if(stopMovement) { playerInput = Vector2.zero; }


        float moveCurve = playerMovementCurve.Evaluate(Time.time);

        rb.AddForce(playerInput * moveForce * moveCurve, ForceMode2D.Force);

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if(playerInput == Vector2.zero)
        {
            rb.velocity -= rb.velocity * drag * Time.fixedDeltaTime;
        }

        //rb.velocity = playerInput * (Mathf.Lerp(speed, maxSpeed, accel)) * RuntimeData.timeScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collide");
        Vector2 normal = collision.contacts[0].normal;
        Vector2 dir = Vector2.Reflect(rb.velocity.normalized, normal).normalized;
        rb.AddForce(dir * bounceForce, ForceMode2D.Impulse);

        if (collision.gameObject.CompareTag("Car"))
        {
            hitByCar = true;
            lose = true;
            SKAudioManager.instance.PlaySound("die");
        }
    }

}
