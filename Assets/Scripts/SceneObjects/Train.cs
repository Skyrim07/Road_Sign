using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class Train : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.Translate((transform.rotation * Vector2.up) * speed * Time.fixedDeltaTime * RuntimeData.timeScale, Space.World);
    }

    private void OnHitPlayer(PlayerMovement player)
    {
        player.HitByCar(transform, speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<PlayerMovement>(out var player))
        {
            OnHitPlayer(player);
        }
    }
}
