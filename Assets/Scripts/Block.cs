using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    #region VARIABLES
    public Material blockMat;
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
    public void UpdateMat(Material mat)
    {
        if (gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            meshRenderer.material = mat;
            blockMat = mat;
        }
    }
    #endregion
}
