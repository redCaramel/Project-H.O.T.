using System;
using TMPro;
using UnityEngine;

public class TimerR : MonoBehaviour
{
    TextMeshPro text;
    public float time;
    public bool isStart;
    public float maxTime;
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
            string timerText = "남은 시간: ";
            float remainTime = maxTime - time;
            if (remainTime < 0) remainTime = 0;
            timerText += Math.Truncate(remainTime).ToString();
            timerText += "초";
            text.text = timerText;
        }
        
    }
}
