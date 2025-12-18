using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissonB : MonoBehaviour
{
    public GameObject hellicopter;
    public TimerR timerR;
    public TextMeshPro scText;
    public List<GameObject> target;
    public Collider player;
    int score = 0;
    void Start()
    {
        //gotoResult(63);
    }

    // Update is called once per frame
    void Update()
    {

        isCollision();
        if (timerR.time >= timerR.maxTime)
        {
            gotoResult(score);
        }
    }

    void isCollision()
    {
        for(int i = 0;i < target.Count; i++)
        {
            if (player.bounds.Intersects(target[i].GetComponent<Collider>().bounds))
            {
                Debug.Log($"A와 {i} 충돌 감지!");
                //FlightManager.instance.stopFlight();
                score++;
                scText.text = "점수: " + score.ToString();
                Destroy(target[i]);
                target.RemoveAt(i);
                i--;
            }
        }
    }
    public void gotoResult(int score)
    {
        SaveData(2, score);
        
        SceneManager.LoadScene(7);
    }

    public void SaveData(int type, int score)
    {
        PlayerPrefs.SetInt("type", type);
        PlayerPrefs.SetInt("scoreInt", score);
    }
    
}
