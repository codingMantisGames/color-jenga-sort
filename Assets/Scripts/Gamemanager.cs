using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    #region VARIABLES
    public static Gamemanager instance;
    [SerializeField] private List<Level> levels;
    [HideInInspector] public Level level;
    public float rotationThreshold = 1.0f;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        int lvl = PlayerPrefs.GetInt("Level");

        if (lvl >= levels.Count)
        {
            lvl = Random.Range(0, levels.Count - 1);
        }
        level = levels[lvl];

        level.gameObject.SetActive(true);
    }
    void Update()
    {

    }
    #endregion

    #region FUNCTIONS
    public void CheckForGameOver(Transform block)
    {
        Debug.LogWarning("Check for gameover!");

        float rotationX = block.eulerAngles.x;
        float rotationZ = block.eulerAngles.z;

        // Normalize rotations to the range [-180, 180]
        rotationX = NormalizeAngle(rotationX);
        rotationZ = NormalizeAngle(rotationZ);

        // Check if rotation exceeds the threshold
        if (Mathf.Abs(rotationX) > rotationThreshold || Mathf.Abs(rotationZ) > rotationThreshold)
        {
            Debug.LogWarning("Gameover!");
        }
    }
    bool IsAngleAccepted(float angle)
    {
        if (angle > 359f && angle < 1f)
            return true;
        return false;
    }
    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
    #endregion
}
