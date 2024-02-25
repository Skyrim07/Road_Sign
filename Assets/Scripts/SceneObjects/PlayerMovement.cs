using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public bool hitByCar;
    public bool lose;

    private float speed = 3f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float accel = 0.5f;

    private Rigidbody2D rb;
    private Vector2 playerInput;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerInput.Normalize();
        if (lose) { playerInput = Vector2.zero; }
    }
    private void FixedUpdate()
    {
        rb.velocity = playerInput * (Mathf.Lerp(speed, maxSpeed, accel));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            hitByCar = true;
            lose = true;
        }
    }
}
