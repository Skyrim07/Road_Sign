using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatSlot : MonoBehaviour
{
    [SerializeField] GameObject fullVisual, emptyVisual;
    [SerializeField] Animator anim;

    private int id;
    void Start()
    {
        id = transform.GetSiblingIndex();
        UpdateInfo();

        if (id == 0)
            OnSelect();
    }

    public void UpdateInfo()
    {
        if (PersistentData.hatUnlocked.Contains(id))
        {
            fullVisual.SetActive(true);
            emptyVisual.SetActive(false);
        }
        else
        {
            fullVisual.SetActive(false);
            emptyVisual.SetActive(true);
        }

        anim.SetBool("Appear", PersistentData.equippedHat == id);
    }

    public void OnSelect()
    {
        if (!PersistentData.hatUnlocked.Contains(id))
            return;
        HatPanel.instance.OnSelectHatSlot(this);
        anim.SetBool("Appear", true);

        PersistentData.equippedHat = id; 
    }
    public void OnDeselect()
    {
        anim.SetBool("Appear", false);
    }
}
