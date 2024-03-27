using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class ShapeFactoryObject : MonoBehaviour
{
    public bool isEnterDirectly = false;
    [SerializeField] Animator indicatorAnim;

    private bool isPlayerIn;
    private void Start()
    {

    }

    private void Update()
    {
        return;
        if (isPlayerIn)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                 PlayerLogic.instance.EnterShapeFactory();
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
        if (isEnterDirectly)
        {
            PlayerLogic.instance.EnterShapeFactory();
        }
        else
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
