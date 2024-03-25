using SKCell;
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
    [SerializeField] private float maxSpeed = 2f;
    //[SerializeField] private float accel = 0.5f;
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private float bounceForce = 8f;
    [SerializeField] private float drag = 2f;
    public AnimationCurve playerMovementCurve;


    [SKFolder("Player Hurt")]
    [SerializeField] private float hurtFlashAmt;
    [SerializeField] private float hurtFlashTime;
    private Color ogCol;

    private float playerHitWait = 1.5f;

    [SerializeField] Animator anim;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRend;

    private Vector2 playerInput;
    private bool stopPlayerInput;

    private Rect cameraRect;
    private Vector3 cameraPos;
    private Vector3 bottomLeft, topRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLogic = GetComponent<PlayerLogic>();
        spriteRend= GetComponent<SpriteRenderer>();
        ogCol = spriteRend.color;


    }
    private void Update()
    {


        anim.SetBool("Walk", playerInput.sqrMagnitude > 0);

        if (playerInput.sqrMagnitude > 0)
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


        if(cameraRect != null)
        {
            Vector2 clampedPos = transform.position;
            clampedPos = new Vector2(Mathf.Clamp(clampedPos.x, cameraRect.xMin, cameraRect.xMax), Mathf.Clamp(clampedPos.y, cameraRect.yMin, cameraRect.yMax));

            transform.position = clampedPos;
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

        rb.AddForce(playerInput * moveForce * moveCurve, ForceMode2D.Force);

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
    public void HitByCar(Transform carPos)
    {
        stopPlayerInput = true;
        StartCoroutine(PlayerHitWait());
        Vector2 bounceDir = (transform.position - carPos.position).normalized;
        rb.velocity = Vector2.zero;
        rb.AddForce(bounceDir * bounceForce, ForceMode2D.Impulse);
    }

    public void CameraCalculations()
    {
        cameraPos = Camera.main.transform.position;
        bottomLeft = Camera.main.ScreenToWorldPoint(cameraPos);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth + cameraPos.x, Camera.main.pixelHeight + cameraPos.y));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        transform.position = cameraPos;
    }
    private IEnumerator PlayerHitWait()
    {
        //yield return new WaitForSeconds(playerHitWait * Time.timeScale);

        for(int i=0; i< hurtFlashAmt; i++)
        {
            spriteRend.color = Color.red;
            yield return new WaitForSeconds(hurtFlashTime);
            spriteRend.color = ogCol;
            yield return new WaitForSeconds(hurtFlashTime);
        }
        spriteRend.color = ogCol;
        stopPlayerInput = false;
    }

}
