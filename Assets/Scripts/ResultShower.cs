using TMPro;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.Rendering;

public class ResultShower : MonoBehaviour
{
    public static ResultShower instance;
    SerialPort serialPort;
    public GameObject result;
    public int playerScoreInt;
    public float playerScoreFloat;
    public int missonType;
    public string playerName;
    void Awake()
    {
        instance = this;
        playerScoreInt = PlayerPrefs.GetInt("scoreInt");
        playerScoreFloat = PlayerPrefs.GetFloat("scoreFloat");
        missonType = PlayerPrefs.GetInt("type");
        playerName = PlayerPrefs.GetString("PlayerName");
    }
    void Start()
    {
        serialPort = new SerialPort("COM6", 9600);
        if(!serialPort.IsOpen)
        {
            serialPort.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            
            string receivedData = serialPort.ReadLine();
            Debug.Log(receivedData);
            string[] cmd = receivedData.Split(",");
            if(cmd[2].Equals("btn"))
            {
                if(serialPort.IsOpen) serialPort.Close();
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(serialPort.IsOpen) serialPort.Close();
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }
    
    public void notBoard()
    {
        result.GetComponent<TextMeshPro>().text = "임무 성공!\n더 높은 기록에 도전해보세요!";
    }
    public void Fail()
    {
        result.GetComponent<TextMeshPro>().text = "임무 실패...";
    }
    public void success()
    {
        result.GetComponent<TextMeshPro>().text = "임무 성공! 당신은 전설의 조종사입니다!";
    }
}
