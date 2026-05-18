using RatVR.ExcelData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using FSM;
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
    public PlayerMovement _playerMovement;

    public Dictionary<string, string> trialVariablesDict = new Dictionary<string, string>();
    public Dictionary<string, string> trialLogDict = new Dictionary<string, string>();
    private string[] trialVariablesNamesArray;
    private string[] trialVariablesDefaultArray;

    [HideInInspector] public bool sessionRunning = false;
    [HideInInspector] public bool trialRunning = false;
    [HideInInspector] public bool abortTrialFlag = false;
    [HideInInspector] public int _currentTrialID = -1;
    [HideInInspector] public int currentRewardNum = 0;
    [HideInInspector] public bool rewardPresent = false;
    [HideInInspector] public bool rewardSucked = false;
    [HideInInspector] public int successfulTrials = 0; //only in P1300 
    [HideInInspector] public int failedTrials = 0; //only in P1300

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
        rewardPresent = false;
        currentRewardNum = 0;
        rewardSucked = false;
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

    public void UpdateTrialVariable(string variableJsonString)
    {
        Debug.Log($"Updating Trial Variables with {variableJsonString}");
        Dictionary<string, string> tempDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(variableJsonString);
        foreach (var item in tempDict)
        {
            if (trialVariablesDict.ContainsKey(item.Key))
            {
                trialVariablesDict[item.Key] = item.Value;
                Debug.Log($"Trial Variable {item.Key} updated with {item.Value}");
            }
            else
            {
                Debug.Log($"Trial Variable {item.Key} not found in updating");
            }
        }
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
        long ticksSinceEpoch = currentDateTime.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        long unixTimestampMicroseconds = ticksSinceEpoch / 10;
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

    public void Add_Decimal(BaseStateMachine stateMachine, string variableName)
    {
        float variable = float.Parse(stateMachine._sessionManager.trialVariablesDict[variableName]);
        if (variable % 1 == 0)
            stateMachine._sessionManager.trialVariablesDict[variableName] = variable.ToString() + ".0";
        else
            stateMachine._sessionManager.trialVariablesDict[variableName] = variable.ToString();
    }
    
}
