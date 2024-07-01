using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelAnimation : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private Ease ease;
    [SerializeField] private float movTime;
    [SerializeField] private List<AnimatedObject> animatedObjects;
    public bool startHidden;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        if(startHidden)
        {
            foreach (var item in animatedObjects)
            {
                item.rect.anchoredPosition = item.localAnchoredHiddenPosition;
            }
        }
    }
    void Start()
    {
	
    }
    void Update()
    {
	
    }
    #endregion

    #region FUNCTIONS
    public void Hide()
    {
        foreach (var item in animatedObjects)
        {
            item.rect.DOAnchorPos(item.localAnchoredHiddenPosition,movTime).SetEase(ease);
        }
    }
    public void Show()
    {
        foreach (var item in animatedObjects)
        {
            item.rect.DOAnchorPos(item.localAnchoredNormalPosition, movTime).SetEase(ease);
        }
    }
    public void Show(int num)
    {
        var item = animatedObjects[num];
        item.rect.DOAnchorPos(item.localAnchoredNormalPosition, movTime).SetEase(ease);
    }
    public void Hide(int num)
    {
        var item = animatedObjects[num];
        item.rect.DOAnchorPos(item.localAnchoredHiddenPosition, movTime).SetEase(ease);
    }
    #endregion
}
[System.Serializable]
public class AnimatedObject
{
    public RectTransform rect;
    public Vector3 localAnchoredNormalPosition;
    public Vector3 localAnchoredHiddenPosition;
}