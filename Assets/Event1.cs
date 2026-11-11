using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Event1 : MonoBehaviour
{
    public Animator transitionObacity,Roboy;
    public VideoPlayer VidPlayer;
    public BeatManager BeatManager;
    public GameObject Cameraobj, transitionObacityOBJ;
    public Transform ManPos, defualtPos;
    public Material shader, shader2;
    public float[] camMoveSpeed;  // red component
    public float[] r;  // red component
    public float[] g;  // green component
    public float[] b;  // blue component
    public bool[] BOOLbuffer;
    ///lvl2 NightBus///

    public GameObject bus;
    private void Start()
    {
        transitionObacity.speed = .5f;
        shader.color = new Color(r[0], g[0], b[0]);
    }
    public void EventHandler(int id)
    {
        Debug.Log("calledEvent");
        //TrigEffect
        if (id == -1)
        {
            shader2.color = new Color(r[2], g[2], b[2], 0.5686275f);
        }
        if (id == -2)
        {
            shader2.color = new Color(r[3], g[3], b[3],0.5686275f);
        }
        ////
        if (id == 1)
        {
            BOOLbuffer[1] = true;

            shader.color = new Color(r[1], g[1], b[1]);
        }
        if (id == 2)
        {
            BOOLbuffer[2] = true;
            BOOLbuffer[1] = false;
            shader.color = new Color(r[0], g[0], b[0]);
        }
        if (id == 3)
        {
            BOOLbuffer[2] = false;
            BeatManager.enabled = false;
            Invoke("end", 3);
        }
        ////LVL2 Nightbus////


        if (id == 4)
        {
            bus.SetActive(true);
            Invoke("end", 5);
        }
        if(id==5)
        {
            Invoke("end", 5);
        }
        ////Lvl3 Roby///
        if (id==6)
        {
            Roboy.SetInteger("states", 1);

        }
        if (id==7)
        {
            VidPlayer.Play();
            ManPos.gameObject.SetActive(true);
        }
        if (id == 8)
        {
            BOOLbuffer[2] = true;
            ManPos.gameObject.SetActive(false);
        }
        if (id == 9)
        {
            Invoke("end", 5);
        }
        ////Lvl4 Car////
        if (id==10)
        {
            BOOLbuffer[1]=true;
            BOOLbuffer[2]=false;

        }
        if (id == -10)
        {
            BOOLbuffer[1]=false;
            BOOLbuffer[2]=true;
        }
        ////Lvl5 Hotel////
        if (id == 11)
        {
            ManPos.gameObject.SetActive(true);
            Invoke("SceneChange",1f);
        }
    }
    public void SceneChange()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void Update()
    {
        if (BOOLbuffer[1])
        {
            Cameraobj.transform.position = Vector3.Lerp(Cameraobj.transform.position, ManPos.position, camMoveSpeed[1] * Time.deltaTime);

        }
        if (BOOLbuffer[2])
        {
            Cameraobj.transform.position = Vector3.Lerp(Cameraobj.transform.position, defualtPos.position, camMoveSpeed[1] * Time.deltaTime);

        }
    }
    void end()
    {
        transitionObacityOBJ.SetActive(true);
        Debug.Log("end");
    }

}
