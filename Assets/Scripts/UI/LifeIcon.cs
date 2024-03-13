using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIcon : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void SetState(bool active)
    {
        anim.SetBool("Appear", active);
    }
}
