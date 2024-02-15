using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class SignSlot : MonoBehaviour
{
    public bool isOccupied = false;
    //not sure if we want to have more than one sign in the same slot but rn its just one
    public Sign.SignType mySignType;
    public GameObject signPrefab;
    public Sign mySign;
    public Road myRoad;
    private void Start()
    {
    }
    public void OnMouseDown()
    {
        //rn only accounts for stop signs, will change later once we get more
        if(!isOccupied)
        {
            isOccupied= true;
            mySignType = Sign.SignType.Stop;
            mySign = Instantiate(signPrefab, transform.position - Vector3.forward,transform.rotation,this.transform).GetComponent<Sign>();
            mySign.type= mySignType;

            SKAudioManager.instance.PlaySound("signplace1");
        }
        else
        {
            isOccupied = false;
            Destroy(mySign.gameObject);
        }
    }
}
