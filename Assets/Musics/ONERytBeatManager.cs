using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ONERytBeatManager : MonoBehaviour
{
    public AudioSource Music,clap;
    public Animator HeadPhone;
    public GameObject HeadPhoneOBJ;
    public GameObject Beat,Beateffect;
    public float[] TimeInterval;
    public float[] TimeDelay;
    public int[] Repeat;


    public Event1 Event1;
    public int[] EventID;
    public int[] EventDelay;
    bool IsPerfect,IsAlright;
    public float acuracy, AccuracyState;
    public LayerMask triggerPERFECT, triggerALRIGHT;
    public Sprite UIP, UIA, UIS, UII;
    void Start()
    {
        Invoke("MusicPlay", 3);
    }
    private IEnumerator ScheduleInvokes(float[] timings, int[] repeats, float[] startDelays, Action func)
    {
        int len = Mathf.Min(timings.Length, repeats.Length, startDelays.Length);

        for (int i = 0; i < len; i++)
        {
            float startDelay = startDelays[i];
            float delay = timings[i];
            int count = repeats[i];

            yield return new WaitForSeconds(startDelay);  // ? Wait before starting

            for (int j = 0; j < count; j++)
            {
                func?.Invoke();                           // ?? Call the function
                yield return new WaitForSeconds(delay);   // ?? Wait between repeats
            }
        }
    }

    void MusicPlay()
    {
        Music.Play(); StartCoroutine(ScheduleInvokes(TimeInterval, Repeat, TimeDelay, SpawnBeat));
        int len = Mathf.Min(EventDelay.Length, EventID.Length);

        for (int i = 0; i < len; i++)
        {
            StartCoroutine(EventCaller(EventDelay[i], EventID[i]));
        }
    }
    private IEnumerator EventCaller(float delay, int id)
    {
        yield return new WaitForSeconds(delay);
        Event1.EventHandler(id);
    }
    void SpawnBeat()
    {
        Debug.Log("Spawned");
        Beat.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        IsPerfect = Physics.CheckSphere(Beat.transform.position, acuracy, triggerPERFECT);
        IsAlright = Physics.CheckSphere(Beat.transform.position, acuracy, triggerALRIGHT);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            Beat.SetActive(false);
            if (IsPerfect)
            {
                clap.Play();
                AccuracyState = AccuracyState+1;
                Beateffect.SetActive(true);
                Event1.EventHandler(-1);
                HeadPhoneOBJ.GetComponent<SpriteRenderer>().sprite = UIP;
                Invoke("HeadPHonereset", .5f);
            }
            if (IsAlright)
            {
                AccuracyState = AccuracyState+.5f; 
                Beateffect.SetActive(true);
                Event1.EventHandler(-2);
                HeadPhoneOBJ.GetComponent<SpriteRenderer>().sprite = UIA; Invoke("HeadPHonereset", .5f);

            }
            if (!IsAlright && !IsPerfect)
            {
                AccuracyState = AccuracyState - .5f;
                Beateffect.SetActive(true);
                Event1.EventHandler(-2);
                HeadPhoneOBJ.GetComponent<SpriteRenderer>().sprite = UIS; Invoke("HeadPHonereset", .5f);

            }
        }
    }
    void HeadPHonereset()
    {
        HeadPhoneOBJ.GetComponent<SpriteRenderer>().sprite = UII;
    }
}
