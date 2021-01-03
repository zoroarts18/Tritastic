
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BackGroundMusicManagement : MonoBehaviour
{
    public AudioManager Audio;
    public bool muted;
    public bool vibrationMuted;
    public Button muteButton;
    public Button muteButtonHomeMenu;
    public Button VibrationButton;
    public Button highQualityButton;
    public bool highQuality;
    public Sprite MusicLoud;
    public Sprite MusicMuted;
    public GameManager gameManager;
    public void Start()
    {
        muteButton.onClick.AddListener(changeSoundSetting);
        muteButtonHomeMenu.onClick.AddListener(changeSoundSetting);
        VibrationButton.onClick.AddListener(changeVibrationSetting);
        highQualityButton.onClick.AddListener(changeQualitySettings);

        if (PlayerPrefs.GetInt("Muted") == 0)
        {
            muted = false;
            muteButtonHomeMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
        }

        else
        {
            muted = true;
            muteButtonHomeMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }

        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            vibrationMuted = false;
            VibrationButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }
        else
        {
            vibrationMuted = true;
            VibrationButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
        }

        if(PlayerPrefs.GetInt("HighQuality") == 0)
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = true;
            highQuality = true;
            highQualityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
        }
        else
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = false;
            highQuality = false;
            highQualityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }
    }
    public void changeQualitySettings()
    {
        if(highQuality)
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = false;
            PlayerPrefs.SetInt("HighQuality", 1);
            highQualityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            highQuality = false;
        }
        else
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = true;
            PlayerPrefs.SetInt("HighQuality", 0);
            highQualityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
            highQuality = true;
        }
    }
    public void changeSoundSetting()
    {
        if (muted)
        {
            PlayerPrefs.SetInt("Muted", 0);
            muteButtonHomeMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
            muted = false;
        }
       
        else
        {
            PlayerPrefs.SetInt("Muted", 1);
            muteButtonHomeMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            muted = true;
        }
  
    }
    public void changeVibrationSetting()
    {
        Debug.Log("change Vib");
        if(vibrationMuted)
        {
            Debug.Log("vib on");
            PlayerPrefs.SetInt("Vibration", 1);
            VibrationButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
            vibrationMuted = false;
        }
        else
        {
            Debug.Log("vib off");
            PlayerPrefs.SetInt("Vibration", 0);
            VibrationButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            vibrationMuted = true;
        }
    }

    void Update()
    {
        if(highQuality)
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = true;
            highQualityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
        }
        else
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = false;
            highQualityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }


        if(muted)
        {
            muteButton.GetComponent<Image>().sprite = MusicMuted;
            muteButtonHomeMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            GetComponent<AudioSource>().volume = 0;
        }

        else
        {
            muteButton.GetComponent<Image>().sprite = MusicLoud;
            muteButtonHomeMenu.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
        }

            
        if(vibrationMuted)
        {
            VibrationButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        }

        else
        {
            VibrationButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "X";
        }
        
        if (gameManager.GetComponent<GameManager>().GameIsPlayed == true)
        {
            if(muted == true)
            GetComponent<AudioSource>().volume = 0f;

            else
                GetComponent<AudioSource>().volume = 0.3f;
        }

        else
        {
            if(muted == false)
            GetComponent<AudioSource>().volume = 0.04f;

            else
                GetComponent<AudioSource>().volume = 0f;
        }
        
    }
    

}
