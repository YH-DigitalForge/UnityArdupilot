using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class PilotBehavior : MonoBehaviour
{
    public string portName = "COM7";

    public int baudRate = 9600;

    private static SerialPort port;

    // Awake method is called when each objects are load when Scene has been loaded.
    private void Awake()
    {
        // Detect available ports
        foreach (string portName in SerialPort.GetPortNames())
        {
            Debug.Log(portName);
        }
        port = new SerialPort(portName, baudRate);
        port.DataReceived += new SerialDataReceivedEventHandler(serialport_datareceived);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Starting object with port (name={port.PortName},baudrate={port.BaudRate.ToString()})");
        port.Open();
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

        transform.rotation = Quaternion.Euler(float.Parse(parsed[0]), float.Parse(parsed[1]), float.Parse(parsed[2]));        // Rotate GameObject using Quaternion.Euler
    }

    // Update is called once per frame
    void Update()
    {
        string serialRead = port.ReadLine();
        Debug.Log($"Read line from serial : {serialRead}");
        string[] parsed = serialRead.Split('|');
        foreach (string data in parsed)
        {
            Debug.Log($"Parsed serial value : {data}");
        }

        transform.rotation = Quaternion.Euler(float.Parse(parsed[0]), float.Parse(parsed[1]), float.Parse(parsed[2]));        // Rotate GameObject using Quaternion.Euler
    }
    
    // Class Finalizer
    ~PilotBehavior()
    {
        port.Close();   
    }
}
