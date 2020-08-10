﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BackGroundMusicManagement : MonoBehaviour
{
    public AudioManager Audio;

    public bool muted;

    public Button muteButton;

    public Sprite MusicLoud;
    public Sprite MusicMuted;

    public GameObject controllerManager;


    public void Start()
    {
        if (PlayerPrefs.GetInt("Muted") == 0)
        {
            muted = false;
        }

        else
            muted = true;
        
        muteButton.onClick.AddListener(changeSoundSetting);
        

        controllerManager = GameObject.FindWithTag("Controller");
    }
    

    public void changeSoundSetting()
    {
        if (muted)
        {
            PlayerPrefs.SetInt("Muted", 0);
            muted = false;
        }
            
        

        else
        {
            PlayerPrefs.SetInt("Muted", 1);
            muted = true;
        }
            
    }
    void Update()
    {
        
        if(muted)
        {
            muteButton.GetComponent<Image>().sprite = MusicMuted;
            GetComponent<AudioSource>().volume = 0;

        }

        else
        {
            muteButton.GetComponent<Image>().sprite = MusicLoud;
            
            
        }
            

        
            


        if (controllerManager.GetComponent<ControlManagerScript>().GameIsPlayed == true)
        {
            if(muted == true)
            GetComponent<AudioSource>().volume = 0f;

            else
                GetComponent<AudioSource>().volume = 0.5f;
        }

        else
        {
            if(muted == false)
            GetComponent<AudioSource>().volume = 0.05f;

            else
                GetComponent<AudioSource>().volume = 0f;
        }
        
    }
    

}
