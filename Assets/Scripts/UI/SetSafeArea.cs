using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSafeArea : MonoBehaviour
{
    #region VARIABLES
    private RectTransform safeAreaRect;
    private Canvas canvas;
    private Rect lastSafeArea;
    [SerializeField] bool useSafeArea;
    #endregion

    #region START AWAKE
    void Start()
    {
        if (Application.isPlaying && Application.platform == RuntimePlatform.Android)
        {
            useSafeArea = true;
        }

        safeAreaRect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        OnRectTransformDimensionsChange();
    }
    #endregion

    #region FUNCTIONS
    private void OnRectTransformDimensionsChange()
    {

        if (GetSafeArea() != lastSafeArea && canvas != null)
        {
            lastSafeArea = GetSafeArea();
            UpdateSizeToSafeArea();
        }
    }

    private void UpdateSizeToSafeArea()
    {

        var safeArea = GetSafeArea();
        var inverseSize = new Vector2(1f, 1f) / canvas.pixelRect.size;
        var newAnchorMin = Vector2.Scale(safeArea.position, inverseSize);
        var newAnchorMax = Vector2.Scale(safeArea.position + safeArea.size, inverseSize);

        safeAreaRect.anchorMin = newAnchorMin;
        safeAreaRect.anchorMax = newAnchorMax;

        safeAreaRect.offsetMin = Vector2.zero;
        safeAreaRect.offsetMax = Vector2.zero;
    }

    private Rect GetSafeArea()
    {
        if (!useSafeArea)
            return new Rect(0, 0, 1080, 1920);
        else
            return Screen.safeArea;
    }
    #endregion
}
