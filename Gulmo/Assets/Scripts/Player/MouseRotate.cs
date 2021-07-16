using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MouseRotate
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVR = true;
    public float MinimumX = -70f;
    public float MaximumX = 70f;
    public bool smooth = true;
    public float smoothTime = 5f;
    public bool lockCursor = true;
    public int viewType = 0;
    private Quaternion CharacterTargetRotate;
    private Quaternion CameraTargetRotate;
    private bool m_cursorIsLocked = false;
    private bool isMain;

    public void Init(Transform character, Transform camera)
    {
        CharacterTargetRotate = character.localRotation;
        CameraTargetRotate = camera.localRotation;
    }

    public void LookRotation(Transform character, Transform camera)
    {
        float yRotate = Input.GetAxis("Mouse X") * XSensitivity;
        float xRotate = Input.GetAxis("Mouse Y") * YSensitivity;

            CharacterTargetRotate *= Quaternion.Euler(0f, yRotate, 0f);
            CameraTargetRotate *= Quaternion.Euler(-xRotate, 0f, 0f);

        if (clampVR)
        {
            CameraTargetRotate = ClampRotationX(CameraTargetRotate);
        }

        if (smooth)
        {
                character.localRotation = Quaternion.Slerp(character.localRotation, CharacterTargetRotate,
                        smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, CameraTargetRotate,
                smoothTime * Time.deltaTime);         
        }
        else
        {
            character.localRotation = CharacterTargetRotate;
            camera.localRotation = CameraTargetRotate;
        }
    }
    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void UpdateCursorLock()
    {
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0) && !isMain)
            {
            if (!isMain)
            {
                m_cursorIsLocked = true;
            }
            }
            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            if(!isMain)
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
    }
    private Quaternion ClampRotationX(Quaternion quat)
    {
        quat.x /= quat.w;
        quat.y /= quat.w;
        quat.z /= quat.w;
        quat.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quat.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        quat.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return quat;
    }
    private Quaternion ClampRotationX2(Quaternion quat)
    {
        quat.x /= quat.w;
        quat.y /= quat.w;
        quat.z /= quat.w;
        quat.w = 1.0f;

        quat.y += 180;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quat.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        quat.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return quat;
    }
    public void MainChange(bool main)
    {
        isMain = main;
    }
}
