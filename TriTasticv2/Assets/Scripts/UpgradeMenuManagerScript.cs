using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UpgradeMenuManagerScript : MonoBehaviour
{
    public Button Upgrade1;
    public Button Upgrade2;
    public Button backToHomeButton2;
    void Start()
    {
        Upgrade1.onClick.AddListener(doUpgrade1);
        Upgrade2.onClick.AddListener(doUpgrade2);
        backToHomeButton2.onClick.AddListener(backToHomeMenu2);
    }

    

    public void doUpgrade1()
    {
        // Das Gun Upgrade!!!

        //wenn Upgrade gemacht Bool GunIsBought in PauseManagerScript umstellen!!!

       
        FindObjectOfType<AudioManager>().Play("Upgrade Sound");
        Debug.Log("UpgradeSuccesed");


    }

    public void doUpgrade2()
    {
        // Das Split Upgrade!!!

        FindObjectOfType<AudioManager>().Play("Upgrade Sound");
        Debug.Log("Upgrade2Successed");
    }
    public void backToHomeMenu2()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
