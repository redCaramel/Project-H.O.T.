using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;
public class modSelect : MonoBehaviour
{
    SerialPort serialPort;
    public int select = 1;
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;
    public RectTransform cursor;
  

    void Start () {
        serialPort = new SerialPort("COM6", 9600);
        if(!serialPort.IsOpen)
        {
            serialPort.Open();
        }
	}
    void Update()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            
            string receivedData = serialPort.ReadLine();
            Debug.Log(receivedData);
            string[] cmd = receivedData.Split(",");
            if (select > 1 && (Input.GetKeyDown(KeyCode.A) || cmd[3].Equals("lp")))
            {
                select--;
            }
            if (select < 3 && (Input.GetKeyDown(KeyCode.D) || cmd[3].Equals("rp")))
            {
                select++;
            }
            if (Input.GetKeyDown(KeyCode.Space) ||cmd[2].Equals("btn"))
            {
                if (select == 1)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(4);
                }
                else if(select == 2)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(1);
                }
                else if(select == 3)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(6);
                }
            }
        }
        else // not opened serialPort
        {
            if (select > 1 && Input.GetKeyDown(KeyCode.A))
            {
                select--;
            }
            if (select < 3 && Input.GetKeyDown(KeyCode.D))
            {
                select++;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (select == 1)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(4);
                }
                else if(select == 2)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(1);
                }
                else if(select == 3)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(6);
                }
            }
        }
        if (select == 1) cursor.position = a;
        else if (select == 2) cursor.position = b;
        else cursor.position = c;
    }
}
