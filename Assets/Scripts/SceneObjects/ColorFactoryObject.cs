using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class ColorFactoryObject : MonoBehaviour
{

    [SerializeField] Animator indicatorAnim;

    private bool isPlayerIn;
    private void Start()
    {

    }

    private void Update()
    {
        if (isPlayerIn)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PlayerLogic.instance.EnterColorFactory();
            }
        }
    }

    private void OnPlayerEnter()
    {
        isPlayerIn = true;
        indicatorAnim.Appear();
    }
    private void OnPlayerExit()
    {
        isPlayerIn = false;
        indicatorAnim.Disappear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerEnter();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPlayerExit();
        }
    }
}
