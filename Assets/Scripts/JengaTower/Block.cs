using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    #region VARIABLES
    public Material blockMat;
    private MeshRenderer mesh;
    private Outline outline;

    [SerializeField] private bool isMuticolor = false;
    [SerializeField] private float moveTime;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float moveDistance;
    private bool isRemoved = false;
    #endregion

    #region UNITY FUNCTIONS
    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
        outline = gameObject.GetComponent<Outline>();
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
    public Color GetColor()
    {
        if (isMuticolor)
            return Color.black;

        if (mesh)
            return mesh.material.color;

        return Color.white;
    }
    public void Select()
    {
        outline.enabled = true;
    }
    public void Deselect()
    {
        outline.enabled = false;
    }
    public void ResetBlock()
    {
        outline.enabled = false;

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        isRemoved = false;
    }
    public void RemoveFromSelection(bool flag = false)
    {
        isRemoved = true;

        Vector3 currentPosition = transform.position;

        Vector3 pos = currentPosition + transform.right * moveDistance;

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (flag)
        {
            if (JengaHistoryHolder.instance)
                JengaHistoryHolder.instance.SaveHistory();
        }

        transform.DOMove(pos, moveTime).SetEase(moveEase).OnComplete(() =>
        {
            gameObject.SetActive(false);

            if (BlockRemoveLogic.instance)
                BlockRemoveLogic.instance.removedBlocks.Add(this);
        });
    }
    private void OnMouseDown()
    {
        if (isRemoved)
            return;

        if (BlockRemoveLogic.instance)
            BlockRemoveLogic.instance.OnClickedBlock(this);
    }
    public float RoundValue(float originalValue)
    {
        float roundedValue = Mathf.Round(originalValue * 100f) / 100f;

        return roundedValue;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.right * 5);
    }
    #endregion
}
