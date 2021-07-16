using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rd;
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        rd.AddForce(new Vector3(0, -1f, -1f)* 100, ForceMode.VelocityChange);
    }

    // Update is called once per frame
}
