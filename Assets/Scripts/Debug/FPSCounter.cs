using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    #region VARIABLES
    public float updateInterval = 0.5f;
    private float accumulatedFPS = 0f;
    private int frames = 0;
    private float timeleft;
    private TMP_Text textMeshPro;
    #endregion

    #region UNITY FUNCTIONS
    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component not found.");
            enabled = false;
            return;
        }
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accumulatedFPS += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            float fps = accumulatedFPS / frames;
            textMeshPro.text = $"FPS: {fps:F2}";

            timeleft = updateInterval;
            accumulatedFPS = 0.0f;
            frames = 0;
        }
    }
    #endregion

    #region FUNCTIONS

    #endregion
}
