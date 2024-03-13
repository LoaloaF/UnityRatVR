// The script interfacing the Arduino with 3 ball rotation sensors to Unity scene
// Author: Eminhan Ozil & Simon Steffens
// ETH Zurich

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class ArduinoHandler : MonoBehaviour {
        [Tooltip("Arduino Serial Port name as a string")]
        [SerializeField] string PortName = "COM7";

        [Tooltip("Serial port baud rate")]
        [SerializeField] int BaudRate = 38400;

        SerialPort stream = new SerialPort();
        private string serialLineStr;
        private string[] serialLineArr;
        private int[] velXYZ = new int[3];

        void Start() {
            setPortParameters();
            openPort();
        }

        private void setPortParameters() {
            stream.PortName = PortName;
            stream.BaudRate = BaudRate;
            stream.ReadTimeout = 10;
            stream.WriteTimeout = 10;
            stream.DtrEnable = true;
            stream.RtsEnable = true;
        }

        private void openPort() {
            try {
                stream.Open();
                Debug.Log("Port opened");
                stream.DiscardInBuffer();
                stream.DiscardOutBuffer();
        }
        catch (Exception e) {
                Debug.LogError(String.Format("Could not open Port  `{0}`:\n{1}", stream.PortName, e));
            }
        }

        public void WriteLineArduino(string msg = "REWARD_MESSAGE") {
            stream.WriteLine(msg);
        }

        public string ReadLineArduino() {
            //stream.DiscardInBuffer();
            //stream.DiscardOutBuffer();
            try {
                return stream.ReadLine();
            }
            catch (TimeoutException e) {
                return "0_0_0";
            }
            // catch (OverflowException e) {
            //     return "0_0_0";
            // }
        }

        public int[] GetBallXYZVelocities() {
            serialLineStr = this.ReadLineArduino();
            //Debug.Log("serialLineStr: " + serialLineStr);
            serialLineArr = serialLineStr.Split('_');

            for (int i = 0; i < 3; i++) {
                try {
                    velXYZ[i] = int.Parse(serialLineArr[i]);
                } catch (Exception e)  {
                    velXYZ[i] = -1;
                }   
            }
            return velXYZ;
        }

        private void OnApplicationQuit() {
            stream.Close();
            Debug.Log("Steam closed");
        }
}