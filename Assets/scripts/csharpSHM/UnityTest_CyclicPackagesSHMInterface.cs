using System;
using System.Collections;
using System.Collections.Generic;
// using static CyclicPackagesSHMInterface;
using UnityEngine;
using System.IO; // For StreamReader and FileNotFoundException

public class UnityTest_CyclicPackagesSHMInterface : MonoBehaviour
{
    CyclicPackagesSHMInterface ballVelSHMInterface;

    void Start()
    {
        ballVelSHMInterface = new CyclicPackagesSHMInterface("../tmp_shm_structure_JSONs/ballvelocity_shmstruct.json");
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // int[] frameBallVel = new int[3];
        string frameBallVel = "";
        int[] ballVel = new int[3];
        int ballVelPackID;

        while (true)
        {
            // (ballVel, ballVelPackID) = ballVelSHMInterface.fastPopBallVelocity();
            // if (ballVelPackID == -1) break;
            // frameBallVel[0] += ballVel[0];
            // frameBallVel[1] += ballVel[1];
            // frameBallVel[2] += ballVel[2];

            var output = ballVelSHMInterface.Popitem();
            if (output == null) break;
            frameBallVel = output;
            i++;
            if (i>1000) break;
        }
        stopwatch.Stop();
        Debug.Log($"Got {i} BVs in {stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000)} μs: {frameBallVel}");

    }
}