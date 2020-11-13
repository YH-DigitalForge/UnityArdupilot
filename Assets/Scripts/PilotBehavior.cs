using System.Globalization;
using System.IO;
using System.IO.Ports;
using TMPro;
using UnityEngine;

public class PilotBehavior : MonoBehaviour
{
    /*
     * Gameobject info
     */
    public Camera firstPerson;
    public Camera secondPerson;
    public bool isFirstPerson = false;
    
    
    /*
     * Communication Format
     */
    private const string Indicator = "[UnityArdupilot]";
    private const string Gyro = "Gyro";
    private const string Accel = "Acc";
    private const string Angle = "Angle";
    private const string Temperature = "Tmp";
    
    /*
     * [Arduino Serial Communication Datasheet]
     * 0 ~ 2 : Angle[X-Z]
     * 3 ~ 5 : Acc[X-Z]
     * 6 ~ 8 : Gyro[X-Z]
     * 9 : Tmp
     * 10 ~ 11 : AccAngle[X-Y]
     * 12 ~ 14 : GyroAngle[X-Z]
     */
    
    
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
        port = new SerialPort(this.portName, this.baudRate);
        // Detect available ports
        foreach (string portName in SerialPort.GetPortNames())
        {
            Debug.Log(portName);
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Starting object with port (name={port.PortName},baudrate={port.BaudRate.ToString()})");
        port.Open();
        CameraUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            string serialRead = port.ReadLine();
            Debug.Log($"Read line from serial : {serialRead}");
            
            if (serialRead.StartsWith(Indicator))
                PilotUpdate(serialRead);
        }
        catch (IOException exception)
        {
            Debug.LogError("Port is not open!");
            Debug.LogError(exception);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            isFirstPerson = !isFirstPerson;
            CameraUpdate();
        }
    }

    void CameraUpdate()
    {
        firstPerson.gameObject.SetActive(isFirstPerson);
        secondPerson.gameObject.SetActive(!isFirstPerson);
    }

    void PilotUpdate(string line)
    {
        string[] parsed = line.Remove(0, Indicator.Length).Split('|');
        
        float angleX = ParseData(Angle, Axis.X, parsed[0]);
        float angleY = ParseData(Angle, Axis.Y, parsed[1]);
        float angleZ = ParseData(Angle, Axis.Z, parsed[2]);
        float accX = ParseData(Accel, Axis.X, parsed[3]);
        float accY = ParseData(Accel, Axis.Y, parsed[4]);
        float accZ = ParseData(Accel, Axis.Z, parsed[5]);
        float gyroX = ParseData(Gyro, Axis.X, parsed[6]);
        float gyroY = ParseData(Gyro, Axis.Y, parsed[7]);
        float gyroZ = ParseData(Gyro, Axis.Z, parsed[8]);
        float tmp = ParseData(Temperature, Axis.None, parsed[9]);
        float accAngleX = ParseData(Accel + Angle, Axis.X, parsed[10]);
        float accAngleY = ParseData(Accel + Angle, Axis.Y, parsed[11]);
        float gyroAngleX = ParseData(Gyro + Angle, Axis.X, parsed[12]);
        float gyroAngleY = ParseData(Gyro + Angle, Axis.Y, parsed[13]);
        float gyroAngleZ = ParseData(Gyro + Angle, Axis.Z, parsed[14]);

        // Rotate GameObject using Quaternion.Euler
        transform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
        
        // Move object
        this.GetComponent<Rigidbody>().AddForce(accX, accY, accZ);
    }
    
    /*
     * Enum-like to indicate Axis value.
     */
    private static class Axis
    {
        public static string X = "X=";
        public static string Y = "Y=";
        public static string Z = "Z=";
        public static string None = "=";
    }

    private static float ParseData(string type, string axis, string data)
    {
        return float.Parse(data.Replace(type + axis, ""), NumberStyles.Float, CultureInfo.InvariantCulture);
    }
    
    // Class Finalizer
    ~PilotBehavior()
    {
        port.Close();
    }
}
