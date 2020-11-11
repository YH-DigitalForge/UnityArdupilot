using System;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using UnityEngine;

public class PilotBehavior : MonoBehaviour
{
    /*
     * Gameobject info
     */
    public Rigidbody rigidbody;
    
    
    /*
     * Communication Format
     */

    public static string Gyro = "Gy";

    public static string Accel = "Ac";

    public static CultureInfo ci;
    
    /*
     * Serial Info
     */
    public bool doAutoDetect = false;                // Some kind of autodetect stuff.
    
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
        port = new SerialPort(this.portName, this.baudRate);
        // port.DataReceived += new SerialDataReceivedEventHandler(serialport_datareceived);
        
        // I hate this stuff; CulturInfo disrupt parsing string to integer.
        ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Starting object with port (name={port.PortName},baudrate={port.BaudRate.ToString()})");
        port.Open();
    }

    // Event listener of SerialDataReceivedEvent
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
        try
        {
            string serialRead = port.ReadLine();
            Debug.Log($"Read line from serial : {serialRead}");
            string[] parsed = serialRead.Split('|');

            int gyroX = int.Parse(parsed[0].Replace(Gyro + "X=", ""), NumberStyles.Any, ci);
            int gyroY = int.Parse(parsed[1].Replace(Gyro + "Y=", ""), NumberStyles.Any, ci);
            int gyroZ = int.Parse(parsed[2].Replace(Gyro + "Z=", ""), NumberStyles.Any, ci);

            // Rotate GameObject using Quaternion.Euler
            transform.rotation = Quaternion.Euler(
                gyroX,
                gyroY,
                gyroZ
            );

            int accX = int.Parse(parsed[3].Replace(Accel + "X=", ""), NumberStyles.Any, ci);
            int accY = int.Parse(parsed[4].Replace(Accel + "Y=", ""), NumberStyles.Any, ci);
            int accZ = int.Parse(parsed[5].Replace(Accel + "Z=", ""), NumberStyles.Any, ci);

            // Move object
            // rigidbody.AddForce(
            //     accX,
            //     accY,
            //     accZ
            // );
        }
        catch (IOException exception)
        {
            Debug.LogError("Port is not open!");
            Debug.LogError(exception);
        }
    }
    
    // Class Finalizer
    ~PilotBehavior()
    {
        port.Close();   
    }
}
