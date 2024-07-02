using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVibration : MonoBehaviour
{
    #region VARIABLES
    #endregion

    #region UNITY FUNCTIONS
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion

    #region FUNCTIONS
    public void PlayVibration()
    {
        if (Gamemanager.instance.canVibrate)
            Taptic.Light();
    }
    #endregion
}
