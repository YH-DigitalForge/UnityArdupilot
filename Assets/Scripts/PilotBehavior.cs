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
    
    /*
     * Serial Info
     */
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
            foreach (string data in parsed)
            {
                Debug.Log($"Parsed serial value : {data}");
            }
        
            // Rotate object
            transform.rotation = Quaternion.Euler(
                int.Parse(parsed[0].Replace(Gyro+"x", "")),
                int.Parse(parsed[1].Replace(Gyro+"y", "")),
                int.Parse(parsed[2].Replace(Gyro+"z", ""))
            );        // Rotate GameObject using Quaternion.Euler
        
            // Move object
            rigidbody.AddForce(
                int.Parse(parsed[3].Replace(Accel+"x", "")),
                int.Parse(parsed[4].Replace(Accel+"y", "")),
                int.Parse(parsed[5].Replace(Accel+"z", ""))
            );
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
