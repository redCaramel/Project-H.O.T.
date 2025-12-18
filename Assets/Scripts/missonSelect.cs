using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class missonSelect : MonoBehaviour
{
    SerialPort serialPort;
    public int select = 1;
    public Vector3 a;
    public Vector3 b;
    public Vector3 c;
    public RectTransform cursor;

    public TextMeshPro guideText;
    int temp;
    public DataBaseHandler scoreBoard;
    void Start () {
        serialPort = new SerialPort("COM6", 9600);
        if(!serialPort.IsOpen)
        {
            serialPort.Open();
        }
        temp = select;
	}
    void Update()
    {
        if(temp != select)
        {
            temp = select;
            if(temp == 1)
            {
                guideText.text = "정확한 지점에 착륙하세요!\n정가운데에 착륙할 수록 추가 점수!";
            } 
            else if(temp == 2)
            {
                guideText.text = "공중의 체크포인트를 통과하세요!\n많이 통과할 수록 높은 점수!";
            }
            else if(temp == 3)
            {
                guideText.text = "목표 지점까지 빠르게 이동하세요!\n짧은 시간만에 도착해야 합니다!";
            }
        }
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            string receivedData = serialPort.ReadLine();
            string[] cmd = receivedData.Split(",");
            if (select > 1 && (Input.GetKeyDown(KeyCode.A) || cmd[3].Equals("lp")))
            {
                select--;
                scoreBoard.switchType(select-1);
            }
            if (select < 3 && (Input.GetKeyDown(KeyCode.D) || cmd[3].Equals("rp")))
            {
                select++;
                scoreBoard.switchType(select-1);
            }
            if (Input.GetKeyDown(KeyCode.Space) || cmd[2].Equals("btn"))
            {
                if (select == 1)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(5);
                }
                if (select == 2)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(11);
                }
                if (select == 3)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(12);
                }
            }
        }
         else // not opened serialPort
        {
            if (select > 1 && Input.GetKeyDown(KeyCode.A))
            {
                select--;
                scoreBoard.switchType(select-1);
            }
            if (select < 3 && Input.GetKeyDown(KeyCode.D))
            {
                select++;
                scoreBoard.switchType(select-1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (select == 1)
                {
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(5);
                }
            }
        }
        if (select == 1) cursor.position = a;
        else if (select == 2) cursor.position = b;
        else cursor.position = c;
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
