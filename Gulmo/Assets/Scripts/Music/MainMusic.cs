using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMusic : MonoBehaviour
{
    public AudioSource menuBGM;
    public AudioSource mainBGM;
    public AudioSource TT;
    private Camera cameras;

    private Transform player;
    public Transform t;

    private int cnt = 0;
    private int c = 0;

    void Start()
    {
        cameras = GameObject.Find("Camera_MENU").GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (!cameras.enabled)
        {
            menuBGM.Stop();
            if (cnt == 0)
            {
                mainBGM.Play();
                cnt ++;
            }
        }
    }
}
