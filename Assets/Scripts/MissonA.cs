using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissonA : MonoBehaviour
{
    public GameObject hellicopter;
    public List<Collider> target;

    public Collider player;
    void Start()
    {
        //gotoResult(1);
    }

    // Update is called once per frame
    void Update()
    {
        //gotoResult(1000);

        isCollision();
    }

    void isCollision()
    {
        for(int i = 0;i < target.Count; i++)
        {
            if (player.bounds.Intersects(target[i].bounds))
            {
                Debug.Log($"A와 {i} 충돌 감지!");
                //FlightManager.instance.stopFlight();
                gotoResult(i+1);
            }
        }
    }
    public void gotoResult(int score)
    {
        SaveData(1, score);
        
        SceneManager.LoadScene(7);
    }

    public void SaveData(int type, int score)
    {
        PlayerPrefs.SetInt("type", type);
        PlayerPrefs.SetInt("scoreInt", score);
    }
    
}
