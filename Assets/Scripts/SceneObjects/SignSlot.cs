using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class SignSlot : MonoBehaviour
{
    public List<Sign> signs = new List<Sign>();
    public Road road;
    public float angle;

    [SerializeField] Animator indicatorAnim, cancelAnim;
    [SerializeField] Animator invalidSignAnim;

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
                if (signs.Count == 0)
                    PlayerPlaceSign();
                else
                    PlayerDestroySign();
            }
        }
    }

    private void PlayerDestroySign()
    {
        if(signs.Count > 0)
        {
            foreach (var sign in signs)
            {
                Destroy(sign.gameObject);
            }
            LevelManager.instance.OnDeleteSlot();
            signs.Clear();
        }
    }
    private void PlayerPlaceSign()
    {
        if(PlayerLogic.HasValidSign())
        {
            indicatorAnim.Disappear();
            InstantiateSignObject(PlayerLogic.instance.sign.type);
            PlayerLogic.instance.DestroySign();

            LevelManager.instance.OnPlaceSlot();
        }
        else
        {
           
        }
    }

    private void InstantiateSignObject(SignType sign)
    {
        SKAudioManager.instance.PlaySound("signplace1");
        if(sign ==SignType.Stop)
        {
            GameObject inst = Instantiate(CommonReference.instance.stopSignPF, transform);
            inst.transform.Rotate(0, 0, angle);
            signs.Add(inst.GetComponent<Sign>());
        }
    }

    private void OnPlayerEnter()
    {
        isPlayerIn = true;
        if (signs.Count == 0)
        {
            if (PlayerLogic.HasValidSign())
            {
                indicatorAnim.Appear();
            }
            else
            {
                invalidSignAnim.Appear();
            }
        }
        else
        {
            cancelAnim.Appear();
        }

    }
    private void OnPlayerExit()
    {
        isPlayerIn = false;
        cancelAnim.Disappear();
        indicatorAnim.Disappear();
        invalidSignAnim.Disappear();
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
