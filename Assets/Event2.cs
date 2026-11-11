using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event2 : MonoBehaviour
{
    bool InearDoor, InearTrigger, InearBOT, InearCar,InearDeath, InearForceTrigg, InearForceTrigg2, InearForceTrigg3, InearTrigger2;
    public LayerMask doorLAYER, triggerLAYER, BOTLAYER, CARLAYER, DeathLayer,ForcedTrigger, ForcedTrigger2, ForcedTrigger3, triggerLAYER2;
    public float senceDist;
    public Animator DoorAnimator;
    public int SceneNum;
    public GameObject wsgAnime, killTrig,Eshocase,TeleportPOS, UITALKER;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InearDoor = Physics.CheckSphere(transform.position, senceDist, doorLAYER);
        InearTrigger = Physics.CheckSphere(transform.position, senceDist, triggerLAYER);
        InearTrigger2 = Physics.CheckSphere(transform.position, senceDist, triggerLAYER2);
        InearBOT= Physics.CheckSphere(transform.position, senceDist, BOTLAYER);
        InearCar= Physics.CheckSphere(transform.position, senceDist, CARLAYER);
        InearDeath = Physics.CheckSphere(transform.position, senceDist, DeathLayer);
        InearForceTrigg = Physics.CheckSphere(transform.position, senceDist, ForcedTrigger);
        InearForceTrigg2 = Physics.CheckSphere(transform.position, senceDist, ForcedTrigger2);
        InearForceTrigg3 = Physics.CheckSphere(transform.position, senceDist, ForcedTrigger3);
        if (InearDoor)
        {
            DoorAnimator.SetInteger("states", 0);
        }
        if (!InearDoor)
        {
            DoorAnimator.SetInteger("states", 1);
        }
        if(InearTrigger||InearCar)
        {
            Eshocase.SetActive(true);
        }
        if(!InearTrigger && !InearCar)
        {
            Eshocase.SetActive(false);
        }
        if(InearTrigger && Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Rigidbody>().MovePosition(TeleportPOS.transform.position);
        }
        if (InearTrigger2)
        {
            UITALKER.SetActive(true);
            Invoke("transition", 13);
        }
        if (InearCar && Input.GetKeyDown(KeyCode.E))
        {
            UITALKER.SetActive(true);
            Invoke("transition", 3);
        }
        //Parkore only
        if(InearDeath)
        {
            SceneManager.LoadScene(SceneNum);
        }
        if (InearForceTrigg)
        {
            gameObject.GetComponent<PlayerMovement>().enabled = false;

            wsgAnime.SetActive(true);
            Invoke("transition", 10f);
        }
        if (InearForceTrigg2)
        {
            killTrig.SetActive(false);


        }
        if (InearForceTrigg3)
        {

            killTrig.SetActive(false);
            transition();
        }
    }
    void transition()
    {
        SceneManager.LoadScene(SceneNum+1);
    }
}
