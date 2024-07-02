using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private CameraMovementController cameraMovement;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        if(PlayerPrefs.GetInt("Level") == 0)
        {
            cameraMovement.enabled = false;
            tutorialPanel.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
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
    #endregion
}
