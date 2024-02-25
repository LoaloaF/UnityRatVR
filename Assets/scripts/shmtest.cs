using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.IO; // For StreamReader and FileNotFoundException

public class testshm : MonoBehaviour
{
    Int64 id;
    Int64 prv_id;
    CyclicPackagesSHMInterface interfaceObj;

    // Start is called before the first frame update
    void Start()
    {
        string sensorsShmStrucFname = "./Assets/scripts/SensorsCyclicTestSHM_shmstruct.json";
        if (File.Exists(sensorsShmStrucFname))
        {
            Debug.Log("File exists.");
        }
        else
        {
            Debug.Log("File does not exist.");
        }

        interfaceObj = new CyclicPackagesSHMInterface(sensorsShmStrucFname);
        Debug.Log(sensorsShmStrucFname);

        // read test
        id = 0;
        prv_id = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // string? item = interfaceObj.PopItem();
        Dictionary<string, object>? item = interfaceObj.PopExtractedItem();

        // Console.WriteLine(item);
        // Console.WriteLine("{" + string.Join(", ", item.Select(kvp => kvp.Key + ": " + kvp.Value.ToString())) + "}");
        if (item != null)
        {
            if (item["N"].ToString() == "BV")
            {
                // Console.WriteLine("{" + string.Join(", ", item.Select(kvp => kvp.Key + ": " + kvp.Value.ToString())) + "}");
                Debug.Log(item["V"]);

                id = (Int64)item["ID"];
                if (id - 1 != prv_id)
                {
                    Debug.Log($"Error: ID jump from {prv_id} to {id}!");
                }
                prv_id = (Int64)item["ID"];
            }
        }
        else if (item != null && item["N"] == "ER")
        {
            Debug.Log(item["N"]);
            Debug.Log(item["V"]);
            Debug.Log("");
        }
        else
        {
            Debug.Log(".");
        }
        Thread.Sleep(1);

        // interfaceObj.Dispose(); // Don't forget to dispose the resources
    }
}
