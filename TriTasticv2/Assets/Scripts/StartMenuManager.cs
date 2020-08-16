using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    
    
    public GameObject StartMenuCanvas;
    


    public Button ShopButton;
    
    public Button StartButton;
    //public Button AboutMeButton;




    void Start()
    {

        ShopButton.onClick.AddListener(openShop);
        StartButton.onClick.AddListener(StartTheGame);
        


        

    }
    

    
    public void openShop()
    {
        SceneManager.LoadScene("ShopMenu");
    }
    

    public void StartTheGame()
    {
        StartMenuCanvas.SetActive(false);

        SceneManager.LoadScene("FollowFinger");
    }

    
}
