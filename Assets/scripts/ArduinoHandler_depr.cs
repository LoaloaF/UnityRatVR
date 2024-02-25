// The script interfacing the Arduino with 3 ball rotation sensors to Unity scene
// Author: Eminhan Ozil
// ETH Zurich
// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoHandler_depr : MonoBehaviour
    //public static class ArduinoHandler
    {
        [Tooltip("Arduino Serial Port name as a string")]
        public string PortName = "COM7";

        [Tooltip("Serial port baud rate")]
        public int BaudRate = 38400;

        [Tooltip("Sensitivity scaler for the ball readout, higher the value, less sensitive it is")]
        public int VelocityScaler = 250;

        [Tooltip("Sensitivity scaler for the ball readout, higher the value, less sensitive it is")]
        public float HorizontalLookScaler = 350;


        SerialPort stream = new SerialPort();
        string serialLine;
        float lookHorz = 0;
        int velX = 0;
        int velY = 0;
        int[] vel = new int[2];

        // when the script is first uploaded and called
        void Start() 
        {
            stream.PortName = PortName;
            stream.BaudRate = BaudRate;
            stream.ReadTimeout = 10;
            stream.DtrEnable = true;
            stream.RtsEnable = true;
            stream.Open();
            Debug.Log("Port opened");
        }
        // Start is called before the first frame update
        //void Start()
        //{
        
        //}

        // Update is called once per frame
        //void Update()
        //{
        
        //}
        //public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout.PositiveInfinity) 
        //{
        //    DateTime initialTime = DateTime.Now;
        //    DateTime nowTime;
        //    TimeSpan diff = default(TimeSpan);

        //    string dataString = null;

        //    while (diff.Milliseconds < timeout) {
        //        try {
        //            dataString = stream.ReadLine();
        //        }
        //        catch (TimeoutException) {
        //            dataString.null;
        //        }

        //        if (dataString != null) { 
        //            callback(dataString);
        //            yield break;
        //        }   else {
        //            yield return null;
        //            }
            
        //        nowTime = DateTime.Now;
        //        diff = nowTime - initialTime;
        //    } 
            
        //    if (fail != null)
        //        fail();
        //    yield return null;
        //}

        public string ReadLineArduino()
        {
            stream.DiscardInBuffer();
            stream.DiscardOutBuffer();
            string nullString = "0_0_0";
            try
            {
                return stream.ReadLine();
            }
            catch (TimeoutException)
            {
                return nullString;
            }
        }

        public int[] GetBallVelocity()
        {
            serialLine = this.ReadLineArduino();
            //Debug.Log(serialLine);
            velX = int.Parse(serialLine.Split('_')[1]);
            velX = velX / VelocityScaler;
            vel[0] = velX;

            velY = int.Parse(serialLine.Split('_')[0]);
            velY = velY/ VelocityScaler;
            vel[1] = velY;

            return vel;
        }

        public float HorizontalLook()
        {
            //stream.DiscardInBuffer();
            //stream.DiscardOutBuffer();

            serialLine = this.ReadLineArduino();
            //Debug.Log(serialLine);
            lookHorz = float.Parse(serialLine.Split('_')[2]);
            lookHorz = lookHorz / HorizontalLookScaler;
            //lookHorz = 1;
            return lookHorz;
        }
        //public static int GetBallVelocityX() 
        //{
        //    serialLine = this.ReadLineArduino();
        //    velX = int.Parse(serialLine.Split('_')[1]);
        //    velX = velX / VelocityScaler;
        //    //Debug.Log(velX);
        //    return velX;
        //}
        //public int GetBallVelocityY()
        //{
        //    serialLine = this.ReadLineArduino();
        //    velX = int.Parse(serialLine.Split('_')[0]);
        //    velX = velX / VelocityScaler;
        //    //Debug.Log(velX);
        //    return velX;
        //}

        void onDestroy()
        {
            stream.Close();
        }
}