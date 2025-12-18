    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
using NUnit.Framework;
using UnityEngine;

    public class ControlPanel : MonoBehaviour {
        SerialPort serialPort;

        [SerializeField]
        KeyCode SpeedUp = KeyCode.Space;
        [SerializeField]
        KeyCode SpeedDown = KeyCode.C;
        [SerializeField]
        KeyCode Forward = KeyCode.W;
        [SerializeField]
        KeyCode Back = KeyCode.S;
        [SerializeField]
        KeyCode Left = KeyCode.A;
        [SerializeField]
        KeyCode Right = KeyCode.D;
        [SerializeField]
        KeyCode TurnLeft = KeyCode.Q;
        [SerializeField]
        KeyCode TurnRight = KeyCode.E;
        [SerializeField]
        KeyCode MusicOffOn = KeyCode.M;
        [SerializeField]
        KeyCode Stop = KeyCode.Delete;
        
        bool isforward = false;
        bool isback = false;
        bool isup = false;
        bool isdown = false;
        bool isleft = false;
        bool isright = false;
        bool isturnleft = false;
        bool isturnright = false;
        bool isstop = false;

        private KeyCode[] keyCodes;

        public Action<PressedKeyCode[]> KeyPressed;
        private void Awake()
        {
            keyCodes = new[] {
                SpeedUp,
                SpeedDown,
                Forward,
                Back,
                Left,
                Right,
                TurnLeft,
                TurnRight,
                Stop
            };

        }

        void Start () {
            serialPort = new SerialPort("COM6", 9600);
            if(!serialPort.IsOpen)
            {
                serialPort.Open();
            }
            
        }
    void Update()
    {
        //Debug.Log("Update Control Panel");
        var pressedKeyCode = new List<PressedKeyCode>();
        if(isup) pressedKeyCode.Add((PressedKeyCode)0);
        if(isdown) pressedKeyCode.Add((PressedKeyCode)1);
        if(isforward) pressedKeyCode.Add((PressedKeyCode)2);
        if(isback) pressedKeyCode.Add((PressedKeyCode)3);
        if(isleft) pressedKeyCode.Add((PressedKeyCode)4);
        if(isright) pressedKeyCode.Add((PressedKeyCode)5);
        if(isturnleft) pressedKeyCode.Add((PressedKeyCode)6);
        if(isturnright) pressedKeyCode.Add((PressedKeyCode)7);
        if(isstop) pressedKeyCode.Add((PressedKeyCode)8);
        if (KeyPressed != null)
        KeyPressed(pressedKeyCode.ToArray());
    }
    void FixedUpdate()
        {
            
            var pressedKeyCode = new List<PressedKeyCode>();
            
            if (serialPort.IsOpen && serialPort.BytesToRead > 0)
                {
                    string receivedData = serialPort.ReadLine();
                    //Debug.Log(receivedData);
                    string[] cmd = receivedData.Split(",");
                    
                    string str = cmd[1] .Substring(cmd[1].IndexOf('o') + 1).Trim();
                    if (int.Parse(str) < 400) {isup = true; isdown = false;}
                    else if (int.Parse(str) > 600) {isdown = true; isup = false;}
                    else {isup = false; isdown = false;}
                    str = cmd[0].Substring(cmd[0].IndexOf('y') + 1).Trim();
                    string[] inp = str.Split('/');
                    //Debug.Log(inp[0] + " " + inp[1]);
                    if (int.Parse(inp[1]) < 100) {isforward = true; isback = false;}
                    else if (int.Parse(inp[1]) > 900) {isforward = false; isback = true;}
                    else {isforward = false; isback = false;}

                    if (int.Parse(inp[0]) > 900) {isleft = true; isright = false;}
                    else if (int.Parse(inp[0]) < 100) {isleft = false; isright = true;}
                    else {isleft = false; isright = false;}
                    
                    if (cmd[3].Equals("lp")) {isturnleft = true; isturnright = false; }
                    else if (cmd[3].Equals("rp")) {isturnleft = false; isturnright = true; }
                    else {isturnleft = false; isturnright = false; }
                    if (cmd[2].Equals("btn")) {
                        isstop = true;
                        serialPort.Close();
                    }
                }

                if (KeyPressed != null)
                    KeyPressed(pressedKeyCode.ToArray());

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
