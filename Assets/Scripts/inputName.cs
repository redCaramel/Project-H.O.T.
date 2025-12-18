using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;
using TMPro;
public class inputName : MonoBehaviour
{
    SerialPort serialPort;
    char a;
    char b;
    char c;
    int select = 1;
    int current = 1;
    public GameObject cursor;
    public GameObject letterA;
    public GameObject letterB;
    public GameObject letterC;
    public int missionType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        missionType = PlayerPrefs.GetInt("type");
        a = 'A';
        b = 'B';
        c = 'C';
        serialPort = new SerialPort("COM6", 9600);
        if(!serialPort.IsOpen)
        {
            serialPort.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateLetter();
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            
            string receivedData = serialPort.ReadLine();
            Debug.Log(receivedData);
            string[] cmd = receivedData.Split(",");
            if (select > 1 && (Input.GetKeyDown(KeyCode.A) || cmd[3].Equals("lp")))
            {
                select--;
            }
            if (select < 26 && (Input.GetKeyDown(KeyCode.D) || cmd[3].Equals("rp")))
            {
                select++;
            }
            if (Input.GetKeyDown(KeyCode.Space) ||cmd[2].Equals("btn"))
            {
                if (current == 1)
                {
                    a = (char)(select + 64);
                    current++;
                    select = 1;
                    cursor.transform.position = new Vector3(letterB.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
                }
                else if(current == 2)
                {
                    b = (char)(select + 64);
                    current++;
                    select = 1;
                    cursor.transform.position = new Vector3(letterC.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
                }
                else if(current == 3)
                {
                    c = (char)(select + 64);
                    string playerName = "" + a + b + c;
                    PlayerPrefs.SetString("PlayerName", playerName);
                    if(serialPort.IsOpen) serialPort.Close();
                    SceneManager.LoadScene(3);  
                }
            }
        }
        else // not opened serialPort
        {
           if (select > 1 && (Input.GetKeyDown(KeyCode.A)))
            {
                select--;
            }
            if (select < 26 && (Input.GetKeyDown(KeyCode.D)))
            {
                select++;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (current == 1)
                {
                    a = (char)(select + 64);
                    current++;
                    select = 1;
                    cursor.transform.position = new Vector3(letterB.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
                }
                else if(current == 2)
                {
                    b = (char)(select + 64);
                    current++;
                    select = 1;
                    cursor.transform.position = new Vector3(letterC.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
                }
                else if(current == 3)
                {
                    c = (char)(select + 64);
                    string playerName = "" + a + b + c;
                    PlayerPrefs.SetString("PlayerName", playerName);
                    if(serialPort.IsOpen) serialPort.Close();
                    if(missionType == 1) SceneManager.LoadScene(3);  
                    else if(missionType == 2) SceneManager.LoadScene(8);
                    else if(missionType == 3) SceneManager.LoadScene(9);
                    else if(missionType == 4) SceneManager.LoadScene(10);
                    
                }
            }
        }
    }
    void updateLetter()
    {
        if(current == 1)
        {
            letterA.GetComponent<TextMeshPro>().text = "" + (char)(select + 64);
        }
        else if(current == 2)
        {
            letterB.GetComponent<TextMeshPro>().text = "" + (char)(select + 64);
        }
        else if(current == 3)
        {
            letterC.GetComponent<TextMeshPro>().text = "" + (char)(select + 64);
        }
    }
}
