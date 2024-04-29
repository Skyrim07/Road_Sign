using SKCell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public bool hitByCar;
    public bool lose;
    public PlayerLogic playerLogic;

    [SKFolder("Movement Values")]
    [SerializeField] private float maxSpeed = 4f;
    //[SerializeField] private float accel = 0.5f;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 0.3f;
    [SerializeField] private float bounceForce = 100f;
    [SerializeField] private float drag = 8f;
    public AnimationCurve playerMovementCurve;
    private bool isDashing;
    private float lastDashTime;
    private KeyCode dashControl = KeyCode.LeftShift;


    [SKFolder("Player Hurt")]
    [SerializeField] private float hurtFlashAmt;
    [SerializeField] private float hurtFlashTime;
    private bool invincible;
    private Color ogCol;

    private float playerHitWait = 1.5f;

    [SerializeField] Animator anim;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;

    private Vector2 playerInput;
    private bool stopPlayerInput;

    private Rect cameraRect;
    [SerializeField] private Vector3 cameraPos;
    [SerializeField] private Vector3 bottomLeft, topRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLogic = GetComponent<PlayerLogic>();
        spriteRend= GetComponent<SpriteRenderer>();
        ogCol = spriteRend.color;
        CameraCalculations();


    }
    private void Update()
    {
        if (RuntimeData.isLevelComplete) return;
       

        anim.SetBool("Walk", playerInput.sqrMagnitude > 0);

        if (playerInput.sqrMagnitude > 0 && RuntimeData.timeScale>0)
        {
            float orientation = Mathf.Atan2(playerInput.y, playerInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, orientation - 90);
        }

        if (lose) { playerInput = Vector2.zero; }


    }
    private void FixedUpdate()
    {
        if (RuntimeData.isLevelComplete)
        {
            rb.velocity = Vector2.zero;
            return;
        }


        if (cameraPos != Camera.main.transform.position)
        {
            CameraCalculations();
        }
        float playerX = Input.GetAxisRaw("Horizontal");
        float playerY = Input.GetAxisRaw("Vertical");
        if(cameraRect != null)
        {
            Vector2 clampedPos = transform.position;
            clampedPos = new Vector2(Mathf.Clamp(clampedPos.x, cameraRect.xMin, cameraRect.xMax), Mathf.Clamp(clampedPos.y, cameraRect.yMin, cameraRect.yMax));

            transform.position = clampedPos;
        }
        if (Input.GetKeyDown(dashControl) && Time.time > lastDashTime + dashDuration)
        {
            SKAudioManager.instance.PlaySound("dash cartoony");

            isDashing = true;
            lastDashTime = Time.time;
        }
        if (isDashing && Time.time < lastDashTime + dashDuration)
        {
            Vector2 dashDirection = playerInput.normalized;
            rb.velocity = dashDirection * dashForce;
            return;
        }
        if (isDashing && Time.time >= lastDashTime + dashDuration)
        {
            isDashing = false;
        }


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

        stopPlayerInput = playerLogic.InFactory();
        if (stopPlayerInput) { playerInput = Vector2.zero; }


        float moveCurve = playerMovementCurve.Evaluate(Time.time);

        rb.AddForce(playerInput * moveForce * moveCurve * RuntimeData.timeScale, ForceMode2D.Force);


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if (playerInput == Vector2.zero)
        {
            rb.velocity -= rb.velocity * drag * Time.fixedDeltaTime;
        }

        //rb.velocity = playerInput * (Mathf.Lerp(speed, maxSpeed, accel)) * RuntimeData.timeScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collide");
        //Vector2 normal = collision.contacts[0].normal;
        //Vector2 dir = Vector2.Reflect(rb.velocity.normalized, normal).normalized;
        //rb.AddForce(dir * bounceForce, ForceMode2D.Impulse);
        if (collision.gameObject.CompareTag("Car"))
        {
            hitByCar = true;
            lose = true;
            SKAudioManager.instance.PlaySound("die");

            Debug.Log("collide");
            
        }

    }
    //private void Dash()
    //{
    //    Vector2 dir = playerInput.normalized;
    //    rb.velocity = Vector2.zero;
    //    rb.AddForce(dir * dashForce, ForceMode2D.Impulse);
    //}
    public void HitByCar(Transform carPos, float speed)
    {
        //only hurt and bounce back if car is moving
        float bounce = bounceForce;
        Debug.Log(speed);
        if(speed >= 0.1f && !invincible)
        {
            FlowManager.instance.OnPlayerCollision();
            stopPlayerInput = true;
            StartCoroutine(PlayerHitWait());
        }
        else
        {
            bounce = bounceForce / 4;
        }
        Vector2 bounceDir = (transform.position - carPos.position).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(bounceDir * bounce, ForceMode2D.Impulse);
    }

    public void CameraCalculations()
    {
        Debug.Log("camera calc");
        cameraPos = Camera.main.transform.position;
        bottomLeft = Camera.main.ScreenToWorldPoint(cameraPos);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth + cameraPos.x, Camera.main.pixelHeight + cameraPos.y));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        //transform.position = cameraPos;
    }
    private IEnumerator PlayerHitWait()
    {
        //yield return new WaitForSeconds(playerHitWait * Time.timeScale);
        invincible = true;
        for(int i=0; i< hurtFlashAmt; i++)
        {
            spriteRend.color = Color.red;
            yield return new WaitForSeconds(hurtFlashTime);
            spriteRend.color = ogCol;
            yield return new WaitForSeconds(hurtFlashTime);
        }
        spriteRend.color = ogCol;
        stopPlayerInput = false;
        invincible = false;
    }

}
