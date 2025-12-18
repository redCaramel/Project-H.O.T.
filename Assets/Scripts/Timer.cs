using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TextMeshPro text;
    public float time;
    public bool isStart;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        isStart = true;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) isStart = true;
        if(isStart)
        {
            time += Time.deltaTime;
            string timerText = "";
            if (time < 10) timerText += '0';
            timerText += Math.Truncate(time).ToString();
            timerText += ':';
            timerText += Math.Truncate(time*100) % 100;
            text.text = timerText;
        }
        
    }
}
