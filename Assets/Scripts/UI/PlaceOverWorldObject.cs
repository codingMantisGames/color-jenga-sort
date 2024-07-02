using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOverWorldObject : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private Transform refObject;
    private RectTransform rect;
    [SerializeField] private Vector3 offset;
    #endregion

    #region UNITY FUNCTIONS
    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
    }
    void Update()
    {
        rect.position = Camera.main.WorldToScreenPoint(refObject.position) + offset;
    }
    #endregion

    #region FUNCTIONS
    #endregion
}
