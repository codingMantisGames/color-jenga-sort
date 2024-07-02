using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TapToOpen : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private GameObject tapToContinueButton;
    [SerializeField] private GameObject glow;
    [SerializeField] private GameObject coin;
    [SerializeField] private int tapCount;
    [SerializeField] private float scale;
    [SerializeField] private float easeTime;
    [SerializeField] private Ease ease;
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

    public void Grow()
    {
        transform.DOScale(scale, easeTime / 2).SetEase(ease).OnComplete(() =>
          {
              transform.DOScale(1, easeTime / 2).SetEase(ease).OnComplete(() =>
              {
                  tapCount--;

                  if (tapCount == 0)
                  {
                      glow.SetActive(false);
                      gameObject.SetActive(false);

                      coin.gameObject.SetActive(true);
                      coin.transform.DOScale(1, 0.5f).SetEase(Ease.Flash);

                      Invoke("ShowTapToContButton", 2);
                  }
              });
          });
    }
    void ShowTapToContButton()
    {
        tapToContinueButton.SetActive(true);

        if (Gamemanager.instance)
            Gamemanager.instance.RemoveGift();
    }
    #endregion
}
