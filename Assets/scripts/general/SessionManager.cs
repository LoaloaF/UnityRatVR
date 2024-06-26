using RatVR.ExcelData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// This class manages provided a reference to session parameters from the excel sheet
/// And it manages single trial logging
/// </summary>
public sealed class SessionManager : MonoBehaviour
{        
    public int rewardPostSoundDelay;
    public int rewardAmount;
    public int punishmentLength;
    public int punishmentInactivationLength;
    public string onWallZoneEntry;
    public string onInterTrialInterval;
    public float interTrialIntervalLength;
    public int abortInterTrialIntervalLength;
    public float successSequenceLength;
    public float maximumTrialLength;
    public string sessionDescription;

    public Dictionary<string, string> trialVariablesDict = new Dictionary<string, string>();
    private string[] trialVariablesNamesArray;
    private string[] trialVariablesDefaultArray;

    [HideInInspector] public bool sessionRunning = false;
    [HideInInspector] public bool trialRunning = false;
    [HideInInspector] public bool abortTrialFlag = false;
    [HideInInspector] public int _currentTrialID = -1;

    // Triallogging information
    [HideInInspector] public float trialStartTimestamp;
    private int trialStartFrameID;
    private long trialStartTimestampMicroseconds;
    private long trialEndTimestampMicroseconds;
    private long trialDuration;
    private int trialCount = 0;
    private string trialPackage = "";


    public void Start()
    {
        sessionRunning = false;
    }

    public void InitializeSessionManager(ExcelSessionMetaData sessionMetaData)
    {
        this.rewardPostSoundDelay = sessionMetaData.rewardPostSoundDelay;
        this.rewardAmount = sessionMetaData.rewardAmount;
        this.punishmentLength = sessionMetaData.punishmentLength;
        this.punishmentInactivationLength = sessionMetaData.punishmentInactivationLength;
        this.onWallZoneEntry = sessionMetaData.onWallZoneEntry;
        this.onInterTrialInterval = sessionMetaData.onInterTrialInterval;
        this.interTrialIntervalLength = sessionMetaData.interTrialIntervalLength;
        this.abortInterTrialIntervalLength = sessionMetaData.abortInterTrialIntervalLength;
        this.successSequenceLength = sessionMetaData.successSequenceLength;
        this.maximumTrialLength = sessionMetaData.maximumTrialLength;
        this.sessionDescription = sessionMetaData.sessionDescription;

        this.trialVariablesNamesArray = sessionMetaData.trialPackageVariables.Split(',');
        this.trialVariablesDefaultArray = sessionMetaData.trialPackageVariablesDefault.Split(',');

        for (int i = 0; i < trialVariablesNamesArray.Length; i++)
        {
            trialVariablesDict[trialVariablesNamesArray[i]] = trialVariablesDefaultArray[i];
            Debug.Log($"Trial Variable {trialVariablesNamesArray[i]} initialized with {trialVariablesDefaultArray[i]}");
        }


        this._currentTrialID = -1;
        trialRunning = false;

    }

    public void UpdateTrialVariable(string variableName, string variableValue)
    {
        if (trialVariablesDict.ContainsKey(variableName))
        {
            trialVariablesDict[variableName] = variableValue;
            Debug.Log($"Trial Variable {variableName} updated with {variableValue}");
        }
        else
        {
            Debug.Log($"Trial Variable {variableName} not found");
        }
    }
    public void updateTrialEndTeleportCenterAngle(float newTrialEndTeleportCenterAngle) 
    {
        UpdateTrialVariable("PA", newTrialEndTeleportCenterAngle.ToString());
    }

    public void updateTrialEndTeleportCenterDist(float newTrialEndTeleportCenterDist) 
    {
        UpdateTrialVariable("PD", newTrialEndTeleportCenterDist.ToString());
    }


    public void newTrial() {
        trialCount++;
        this._currentTrialID = trialCount;
        trialStartTimestamp = Time.realtimeSinceStartup;
        trialStartTimestampMicroseconds = getUnixTimestampMicroseconds();
        trialStartFrameID = Time.frameCount;
    }

    public long getUnixTimestampMicroseconds() {
        DateTime currentDateTime = DateTime.UtcNow;
        // Calculate the Unix timestamp in milliseconds
        long unixTimestampMilliseconds = ((DateTimeOffset)currentDateTime).ToUnixTimeMilliseconds();
        // Calculate the microseconds part
        long microseconds = currentDateTime.Millisecond * 1000 + (DateTime.Now.Ticks % TimeSpan.TicksPerMillisecond) / 10;
        // Combine milliseconds and microseconds
        long unixTimestampMicroseconds = unixTimestampMilliseconds * 1000 + microseconds;
        return unixTimestampMicroseconds;
    }

    public void logEndTrial(int outcome = -1, string paradigmTrialSpecficValues="") {
        trialEndTimestampMicroseconds = getUnixTimestampMicroseconds();
        trialDuration = trialEndTimestampMicroseconds-trialStartTimestampMicroseconds;

        trialPackage = $"N:T,ID:{trialCount},SFID:{trialStartFrameID},"+
                       $"SPCT:{trialStartTimestampMicroseconds},EFID:{Time.frameCount},"+
                       $"EPCT:{trialEndTimestampMicroseconds},TD:{trialDuration},"+
                       $"O:{outcome}{paradigmTrialSpecficValues}";

        Debug.Log($"Calling Push with End Trial Pckg {trialPackage}");
        GetComponent<UnityFrameLogger>().unityOutputSHMInterface.Push("<{"+trialPackage+"}>\r\n");
    }


    public void ClearSession()
    {
        GameObject[] pillars = GameObject.FindGameObjectsWithTag("Pillar");
        foreach (GameObject pillar in pillars)
        {
            Destroy(pillar);
        }

        GameObject[] landmarks = GameObject.FindGameObjectsWithTag("Landmark");
        foreach (GameObject landmark in landmarks)
        {
            Destroy(landmark);
        }

    }
}
