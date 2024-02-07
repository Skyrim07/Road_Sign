using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float lifetime;


    private float timer;
    void Update()
    {
        timer += Time.deltaTime * RuntimeData.timeScale;
        if(timer> lifetime)
        {
            Destroy(gameObject);
        }
        transform.Translate(direction * speed * Time.deltaTime * RuntimeData.timeScale);
    }
}
