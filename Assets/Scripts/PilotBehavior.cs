using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class PilotBehavior : MonoBehaviour
{
    public string portName = "COM7";

    public int baudRate = 9600;

    private SerialPort port;

    // Awake method is called when each objects are load when Scene has been loaded.
    private void Awake()
    {
        // Detect available ports
        foreach (string portName in SerialPort.GetPortNames())
        {
            Debug.Log(portName);
        }
        SerialPort port = new SerialPort(portName, baudRate);
        port.DataReceived += new SerialDataReceivedEventHandler(serialport_datareceived);
        port.Open();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Starting object with port (name={port.PortName},baudrate={port.BaudRate.ToString()})");
    }

    private void serialport_datareceived(object sender, SerialDataReceivedEventArgs e)
    {
        string serialRead = port.ReadLine();
        Debug.Log($"Read line from serial : {serialRead}");
        string[] parsed = serialRead.Split('|');
        foreach (string data in parsed)
        {
            Debug.Log($"Parsed serial value : {data}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
