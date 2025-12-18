using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;
public class motortest : MonoBehaviour
{
    SerialPort serialPort;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort("COM10", 9600);
        if(!serialPort.IsOpen)
        {
            serialPort.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            string sendingData = "null";
            if(serialPort.IsOpen) serialPort.Write(sendingData);
            Debug.Log(sendingData);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            string sendingData = "+10/-10";
            if(serialPort.IsOpen) serialPort.Write(sendingData);
            Debug.Log(sendingData);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            string sendingData = "-10/+10";
            if(serialPort.IsOpen) serialPort.Write(sendingData);
            Debug.Log(sendingData);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            string sendingData = "break";
            if(serialPort.IsOpen) serialPort.Write(sendingData);
            Debug.Log(sendingData);
        }
        if (Input.GetKeyDown(KeyCode.Space))
            {
                if(serialPort.IsOpen) serialPort.Close();
                SceneManager.LoadScene(0);
                

            }
         if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            
            string receivedData = serialPort.ReadLine();
            Debug.Log(receivedData);
        }
    }
     void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial Port Closed Safely");
        }
    }
}
