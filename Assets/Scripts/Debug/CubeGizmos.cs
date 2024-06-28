using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGizmos : MonoBehaviour
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
    void OnDrawGizmos()
    {
        // Get the transform's position and scale
        Vector3 position = transform.position;
        Vector3 scale = transform.localScale;

        // Set the color of the Gizmo
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawCube(position, scale);
    }
    #endregion
}
