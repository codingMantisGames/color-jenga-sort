using SupersonicWisdomSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Image slider;
    void Awake()
    {
        SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
        SupersonicWisdom.Api.Initialize();

        slider.DOFillAmount(0.6f, 1);
    }


    void OnSupersonicWisdomReady()
    {
        slider.DOFillAmount(1f, 1).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(1);
        });
    }
}
