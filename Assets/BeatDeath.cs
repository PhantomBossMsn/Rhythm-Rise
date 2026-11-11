using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDeath : MonoBehaviour
{
    public float acuracy, time;
    public bool KillbyTrig,NonVectReset, IsShit;
    public LayerMask triggerSHIT;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (!KillbyTrig)
        {
            Invoke("off", time);
        }
      
    }
    private void Update()
    {
        if (KillbyTrig) { 
            IsShit = Physics.CheckSphere(transform.position, acuracy, triggerSHIT);
            if (IsShit) { off(); }
        }
    }
    void off()
    {
        gameObject.SetActive(false);
        CancelInvoke("off");
        if (!NonVectReset)
        {
            transform.position = Vector3.zero;
        }
        
        Debug.Log("sigma");
    }
 
}
