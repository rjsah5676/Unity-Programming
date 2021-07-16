using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControll : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;
    private Rigidbody rd;

    private Camera camera_1;
    private Camera camera_2;
    private Camera camera_3;
    private Camera mainCamera;
    private Camera stopCamera;

    public Texture2D cursorTexture;
    public Texture2D cursorTextureEntered;
    private Vector2 hotSpot;

    private float h = 0.0f;    //수평
    private float v = 0.0f;    //수직
    private int ViewType = 0;
    private int stopCnt = 0;
    private int dmgCnt = 0;
    private int forceType = 0;
    private bool rayStart = false;
    private bool rayExit = false;
    private bool raySetting = false;

    public Ray ray;
    public RaycastHit hitinfo;
    private float jumpSpd = 5.0f;
    public float moveSpd = 5.0f;
    public float rotateSpd = 100.0f;
    public bool jump = false;
    public bool isMain;
    public bool isStop;
    private bool gz1_Damage = false;

    [SerializeField] private MouseRotate m_MouseRotate;


    // Start is called before the first frame update
    void Start()
    {
        hotSpot.x = cursorTexture.width / 2;
        hotSpot.y = cursorTexture.height / 2;
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        camera_1 = GameObject.Find("Main Camera").GetComponent<Camera>();
        camera_2 = GameObject.Find("Camera").GetComponent<Camera>();
        camera_3 = GameObject.Find("Camera_2").GetComponent<Camera>();
        mainCamera = GameObject.Find("Camera_MENU").GetComponent<Camera>();
        stopCamera = GameObject.Find("Stop Camera").GetComponent<Camera>();
        m_MouseRotate.Init(transform, camera_1.transform);
        rd = GetComponent<Rigidbody>();

        isMain = true;
        m_MouseRotate.MainChange(isMain);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gz1_Damage)
        {
            dmgCnt++;
            if (dmgCnt > 150)
            {
                dmgCnt = 0;
                gz1_Damage = false;
            }
            if (dmgCnt > 50)
            {
                if (forceType == 0)
                {
                    rd.AddForce(Vector3.forward * 10, ForceMode.Impulse);
                    forceType = -1;
                }
                if (forceType == 1)
                {
                    rd.AddForce(Vector3.left * 10, ForceMode.Impulse);
                    forceType = -1;
                }
                if (forceType == 2)
                {
                    rd.AddForce(Vector3.right * 10, ForceMode.Impulse);
                    forceType = -1;
                }
            }
        }
        if (isMain)
        {
            CameraOn(0);
            stopCamera.enabled = false;
            mainCamera.enabled = true;
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            MenuButtonClick("startButton", "startButton_Click", "exitButton", "exitButton_Click", "settingButton", "settingButton_Click", 60f);
        }
        else if (isStop)
        {
            stopCnt++;
            ray = stopCamera.ScreenPointToRay(Input.mousePosition);
            MenuButtonClick("backButton_stop", "backButtonClick_stop","exitButton_stop", "exitButtonClick_stop", "settingButton_stop", "settingButtonClick_stop", 60f);
            if (Input.GetKeyDown(KeyCode.Escape) && stopCnt > 3)
            {
                stopCamera.enabled = false;
                if (ViewType % 3 == 1)
                {
                    CameraOn(3);
                }
                if (ViewType % 3 == 2)
                {
                    CameraOn(2);
                }
                if (ViewType % 3 == 0)
                {
                    CameraOn(1);
                }
                isStop = false;
                m_MouseRotate.MainChange(isStop);
                stopCnt = 0;
            }
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            if (ViewType % 3 == 0 || ViewType % 3 == 2)
                RotateView(camera_1);
            else if (ViewType % 3 == 1)
                RotateView(camera_3);
            if (Input.GetKeyDown(KeyCode.Escape))
           {
                isStop = true;
                m_MouseRotate.MainChange(isStop);
                CameraOn(0);
                stopCamera.enabled = isStop;
            }


            if (!gz1_Damage)
            {
                Vector3 moveDirect = (Vector3.forward * v) + (Vector3.right * h);
                
                _transform.Translate(moveDirect.normalized * Time.deltaTime * moveSpd, Space.Self);
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        _animator.SetBool("IsRun_F", true);
                        moveSpd = 10f;
                        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                            _animator.SetBool("IsRun_F", false);
                    }                    
                }
                if (Input.GetKeyUp(KeyCode.E))
                {
                    _animator.SetBool("IsRun_F", false);
                    moveSpd = 5f;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!jump)
                        _animator.Play("Jab", -1, 0);
                    else
                        _animator.SetBool("IsJab", jump);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (!jump)
                        _animator.Play("Hikick", -1, 0);
                    else
                        _animator.SetBool("IsKick", jump);
                }



                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                    moveSpd = 2.5f;
                if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
                    moveSpd = 5f;

                if (jump == false)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        jump = true;
                        Jump();
                        _animator.Play("JUMP00", 0, 0);
                    }
                }
                if (v >= 0.1f)
                {
                    _animator.SetBool("IsWalk_F", true);
                    if (h == 0)
                    {
                        _animator.SetBool("IsWalk_L", false);
                        _animator.SetBool("IsWalk_R", false);
                    }
                    if (h >= 0.1f)
                    {
                        _animator.SetBool("IsWalk_R", true);
                    }
                    if (h <= -0.1f)
                    {
                        _animator.SetBool("IsWalk_L", true);
                    }
                }
                else if (v <= -0.1f)
                {
                    _animator.SetBool("IsWalk_B", true);
                    moveSpd = 2.5f;
                    if (h == 0)
                    {
                        _animator.SetBool("IsWalk_L", false);
                        _animator.SetBool("IsWalk_R", false);
                    }
                }
                else if (h >= 0.1f)
                {
                    _animator.SetBool("IsWalk_R", true);
                    if (v <= -0.1f)
                    {
                        _animator.SetBool("IsWalk_B", true);
                    }
                    if (v == 0)
                    {
                        _animator.SetBool("IsWalk_F", false);
                        _animator.SetBool("IsWalk_B", false);
                    }
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        _animator.SetBool("IsRun_F", false);
                        moveSpd = 5f;
                    }
                }
                else if (h <= -0.1f)
                {
                    _animator.SetBool("IsWalk_L", true);
                    if (v <= -0.1f)
                    {
                        _animator.SetBool("IsWalk_B", true);
                    }
                    if (v == 0)
                    {
                        _animator.SetBool("IsWalk_F", false);
                        _animator.SetBool("IsWalk_B", false);
                    }
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        _animator.SetBool("IsRun_F", false);
                        moveSpd = 5f;
                    }
                }
                else
                {
                    _animator.SetBool("IsWalk_F", false);
                    _animator.SetBool("IsWalk_B", false);
                    _animator.SetBool("IsWalk_L", false);
                    _animator.SetBool("IsWalk_R", false);
                    _animator.SetBool("IsRun_F", false);
                        moveSpd = 5f;
                }
            }
        }
    }

    private void Update()
    {
        m_MouseRotate.UpdateCursorLock();
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (ViewType % 3 == 0)
            {
                CameraOn(3);
                GameObject.Find("unitychan").transform.Translate(0, 0, 300f);   //뷰타입 1인칭시 안보이게
            }
            if (ViewType % 3 == 1)
            {
                CameraOn(2);
                GameObject.Find("unitychan").transform.Translate(0, 0, -300f);
            }
            if (ViewType % 3 == 2)
            {
                CameraOn(1);
            }
            ViewType++;
        }
    }

    private void RotateView(Camera camera)
    {
            m_MouseRotate.LookRotation(transform, camera.transform);
    }
    private void Jump()
    {
        rd.AddForce(Vector3.up * jumpSpd, ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            jump = false;
            _animator.SetBool("IsJab", jump);
            _animator.SetBool("IsKick", jump); 
        }
        if (coll.gameObject.name == "GameZone1")
        {
            this.transform.position = new Vector3(1000f, 809, -303);    //게임존 입장.
        }
        if (coll.gameObject.name == "Akiba")
        {
            this.transform.position = new Vector3(-1450f, -570, 1109);    //아키하바라 입장
        }
        if (coll.gameObject.tag == "gz1_Ball" && !gz1_Damage)
        {
            //this.transform.Translate(0, 0, -20f);
            if (this.transform.rotation.y > 90 && this.transform.rotation.y < 180)
            {
                rd.AddForce(Vector3.right * 10, ForceMode.Impulse);
                forceType = 1;
            }
            else if (this.transform.rotation.y < -90 && this.transform.rotation.y > -180)
            {
                rd.AddForce(Vector3.left * 10, ForceMode.Impulse);
                forceType = 2;
            }
            else
            {
                rd.AddForce(Vector3.back * 10, ForceMode.Impulse);
                forceType = 0;
            }
            _animator.Play("DAMAGED", 0, 0);
            gz1_Damage = true;
        }
    }
    private void MenuButtonClick(string whatButton_1, string whatButton_1_click, string whatButton_2, string whatButton_2_click, string whatButton_3, string whatButton_3_click, float a)
    {
        if (Physics.Raycast(ray, out hitinfo))
        {
                if (hitinfo.transform.gameObject.name == whatButton_1)
                {
                    GameObject.Find(whatButton_1).transform.Translate(0, 0f, a);
                    GameObject.Find(whatButton_1_click).transform.Translate(0f, 0f, -a);
                    Cursor.SetCursor(cursorTextureEntered, hotSpot, CursorMode.Auto);
                    rayStart = true;
                }
                else if (hitinfo.transform.gameObject.name != whatButton_1_click && rayStart)
                {
                    GameObject.Find(whatButton_1).transform.Translate(0f, 0f, -a);
                    GameObject.Find(whatButton_1_click).transform.Translate(0f, 0f, a);
                Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
                rayStart = false;
                }
                if (hitinfo.transform.gameObject.name == whatButton_2)
                {
                    GameObject.Find(whatButton_2).transform.Translate(0, 0f, a);
                    GameObject.Find(whatButton_2_click).transform.Translate(0f, 0f, -a);
                Cursor.SetCursor(cursorTextureEntered, hotSpot, CursorMode.Auto);
                rayExit = true;
                }
                else if (hitinfo.transform.gameObject.name != whatButton_2_click && rayExit)
                {
                    GameObject.Find(whatButton_2).transform.Translate(0f, 0f, -a);
                    GameObject.Find(whatButton_2_click).transform.Translate(0f, 0f, a);
                Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
                rayExit = false;
                }
                if (hitinfo.transform.gameObject.name == whatButton_3)
                {
                    GameObject.Find(whatButton_3).transform.Translate(0, 0f, a);
                    GameObject.Find(whatButton_3_click).transform.Translate(0f, 0f, -a);
                Cursor.SetCursor(cursorTextureEntered, hotSpot, CursorMode.Auto);
                raySetting = true;
                }
                else if (hitinfo.transform.gameObject.name != whatButton_3_click && raySetting)
                {
                    GameObject.Find(whatButton_3).transform.Translate(0f, 0f, -a);
                    GameObject.Find(whatButton_3_click).transform.Translate(0f, 0f, a);
                Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
                raySetting = false;
                }
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hitinfo))
                {
                    if (hitinfo.transform.gameObject.name == whatButton_1_click)
                    {
                        GameObject.Find(whatButton_1).transform.Translate(0f, 0f, -a);
                        GameObject.Find(whatButton_1_click).transform.Translate(0f, 0f, a);
                        isMain = false;
                        isStop = false;
                        m_MouseRotate.MainChange(isMain);
                        mainCamera.enabled = false;
                        stopCamera.enabled = false;
                        if (ViewType % 3 == 1)
                        {
                            CameraOn(3);
                        }
                        if (ViewType % 3 == 2)
                        {
                            CameraOn(2);
                        }
                        if (ViewType % 3 == 0)
                        {
                            CameraOn(1);
                        }
                        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
                        rayStart = false;
                        stopCnt = 0;
                    }
                    if (hitinfo.transform.gameObject.name == whatButton_2_click)
                    {
                        Application.Quit();
                    }
                    if (hitinfo.transform.gameObject.name == whatButton_3_click)
                    {
                        //환경설정 이벤트
                    }
                }
            }
        }
    }
    private void CameraOn(int i)
    {
        if (i == 0)
        {
            camera_1.enabled = false;
            camera_2.enabled = false;
            camera_3.enabled = false;
        }
        if (i == 1)
        {
            camera_1.enabled = true;
            camera_2.enabled = false;
            camera_3.enabled = false;
        }
        if(i == 2)
        {
            camera_1.enabled = false;
            camera_2.enabled = true;
            camera_3.enabled = false;
        }
        if(i == 3)
        {
            camera_1.enabled = false;
            camera_2.enabled = false;
            camera_3.enabled = true;
        }
    }
}
