using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaHistoryHolder : MonoBehaviour
{
    #region VARIABLES
    public List<History> histories;
    public static JengaHistoryHolder instance;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        SaveHistory();
    }
    void Update()
    {

    }
    #endregion

    #region FUNCTIONS
    public void SaveHistory()
    {
        History history = new History();
        history.blockDatas = new List<BlockData>();

        for (int i = 0; i < transform.childCount; i++)
        {
            BlockData data = new BlockData();
            data.active = transform.GetChild(i).gameObject.activeSelf;
            data.pos = transform.GetChild(i).position;
            data.rot = transform.GetChild(i).rotation;

            history.blockDatas.Add(data);
        }

        histories.Add(history);
    }
    public void Undo()
    {
        if (histories.Count < 2)
            return;

        History history = histories[histories.Count - 1];
        histories.Remove(history);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = history.blockDatas[i].pos;
            transform.GetChild(i).rotation = history.blockDatas[i].rot;
            transform.GetChild(i).gameObject.SetActive(history.blockDatas[i].active);

            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).TryGetComponent<Block>(out Block block))
            {
                block.ResetBlock();
            }
        }
    }
    #endregion
}
[System.Serializable]
public class BlockData
{
    public Vector3 pos;
    public Quaternion rot;
    public bool active;
}
[System.Serializable]
public class History
{
    public List<BlockData> blockDatas;
}