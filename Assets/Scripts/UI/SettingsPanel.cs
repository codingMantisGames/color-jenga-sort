using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<GameObject> settingsButtons;
    private bool isEnabled = false;
    private Coroutine coroutine;

    [SerializeField, Space(20)] private Image musicButton;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    private bool isMusicOn = true;

    [SerializeField, Space(20)] private Image sfxButton;
    [SerializeField] private Sprite sfxOnSprite;
    [SerializeField] private Sprite sfxOffSprite;
    private bool isSFXon = true;

    [SerializeField, Space(20)] private Image vibrateButton;
    [SerializeField] private Sprite vibrateOnSprite;
    [SerializeField] private Sprite vibrateOffSprite;
    private bool isVibrateOn = true;
    #endregion

    #region UNITY FUNCTIONS
    void Start()
    {
        int i = PlayerPrefs.GetInt("music");
        isMusicOn = i == 0 ? true : false;
        i = PlayerPrefs.GetInt("sfx");
        isSFXon = i == 0 ? true : false;
        i = PlayerPrefs.GetInt("vibrate");
        isVibrateOn = i == 0 ? true : false;

        if (isMusicOn)
        {
            audioMixer.SetFloat("MusicVol", 0f);
            musicButton.sprite = musicOnSprite;
        }
        else
        {
            audioMixer.SetFloat("MusicVol", -80f);
            musicButton.sprite = musicOffSprite;
        }

        if (isSFXon)
        {
            sfxButton.sprite = sfxOnSprite;
            audioMixer.SetFloat("SFXVol", 0f);
        }
        else
        {
            audioMixer.SetFloat("SFXVol", -80f);
            sfxButton.sprite = sfxOffSprite;
        }

        if (isVibrateOn)
            vibrateButton.sprite = vibrateOnSprite;
        else
            vibrateButton.sprite = vibrateOffSprite;

        if (Gamemanager.instance)
        {
            Gamemanager.instance.canVibrate = isVibrateOn;
        }
    }
    void Update()
    {

    }
    #endregion

    #region FUNCTIONS
    public void ShowButtons()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(ShowButtonProcedure());
    }
    IEnumerator ShowButtonProcedure()
    {
        foreach (var item in settingsButtons)
        {
            item.SetActive(!isEnabled);

            yield return new WaitForSeconds(0.05f);
        }

        isEnabled = !isEnabled;
    }
    public void MusicToogle()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
        {
            audioMixer.SetFloat("MusicVol", 0f);
            musicButton.sprite = musicOnSprite;
        }
        else
        {
            audioMixer.SetFloat("MusicVol", -80f);
            musicButton.sprite = musicOffSprite;
        }

        PlayerPrefs.SetInt("music", isMusicOn ? 0 : 1);
        PlayerPrefs.Save();
    }
    public void SFXToogle()
    {
        isSFXon = !isSFXon;

        if (isSFXon)
        {
            sfxButton.sprite = sfxOnSprite;
            audioMixer.SetFloat("SFXVol", 0f);
        }
        else
        {
            audioMixer.SetFloat("SFXVol", -80f);
            sfxButton.sprite = sfxOffSprite;
        }

        PlayerPrefs.SetInt("sfx", isSFXon ? 0 : 1);
        PlayerPrefs.Save();
    }
    public void VibrateToogle()
    {
        isVibrateOn = !isVibrateOn;

        if (isVibrateOn)
            vibrateButton.sprite = vibrateOnSprite;
        else
            vibrateButton.sprite = vibrateOffSprite;

        if (Gamemanager.instance)
        {
            Gamemanager.instance.canVibrate = isVibrateOn;
        }

        PlayerPrefs.SetInt("vibrate", isVibrateOn ? 0 : 1);
        PlayerPrefs.Save();
    }
    #endregion
}
