using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CameraMovementController : MonoBehaviour
{
    #region VARIABLES
    public CinemachineFreeLook freeLookCamera;
    public float touchSensitivityX = 10f;
    public float touchSensitivityY = 0.1f;

    private Vector2 previousTouchPosition;
    private bool isDragging;
    public static CameraMovementController instance;
    public bool isEnabled;
    [Header("Events")]
    public UnityEvent OnClick;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        instance = this;

        isEnabled = true;
    }
    void Start()
    {

    }
    void Update()
    {
        /*if (isEnabled && Input.touchCount > 0 && !IsPointerOverUIElement())
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

                OnClick.Invoke();
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }*/
        if (isEnabled && Input.GetMouseButton(0) && !IsPointerOverUIElement())
        {
            Vector2 currentMousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                previousTouchPosition = currentMousePosition;
                isDragging = true;
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector2 mouseDelta = (Vector2)currentMousePosition - previousTouchPosition;

                freeLookCamera.m_XAxis.Value += mouseDelta.x * touchSensitivityX * Time.deltaTime;
                freeLookCamera.m_YAxis.Value += mouseDelta.y * touchSensitivityY * Time.deltaTime * -1;

                freeLookCamera.m_YAxis.Value = Mathf.Clamp01(freeLookCamera.m_YAxis.Value);

                previousTouchPosition = currentMousePosition;

                OnClick.Invoke();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

    }
    #endregion

    #region FUNCTIONS
    public static bool IsPointerOverUIElement()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
    #endregion
}
