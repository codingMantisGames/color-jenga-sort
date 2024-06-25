using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovementController : MonoBehaviour
{
    #region VARIABLES
    public CinemachineFreeLook freeLookCamera;
    public float touchSensitivityX = 10f;
    public float touchSensitivityY = 0.1f;

    private Vector2 previousTouchPosition;
    private bool isDragging;
    #endregion

    #region UNITY FUNCTIONS
    void Start()
    {

    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                previousTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 touchDelta = touch.position - previousTouchPosition;

                freeLookCamera.m_XAxis.Value += touchDelta.x * touchSensitivityX * Time.deltaTime;
                freeLookCamera.m_YAxis.Value += touchDelta.y * touchSensitivityY * Time.deltaTime * -1;

                freeLookCamera.m_YAxis.Value = Mathf.Clamp01(freeLookCamera.m_YAxis.Value);

                previousTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }
    #endregion

    #region FUNCTIONS

    #endregion
}
