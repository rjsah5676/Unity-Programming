using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicArea : MonoBehaviour
{
    private AudioSource audio;

    private int c = 0;
    private bool stoP = false;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        c++;
        if(stoP)
        {
            if (c > 100)
            {
                audio.Stop();
                stoP = false;
            }
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (!stoP)
        {
            if (coll.gameObject.tag == "Player")
                audio.Play();
            c = 0;
        }
        stoP = false;
    }
    private void OnCollisionExit(Collision coll)
    {
        c = 0;
        stoP = true;
    }
}
