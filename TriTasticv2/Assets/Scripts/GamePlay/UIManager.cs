using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameManager gameManager;
    private FollowFingerScript followFinger;
    private BlockSpawner blockSpawner;
    public ControlManagerScript CS;

    public GameObject newHighScoreText;
    public Text scoreText;
    public int score;
    public Text highScoretext;

    //Anfangsgeschwindigkeit
    public float startSpeed = 5f;
    //Wird auf Speed draufgerechnet
    public float speedStep = 1f;
    //Der Unterschied im Score der da sein muss, um den SPeed zu erhöhen
    public int scoreBetweenSteps = 8;

    //wird immer nach erhöhen auf den aktuellen Score gesetzt
    public int lastStepScore = 0;

    public float savedSpeed = 0;

    public float[] timeBetweenSpawnList =
    {
        2.5f,
        2f,
        1.5f,
        1f,
    };


    void Start()
    {

        
        newHighScoreText.SetActive(false);

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        followFinger = GameObject.FindWithTag("Player").GetComponent<FollowFingerScript>();
        blockSpawner = GameObject.FindWithTag("BlockSpawner").GetComponent<BlockSpawner>();
        blockSpawner.currentSpeed = startSpeed;

    }

    public void IncrementScore()
    {
        
        if(gameManager.GameIsOver)
        {
            return;
        }
        score++;


        if(CS.GameMode == 0)
        {
            if (score > PlayerPrefs.GetInt("PlayerHighScore", 0))
            {

                PlayerPrefs.SetInt("PlayerHighScore", score);
            }
        }

        if (CS.GameMode == 1)
        {
            if (score > PlayerPrefs.GetInt("RingsHighScore", 0))
            {

                PlayerPrefs.SetInt("RingsHighScore", score);
            }
        }

        if (CS.GameMode == 2)
        {
            if (score > PlayerPrefs.GetInt("ShootHighScore", 0))
            {

                PlayerPrefs.SetInt("ShootHighScore", score);
            }
        }
        

    }
    public int getDifficultyForScore(int score)
    {
        // return: die Methode stoppt hier und gibt den Wert zurück der hinter return steht
        if(score < 10)
        {
            return 0;
        }

        if(score >= 10 && score < 30)
        {
            return 1;
        }

        if (score >= 30 && score < 50)
        {
            return 2;
        }

        if (score >= 50 && score < 70)
        {
            return 3;
        }

        if (score >= 70 && score < 100)
        {
            return 4;
        }

        if (score >= 100 && score < 120)
        {
            return 5;
        }

        if (score >= 120 && score < 140)
        {
            return 6;
        }

        if (score >= 140 && score < 160)
        {
            return 7;
        }

        if (score >= 160 && score < 180)
        {
            return 8;
        }

        if (score >= 180 && score < 200)
        {
            return 9;
        }

        if (score >= 200 && score < 220)
        {
            return 10;
        }

        if (score >= 220 && score < 240)
        {
            return 11;
        }

        if (score >= 240 && score < 260)
        {
            return 12;
        }

        if (score >= 260 && score < 280)
        {
            return 13;
        }

        if (score >= 280 && score < 300)
        {
            return 14;
        }

        else
        {
            return 15;
        }
        
    }
    public void Update()
    {
        


        scoreText.text =  score.ToString();       
        int difficulty = getDifficultyForScore(score);

        if (gameManager.GameIsOver == false && followFinger.isDashing == false)
        {
            if (score - lastStepScore > scoreBetweenSteps)
            {
                lastStepScore = score;
                blockSpawner.setSpeed(blockSpawner.currentSpeed + speedStep);
            }

            if (difficulty >= timeBetweenSpawnList.Length)
            {
                blockSpawner.timeBetweenSpawn = timeBetweenSpawnList[timeBetweenSpawnList.Length - 1];
            }
            else
            {
                blockSpawner.timeBetweenSpawn = timeBetweenSpawnList[difficulty];
            }
        }       
    }
    public void setDashSpeed()
    {        
        savedSpeed = blockSpawner.currentSpeed;
        blockSpawner.setSpeed(blockSpawner.currentSpeed * 3);
    }

    public void resetDashSpeed()
    {        
        blockSpawner.setSpeed(savedSpeed);
    }

}
