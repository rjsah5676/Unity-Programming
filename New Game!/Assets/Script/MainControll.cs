using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControll : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;
    private Rigidbody rd;
    private float h = 0.0f;    //수평
    private float v = 0.0f;    //수직

    private float jumpSpd = 5.0f;
    public float moveSpd = 5.0f;
    public float rotateSpd = 100.0f;

    public bool jump = false;

    [SerializeField] private MouseRotate m_MouseRotate;
    [SerializeField] private Camera m_Camera;


    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _animator = GetComponentInChildren<Animator>();
        m_Camera = Camera.main;
        m_MouseRotate.Init(transform, m_Camera.transform);
        rd = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        RotateView();

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        _animator.SetFloat("Blend", v);
        Vector3 moveDirect = (Vector3.forward * v) + (Vector3.right * h);

        _transform.Translate(moveDirect.normalized * Time.deltaTime * moveSpd, Space.Self);

        //   _transform.Rotate(Vector3.up * Time.deltaTime * rotateSpd * Input.GetAxis("Mouse X"));

     /*if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _animator.SetBool("IsRun_F", true);
                moveSpd = 10.0f;
            }
            if(Input.GetKeyUp(KeyCode.E))
            {
                _animator.SetBool("IsRun_F", false);
                moveSpd = 5.0f;
            }
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            _animator.Play("Jab", -1, 0);
        }

        if (jump == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
                Jump();
                _animator.Play("JUMP00", -1, 0);
            }
        }
            if (v >= 0.1f)
        {
            _animator.SetBool("IsRun", true);
            if (h == 0)
            {
                _animator.SetBool("IsWalk_L", false);
                _animator.SetBool("IsWalk_R", false);
            }
        }
        else if(v <= -0.1f)
        {
            _animator.SetBool("IsRun", true);
            if (h == 0)
            {
                _animator.SetBool("IsWalk_L", false);
                _animator.SetBool("IsWalk_R", false);
            }
        }
        else if (h >= 0.1f)
        {
            _animator.SetBool("IsWalk_R", true);
            if (v == 0)
            {
                _animator.SetBool("IsWalk_F", false);
                _animator.SetBool("IsWalk_B", false);
            }
        }
        else if (h <= -0.1f)
        {
            _animator.SetBool("IsWalk_L", true);
            if (v == 0)
            {
                _animator.SetBool("IsWalk_F", false);
                _animator.SetBool("IsWalk_B", false);
            }
        }
        else
        {
            _animator.SetBool("IsWalk_F", false);
            _animator.SetBool("IsWalk_B", false);
            _animator.SetBool("IsWalk_L", false);
            _animator.SetBool("IsWalk_R", false);
        }
    }

    private void FixedUpdate()
    {
        m_MouseRotate.UpdateCursorLock();
    }

    private void RotateView()
    {
        m_MouseRotate.LookRotation(transform, m_Camera.transform);
    }
    private void Jump()
    {
        rd.AddForce(Vector3.up * jumpSpd, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
            jump = false;
    }
}
