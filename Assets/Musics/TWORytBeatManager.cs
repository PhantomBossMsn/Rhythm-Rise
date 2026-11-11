using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TWORytBeatManager : MonoBehaviour
{
    public AudioSource Music,clap;
    public Animator HeadPhone, Beat1Animator, Beat2Animator;
    public GameObject Beat1, Beat2, Beateffect;
    public float[] TimeInterval;
    public float[] TimeDelay;
    public int[] Repeat;
    public bool[] IsB1;
    public int index;
    public float AnimatorSpeed1, AnimatorSpeed2,Killb1,Killb2;

    public Event1 Event1;
    public int[] EventID;
    public int[] EventDelay;
    bool IsPerfect,IsAlright;
    public float AccuracyState;
    public float acuracy;
    public LayerMask triggerPERFECT, triggerALRIGHT;
    void Start()
    {
        Beat1Animator.speed = AnimatorSpeed1;
        Beat2Animator.speed = AnimatorSpeed2;
        Invoke("MusicPlay", 3);
    }
    private IEnumerator ScheduleInvokes(float[] timings, int[] repeats, float[] startDelays, Action func)
    {
        int len = Mathf.Min(timings.Length, repeats.Length, startDelays.Length);

        for (index = 0; index < len; index++)
        {
            float startDelay = startDelays[index];
            float delay = timings[index];
            int count = repeats[index];

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
        if (IsB1[index])
        {
            Debug.Log("Spawned");
            Beat1.SetActive(true);
            Invoke("B1Killer", Killb1);
        }
        else if (!IsB1[index])
        {
            Debug.Log("Spawned");
            Beat2.SetActive(true);
            Invoke("B2Killer", Killb2);

        }

    }
    // Update is called once per frame
    void Update()
    {
        IsPerfect = Physics.CheckSphere(Beat2.transform.position, acuracy, triggerPERFECT) || Physics.CheckSphere(Beat1.transform.position, acuracy, triggerPERFECT);
        IsAlright = Physics.CheckSphere(Beat2.transform.position, acuracy, triggerALRIGHT) || Physics.CheckSphere(Beat1.transform.position, acuracy, triggerALRIGHT);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            if (IsB1[index]==true)
            {
                B1Killer();

            }
            if (IsB1[index] == false)
            {
                B2Killer();
            }
            if (IsPerfect)
            {
                Beateffect.SetActive(true);
                Event1.EventHandler(-1);
                clap.Play();
                AccuracyState = AccuracyState + 1f;
            }
            if (IsAlright)
            {
                Beateffect.SetActive(true);
                Event1.EventHandler(-2);
                AccuracyState = AccuracyState + .5f;
            }
            if (!IsAlright && !IsPerfect)
            {
                Beateffect.SetActive(true);
                Event1.EventHandler(-2);

                AccuracyState = AccuracyState - .5f;
            }
        }
    }
    public void B1Killer()
    {
        Event1.EventHandler(10);
        Beat1.SetActive(false);
        Beat1.transform.position = Vector3.zero;
    }
    public void B2Killer()
    {
        Event1.EventHandler(-10);
        Beat2.SetActive(false);
        Beat2.transform.position = Vector3.zero;
    }
}
