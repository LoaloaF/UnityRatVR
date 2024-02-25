using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logWriter : MonoBehaviour
{
    [SerializeField] string overrideFilename = "";
    [SerializeField] bool clearBeforeRun = true;

    private string fullPath;
    TextWriter tw;

    void Start()
    {
        createLogFile();
        tw = new StreamWriter(fullPath, true);
    }

    private void createLogFile() {
        string filename;
        if (string.IsNullOrEmpty(overrideFilename)) {
            filename = DateTime.Now.ToString("s").Replace(":", "-");
        } else {
            filename = overrideFilename;
        }
        filename += ".csv";

        fullPath = Path.Combine(Application.dataPath, "..", "Logs", filename);
        if (clearBeforeRun) {
            File.WriteAllText(fullPath, string.Empty);
        }
        Debug.Log(fullPath);
    }

    public void write(string logString) {
        
        tw.WriteLine(logString);
    }

    private void OnApplicationQuit()
    {
        tw.Close();
    }
}
