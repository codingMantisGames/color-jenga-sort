using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JengaAnimation : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private List<Vector3> blockPositions;
    [SerializeField] private List<Transform> blockTrans;
    [SerializeField] private float movementTime = 1;
    [SerializeField] private Ease movementEase;
    [SerializeField] private float timeDelay = 0.1f;
    [SerializeField] private float height;
    private JengaHistoryHolder historyHolder;
    int counter;
    #endregion

    #region UNITY FUNCTIONS
    void Start()
    {
        historyHolder = GetComponent<JengaHistoryHolder>();

        blockPositions = new List<Vector3>();
        blockTrans = new List<Transform>();
        counter = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            blockPositions.Add(transform.GetChild(i).position);
            blockTrans.Add(transform.GetChild(i));
            if (transform.GetChild(i).TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = blockPositions[i];
            pos.y += height;
            transform.GetChild(i).position = pos;
        }

        StartCoroutine(MoveAllBlocks());
    }
    void Update()
    {

    }
    #endregion

    #region FUNCTIONS
    IEnumerator MoveAllBlocks()
    {
        int i = 0;
        foreach (var item in blockTrans)
        {
            item.gameObject.SetActive(true);
            item.DOMove(blockPositions[i++], movementTime).SetEase(movementEase).OnComplete(() =>
            {
                if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
                counter++;

                if (item.TryGetComponent<Block>(out Block blk))
                {
                    blk.PlayAudio();
                }

                if (Gamemanager.instance.canVibrate)
                    Taptic.Medium();

                if(counter == blockPositions.Count)
                {
                    StartGameNow();
                }
            });
            yield return new WaitForSeconds(timeDelay);
        }
    }
    public void StartGameNow()
    {
        foreach (var item in blockTrans)
        {
            if (item.TryGetComponent<Block>(out Block blk))
            {
                blk.enabled = true;
            }
        }

        historyHolder.SaveHistory();
    }
    #endregion
}
