using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AdsHand : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image cursor;
    [SerializeField] private Sprite mouseDown;
    [SerializeField] private Sprite mouseUp;
    [SerializeField] private float easeTime;
    [SerializeField] private Ease ease;
    [SerializeField] private Image circle;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {

    }
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cursor.sprite = mouseDown;

            circle.transform.DOScale(1.5f, easeTime).SetEase(ease);
            circle.DOFade(0, easeTime).SetEase(ease).OnComplete(() =>
            {
                circle.transform.localScale = Vector3.zero;
                circle.color = Color.white;
            });
        }
        if (Input.GetMouseButtonUp(0))
        {
            cursor.sprite = mouseUp;
        }

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);
        rectTransform.localPosition = localPoint;
    }
    #endregion

    #region FUNCTIONS
    #endregion
}
