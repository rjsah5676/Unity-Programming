using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAnimation : MonoBehaviour
{
    private Camera mainCamera;
    private Animator ani;
    private Ray ray;
    private RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Camera_MENU").GetComponent<Camera>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitInfo)) {
            if(hitInfo.transform.gameObject.name == "menuUnitychan")
            {
                ani.SetBool("isCursor", true);
                //ani.Play("HANDUP", 0 , 0);
            }
            if (hitInfo.transform.gameObject.name != "menuUnitychan")
                ani.SetBool("isCursor", false);
        }
    }
}
