using UnityEngine;
using System.IO.Ports;
using System;
using Unity.Mathematics;

public class ArduinoDataController : MonoBehaviour
{
    public Rigidbody HelicopterRigid;
    public Transform Helicopter;
    double tempL = 0;
    double tempR = 0;
    int t = 0;
    SerialPort serialPort;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort("COM10", 9600);
        if(!serialPort.IsOpen)
       {
           serialPort.Open();
        }
        serialPort.Write("null");
    }
    void FixedUpdate()
    {
        float xTilt = Mathf.DeltaAngle(0, Helicopter.rotation.eulerAngles.x); 
        float zTilt = Mathf.DeltaAngle(0, Helicopter.rotation.eulerAngles.z);

        double yL = 20 * (Math.Cos(xTilt * Mathf.Deg2Rad) * Math.Sin(zTilt * Mathf.Deg2Rad) - Math.Sin(xTilt * Mathf.Deg2Rad) * Math.Cos(zTilt * Mathf.Deg2Rad));
        double yR = -20 * (Math.Cos(xTilt * Mathf.Deg2Rad) * Math.Sin(zTilt * Mathf.Deg2Rad) + Math.Sin(xTilt * Mathf.Deg2Rad) * Math.Cos(zTilt * Mathf.Deg2Rad));

        double aL = Math.Asin(yL / 10) * Mathf.Rad2Deg;
        double aR = Math.Asin(yR / 10) * Mathf.Rad2Deg;

        if (!double.IsNaN(aL)) tempL = Mathf.Clamp((float)aL, -30, 30);
        if (!double.IsNaN(aR)) tempR = Mathf.Clamp((float)aR, -30, 30);

        string sendingData = "";
        if (Math.Round(tempL) >= 0) sendingData += "+";
        sendingData += (Math.Round(tempL)).ToString();
        if (Math.Round(tempR) >= 0) sendingData += "+";
        sendingData += (Math.Round(tempR)).ToString();
        Debug.Log(sendingData);
        if(serialPort.IsOpen && t%3==0) serialPort.Write(sendingData);
        if(t == 20)
        {
            t = 0;
        }
        t++;
    }
    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Write("null");
            serialPort.Close();
            Debug.Log("Serial Port Closed Safely");
        }
    }
}
