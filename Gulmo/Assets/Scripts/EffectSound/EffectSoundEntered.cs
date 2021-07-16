using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSoundEntered : MonoBehaviour
{
    private AudioSource audioSource;

    private Ray ray;
    private RaycastHit hitinfo;
    private Camera mainCamera;
    private Camera stopCamera;
    private bool rayStart = false;
    private bool rayExit = false;
    private bool raySetting = false;

    private int cnt = 0;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = GameObject.Find("Camera_MENU").GetComponent<Camera>();
        stopCamera = GameObject.Find("Stop Camera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (mainCamera.enabled)
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            ButtonClick("startButton", "startButton_Click", "exitButton", "exitButton_Click", "settingButton", "settingButton_Click");
        }

        if (stopCamera.enabled)
        {
            ray = stopCamera.ScreenPointToRay(Input.mousePosition);
            ButtonClick("backButton_stop", "backButtonClick_stop", "exitButton_stop", "exitButtonClick_stop", "settingButton_stop", "settingButtonClick_stop");
        }
    }

    private void ButtonClick(string whatButton_1, string whatButton_1_click, string whatButton_2, string whatButton_2_click, string whatButton_3, string whatButton_3_click)
    {
        if (Physics.Raycast(ray, out hitinfo))
        {
                if (hitinfo.transform.gameObject.name == whatButton_1)
                {
                    audioSource.Play();
                    rayStart = true;
                }
                else if (hitinfo.transform.gameObject.name != whatButton_1_click && rayStart)
                {
                    rayStart = false;
                }
                if (hitinfo.transform.gameObject.name == whatButton_2)
                {
                    audioSource.Play();
                    rayExit = true;
                }
                else if (hitinfo.transform.gameObject.name != whatButton_2_click && rayExit)
                {
                    rayExit = false;
                }
                if (hitinfo.transform.gameObject.name == whatButton_3)
                {
                    audioSource.Play();
                    raySetting = true;
                }
                else if (hitinfo.transform.gameObject.name != whatButton_3_click && raySetting)
                {
                    raySetting = false;
                }
        }
    }
    
        
}
