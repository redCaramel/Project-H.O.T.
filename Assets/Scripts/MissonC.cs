using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissonC : MonoBehaviour
{
    public GameObject hellicopter;
    public Timer timer;
    public TextMeshPro timerText;
    public Collider goal;
    public Collider player;
    int score = 0;
    void Start()
    {
        //gotoResult(63);
    }

    // Update is called once per frame
    void Update()
    {

        if(player.bounds.Intersects(goal.bounds))
        {
            gotoResult(timer.time);
        }
    }
    public void gotoResult(float score)
    {
        SaveData(3, score);
        SceneManager.LoadScene(7);
    }

    public void SaveData(int type, float score)
    {
        PlayerPrefs.SetInt("type", type);
        PlayerPrefs.SetFloat("scoreFloat", score);
    }
    
}
