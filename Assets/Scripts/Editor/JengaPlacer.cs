using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class JengaPlacer : EditorWindow
{
    #region VARIABLES
    private Transform mainParent;
    private GameObject prefab;
    private LevelData levelData;
    Vector3 currentPosition;
    private Material selectedMat;
    int index = 0;
    List<Transform> blocks;
    bool isEnabled = false;
    float yOffset;
    float xOffset;
    #endregion

    #region UNITY FUNCTIONS
    [MenuItem("Tools/Jenga Tower Editor")]
    public static void ShowWindow()
    {
        GetWindow<JengaPlacer>("Jenga Tower Editor");
    }
    private void OnGUI()
    {
        if (GUILayout.Button(isEnabled ? "Disable" : "Enable"))
        {
            isEnabled = !isEnabled;
        }

        if (!isEnabled)
        {
            GUILayout.Label("Please Enable Editor", EditorStyles.boldLabel);
            return;
        }
        GUILayout.Label("Jenga Tower Editor", EditorStyles.boldLabel);

        mainParent = (Transform)EditorGUILayout.ObjectField("Jenga Tower Root", mainParent, typeof(Transform), true);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);
        GUILayout.Label("Properties", EditorStyles.boldLabel);
        yOffset = EditorGUILayout.FloatField("Y Offset", yOffset);
        xOffset = EditorGUILayout.FloatField("Z Offset", xOffset);

        if (levelData)
        {
            DrawColorButtons();
        }
        if (GUILayout.Button("Reset"))
        {
            currentPosition = mainParent.position + new Vector3(0, yOffset, 0);
            blocks = new List<Transform>();
            index = 0;

            for (int i = 0; i < mainParent.childCount; i++)
            {
                DestroyAllChildren(mainParent);
            }
        }
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        if (!isEnabled)
        {
            return;
        }

        if (prefab == null) return;

        MeshRenderer meshRenderer = prefab.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Bounds bounds = meshRenderer.bounds;
            Vector3 center = currentPosition;
            Vector3 size = bounds.size;
            Vector3[] rectVertices = new Vector3[4]
            {
                new Vector3(center.x - size.x / 2, center.y, center.z - size.z / 2),
                new Vector3(center.x - size.x / 2, center.y, center.z + size.z / 2),
                new Vector3(center.x + size.x / 2, center.y, center.z + size.z / 2),
                new Vector3(center.x + size.x / 2, center.y, center.z - size.z / 2)
            };

            Handles.color = new Color(1, 1, 1, 0.5f);
            Handles.DrawSolidRectangleWithOutline(rectVertices, new Color(1, 1, 1, 0.5f), Color.black);

            HandleClick(rectVertices);
        }

        SceneView.RepaintAll();
    }
    #endregion

    #region FUNCTIONS
    public void DestroyAllChildren(Transform parent)
    {
        // Iterate through all child objects of the parent transform
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            // Destroy the child GameObject at index i
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }
    private void DrawColorButtons()
    {
        GUILayout.Label("Select block Color", EditorStyles.boldLabel);

        if (levelData.jengaColors == null)
        {
            EditorGUILayout.HelpBox("No mat.colors defined in Level Data.", MessageType.Warning);
            return;
        }

        EditorGUILayout.BeginHorizontal();
        foreach (Material mat in levelData.jengaColors)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

            // Create a texture with a white border for selected mat.color
            if (mat != selectedMat)
            {
                Texture2D tex = MakeTexWithBorder(50, 50, mat.color, Color.white, 5);
                buttonStyle.normal.background = tex;
            }
            else
            {
                buttonStyle.normal.background = MakeTex(2, 2, mat.color);
            }

            if (GUILayout.Button("", buttonStyle, GUILayout.Width(50), GUILayout.Height(50)))
            {
                selectedMat = mat;
                //ApplyColorToPrefab();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private Texture2D MakeTexWithBorder(int width, int height, Color fillColor, Color borderColor, int borderWidth)
    {
        Texture2D tex = new Texture2D(width, height);

        // Fill texture with fill mat.color
        Color[] fillColorArray = tex.GetPixels();
        for (int i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = fillColor;
        }
        tex.SetPixels(fillColorArray);

        // Add border
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < borderWidth; ++j)
            {
                tex.SetPixel(i, j, borderColor); // Top border
                tex.SetPixel(i, height - 1 - j, borderColor); // Bottom border
            }
        }
        for (int j = 0; j < height; ++j)
        {
            for (int i = 0; i < borderWidth; ++i)
            {
                tex.SetPixel(i, j, borderColor); // Left border
                tex.SetPixel(width - 1 - i, j, borderColor); // Right border
            }
        }

        tex.Apply();
        return tex;
    }
    private void HandleClick(Vector3[] rectVertices)
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            Plane plane = new Plane(Vector3.up, rectVertices[0]);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                if (IsPointInRectangle(hitPoint, rectVertices))
                {
                    HandleClicked();
                    e.Use();
                }
            }
        }
    }
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    private bool IsPointInRectangle(Vector3 point, Vector3[] rectVertices)
    {
        float minX = Mathf.Min(rectVertices[0].x, rectVertices[2].x);
        float maxX = Mathf.Max(rectVertices[0].x, rectVertices[2].x);
        float minZ = Mathf.Min(rectVertices[0].z, rectVertices[2].z);
        float maxZ = Mathf.Max(rectVertices[0].z, rectVertices[2].z);

        return point.x >= minX && point.x <= maxX && point.z >= minZ && point.z <= maxZ;
    }

    private void HandleClicked()
    {
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.transform.position = currentPosition;
        blocks.Add(instance.transform);
        instance.transform.parent = mainParent;
        if (instance.TryGetComponent<Block>(out Block block))
        {
            block.UpdateMat(selectedMat);
        }
        index++;
        MeshRenderer meshRenderer = prefab.GetComponent<MeshRenderer>();
        Bounds bounds = meshRenderer.bounds;
        Vector3 size = bounds.size;
        if (index == 1)
        {
            currentPosition.z += (size.z + xOffset);
        }
        else if (index == 2)
        {
            currentPosition.z -= ((size.z + xOffset) * 2);
        }
        else if (index == 3)
        {
            currentPosition.y += (size.y + yOffset);
            index = 0;
            currentPosition.z = mainParent.position.z;

            Vector3 rot = mainParent.transform.rotation.eulerAngles;
            if (rot.y == 90)
            {
                rot.y = 0;
            }
            else
            {
                rot.y = 90;
            }
            mainParent.transform.rotation = Quaternion.Euler(rot);
        }
    }

    private void OnEnable()
    {
        currentPosition = mainParent.position + new Vector3(0, yOffset, 0);
        blocks = new List<Transform>();
        index = 0;
        selectedMat = levelData.jengaColors[0];
        SceneView.duringSceneGui += OnSceneGUI;

    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    #endregion
}
