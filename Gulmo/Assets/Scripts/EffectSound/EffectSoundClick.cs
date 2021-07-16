using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EffectSoundClick : MonoBehaviour
{
    private AudioSource audioSource;

    private Ray ray;
    private RaycastHit hitinfo;
    private Camera mainCamera;
    private Camera stopCamera;

    private bool isMain = true;
    private bool isStop = false;
    private int cnt = 0;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Camera_MENU").GetComponent<Camera>();
        stopCamera = GameObject.Find("Stop Camera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && isMain)
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitinfo))
                {
                if (hitinfo.transform.gameObject.name == "startButton_Click")
                {
                    audioSource.Play();
                    isMain = false;
                }
                }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            isStop = true;
        if (isStop)
        {
            cnt++;
            if (Input.GetKeyDown(KeyCode.Escape) && cnt > 3)
            {
                isStop = false;
                cnt = 0;
            }
            ray = stopCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hitinfo))
                {
                    if (hitinfo.transform.gameObject.name == "backButtonClick_stop")
                    {
                        audioSource.Play();
                        isStop = false;
                        cnt = 0;
                    }
                }
            }
        }
    }

}
