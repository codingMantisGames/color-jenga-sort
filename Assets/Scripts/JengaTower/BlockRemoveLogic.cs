using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRemoveLogic : MonoBehaviour
{
    #region VARIABLES
    [HideInInspector] public Color selectedColor;
    public static BlockRemoveLogic instance;
    [HideInInspector] public Block oldBlock;
    [HideInInspector] public List<Block> removedBlocks;
    [SerializeField] private Transform mainParent;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        removedBlocks = new List<Block>();
    }
    void Update()
    {

    }
    #endregion

    #region FUNCTIONS
    public void OnClickedBlock(Block block)
    {
        if (selectedColor == Color.white)
        {
            selectedColor = block.GetColor();
            oldBlock = block;
            block.Select();
        }
        else if (selectedColor == Color.black || block.GetColor() == Color.black)
        {
            block.Select();
            oldBlock.RemoveFromSelection();
            block.RemoveFromSelection(true);
            selectedColor = Color.white;
        }
        else if (block == oldBlock)
        {
            selectedColor = Color.white;
            block.Deselect();
        }
        else if (selectedColor != block.GetColor())
        {
            oldBlock.Deselect();
            oldBlock = block;
            selectedColor = block.GetColor();
            block.Select();
        }
        else
        {
            block.Select();
            oldBlock.RemoveFromSelection();
            block.RemoveFromSelection(true);
            selectedColor = Color.white;
        }


    }

    public void Undo()
    {
        if (JengaHistoryHolder.instance)
            JengaHistoryHolder.instance.Undo();
    }
    #endregion
}
