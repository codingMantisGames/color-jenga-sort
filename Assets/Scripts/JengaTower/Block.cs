using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Block : MonoBehaviour
{
    #region VARIABLES
    public Material blockMat;
    private MeshRenderer mesh;
    private Outline outline;

    [SerializeField] private bool isMuticolor = false;
    [SerializeField] private bool isGift = false;
    [SerializeField] private float moveTime;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float moveDistance;
    private bool isRemoved = false;
    [SerializeField] private AudioSource clickedAudio;
    [SerializeField] private AudioSource placedAudio;
    public enum MovementDirection { LEFT, RIGHT, FORWARD, BACKWARD };
    [SerializeField] private MovementDirection movementDirection= MovementDirection.RIGHT;
    [Header("Events")]
    public UnityEvent OnClick;
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

        Vector3 pos = currentPosition + GetDirection() * moveDistance;

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

            if (Gamemanager.instance)
            {
                Gamemanager.instance.CheckForGameWin();
                if (isGift)
                {
                    Gamemanager.instance.AddGift();
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }

        });
    }
    private void OnMouseDown()
    {
        if (!this.enabled)
            return;

        if (isRemoved || IsPointerOverUIElement())
            return;

        OnClick.Invoke();

        clickedAudio.Play();

        if (Gamemanager.instance.canVibrate)
            Taptic.Light();

        if (BlockRemoveLogic.instance)
            BlockRemoveLogic.instance.OnClickedBlock(this);
    }
    public Vector3 GetRoationValue()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        rot.x = RoundValue(rot.x);
        rot.y = RoundValue(rot.y);
        rot.z = RoundValue(rot.z);

        return rot;
    }
    public float RoundValue(float originalValue)
    {
        float roundedValue = Mathf.Round(originalValue * 100f) / 100f;

        return roundedValue;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, GetDirection() * 5);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isRemoved && collision.transform.tag == "Block")
        {
            if (collision.transform.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddForce(1f * Vector3.down);
            }
        }
        if (collision.gameObject.transform.tag == "Ground")
        {
            if (Gamemanager.instance)
                Gamemanager.instance.CheckForGameOver(transform);
        }
    }

    public static bool IsPointerOverUIElement()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
    public void PlayAudio()
    {
        clickedAudio.Play();
    }
    Vector3 GetDirection()
    {
        Vector3 dir = transform.forward;
        switch (movementDirection)
        {
            case MovementDirection.LEFT:
                dir = transform.right * -1;
                break;
            case MovementDirection.RIGHT:
                dir = transform.right;
                break;
            case MovementDirection.FORWARD:
                dir = transform.forward;
                break;
            case MovementDirection.BACKWARD:
                dir = transform.forward * -1;
                break;
        }

        return dir;
    }
    #endregion
}
