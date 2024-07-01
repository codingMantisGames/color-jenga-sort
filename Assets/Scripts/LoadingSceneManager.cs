using SupersonicWisdomSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    void Awake()
    {
        SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
        SupersonicWisdom.Api.Initialize();
    }


    void OnSupersonicWisdomReady()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
