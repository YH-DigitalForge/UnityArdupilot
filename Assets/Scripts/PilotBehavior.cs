using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class PilotBehavior : MonoBehaviour
{
    public string portName = "COM7";
    
    // Start is called before the first frame update
    void Start()
    {
        // Detect available ports
        foreach (string portName in SerialPort.GetPortNames())
        {
            Debug.Log(portName);
        }
        SerialPort port = new SerialPort(portName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
