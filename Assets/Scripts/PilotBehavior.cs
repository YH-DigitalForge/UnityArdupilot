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
    public Camera thirdPerson;
    
    
    public enum CamPerspective
    {
        First, Second, Third
    }
    
    public CamPerspective camPerspective = CamPerspective.Third;

    public float speed = 0.1f;    // Speed of Pilot.
    
    /*
     * Communication Format
     */
    private const string Indicator = "[UnityArdupilot]";
    private const string Angle = "Angle";

    public bool fixX = false;
    public bool fixY = false;
    public bool fixZ = false;
    
    
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
    public bool doAutoDetect = true;                // Some kind of autodetect stuff.
    public string portName = "COM3";
    public int baudRate = 9600;
    private static SerialPort _port;

    // Awake method is called when each objects are load when Scene has been loaded.
    private void Awake()
    {
        _port = new SerialPort();
        _port.BaudRate = baudRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraInit();
        ConnectSerial();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CameraChange();
        }
        
        try
        {
            if (!_port.IsOpen)
            {
                ConnectSerial();
            }
            
            string serialRead = _port.ReadLine();
            Debug.Log($"Read line from serial : {serialRead}");
            
            if (serialRead.StartsWith(Indicator))
                PilotUpdate(serialRead);
        }
        catch (IOException exception)
        {
            Debug.LogError("Port is not open!");
            Debug.LogError(exception);
        }
    }

    void ConnectSerial()
    {
        if (doAutoDetect)
        {
            Debug.Log("Displaying ports...");
            // Detect available ports
            foreach (string portname in SerialPort.GetPortNames())
            {
                Debug.Log($"Port : {portname}, Displayig Serial outputs...");
                _port.PortName = portname;
                _port.Open();
                for (int i = 0; i < 5; i++)
                {
                    Debug.Log(_port.ReadLine());
                }
                // Debug.Log($"Validating port `{portname}`...");
                // _port.PortName = portname;
                // try
                // {
                //     _port.Open();
                //     string line = _port.ReadLine();
                //     if (line.StartsWith(Indicator))
                //     {
                //         Debug.Log("Found valid Arduino serial port! connecting with it.");
                //         break;
                //     }
                //     Debug.Log($"Found port {portname} is available, ");
                //     _port.Close();
                // }
                // catch (IOException e)
                // {
                //     Debug.LogError($"Cannot open port {portname}. Trying next port...");
                //     _port.Close();
                // }
            }

            // throw new IOException("Cannot find valid Arduino serial port! Throwing IOException :(");
            throw new IOException("Select port with given ports list!");
        }
        else
        {
            _port.PortName = portName;
            _port.Open();
        }
        
        Debug.Log($"Starting object with port (name={_port.PortName},baudrate={_port.BaudRate.ToString()})");
    }

    void CameraInit()
    {
        switch (camPerspective)
        {
            case CamPerspective.First:
                SetCameraStatus(firstPerson, true);
                SetCameraStatus(secondPerson, false);
                SetCameraStatus(thirdPerson, false);
                break;
            case CamPerspective.Second:
                SetCameraStatus(firstPerson, false);
                SetCameraStatus(secondPerson, true);
                SetCameraStatus(thirdPerson, false);
                break;
            case CamPerspective.Third:
                SetCameraStatus(firstPerson, false);
                SetCameraStatus(secondPerson, false);
                SetCameraStatus(thirdPerson, true);
                break;
        }
    }
    
    void CameraChange()
    {
        switch (camPerspective)
        {
            case CamPerspective.First:
                SetCameraStatus(firstPerson, false);
                SetCameraStatus(secondPerson, true);
                camPerspective = CamPerspective.Second;
                break;
            case CamPerspective.Second:
                SetCameraStatus(secondPerson, false);
                SetCameraStatus(thirdPerson, true);
                camPerspective = CamPerspective.Third;
                break;
            case CamPerspective.Third:
                SetCameraStatus(thirdPerson, false);
                SetCameraStatus(firstPerson, true);
                camPerspective = CamPerspective.First;
                break;
        }
    }

    void SetCameraStatus(Camera camera, bool isActive)
    {
        // camera.gameObject.SetActive(isActive);
        // camera.GetComponent<AudioListener>().enabled = isActive;
        camera.enabled = isActive;
    }

    void PilotUpdate(string line)
    {
        string[] parsed = line.Remove(0, Indicator.Length).Split(',');
        
        float angleX = ParseData(Angle, Axis.X, parsed[0]);
        float angleY = ParseData(Angle, Axis.Y, parsed[1]);
        float angleZ = ParseData(Angle, Axis.Z, parsed[2]);
        // float accX = ParseData(Accel, Axis.X, parsed[3]);
        // float accY = ParseData(Accel, Axis.Y, parsed[4]);
        // float accZ = ParseData(Accel, Axis.Z, parsed[5]);
        // float gyroX = ParseData(Gyro, Axis.X, parsed[6]);
        // float gyroY = ParseData(Gyro, Axis.Y, parsed[7]);
        // float gyroZ = ParseData(Gyro, Axis.Z, parsed[8]);
        // float tmp = ParseData(Temperature, Axis.None, parsed[9]);
        // float accAngleX = ParseData(Accel + Angle, Axis.X, parsed[10]);
        // float accAngleY = ParseData(Accel + Angle, Axis.Y, parsed[11]);
        // float gyroAngleX = ParseData(Gyro + Angle, Axis.X, parsed[12]);
        // float gyroAngleY = ParseData(Gyro + Angle, Axis.Y, parsed[13]);
        // float gyroAngleZ = ParseData(Gyro + Angle, Axis.Z, parsed[14]);
        
        // Debug.Log($"Angle<x={angleX},y={angleY},z={angleZ}>");
        // Debug.Log($"Acceleration<x={accX},y={accY},z={accZ}>");
        // Debug.Log($"Gyro<x={gyroX},y={gyroY},z={gyroZ}>");
        // Debug.Log($"AccelerationAngle<x={accAngleX},y={accAngleY}>");
        // Debug.Log($"GyroAngle<x={gyroAngleX},y={gyroAngleY},z={gyroAngleZ}>");
        // Debug.Log($"Temperature<t={tmp}>");
        
        // Debug.Log(
        //     $"Angle<x={angleX},y={angleY},z={angleZ}>," +
        //     $"Acceleration<x={accX},y={accY},z={accZ}>," +
        //     $"Gyro<x={gyroX},y={gyroY},z={gyroZ}>," +
        //     $"AccelerationAngle<x={accAngleX},y={accAngleY}>," +
        //     $"GyroAngle<x={gyroAngleX},y={gyroAngleY},z={gyroAngleZ}>," +
        //     $"Temperature<t={tmp}>"
        // );
        
        Debug.Log($"Angle<x={angleX},y={angleY},z={angleZ}>");

        if (Input.GetKey(KeyCode.R))
        {
            // Reset position
            Debug.Log("Reset rotation to (0,0,0)");
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            // Close game
            Debug.Log("Close the game.");
            Application.Quit();
        }
        float fAngleX = fixX ? 0.0f : -angleX;
        float fAngleY = fixY ? 0.0f : angleY;
        float fAngleZ = fixZ ? 0.0f : angleZ;
        Debug.Log($"FixedAngle<x={fAngleX},y={fAngleY},z={fAngleZ}>");
        transform.rotation = Quaternion.Euler(fAngleX, fAngleY, fAngleZ);
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
        _port?.Close();
    }
}
