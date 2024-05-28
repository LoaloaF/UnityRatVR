using UnityEngine;
using RatVR.ExcelData;
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
    public string trialPackageVariables;
    public string sessionDescription;
    public bool sessionRunning = false;
    public bool trialRunning = false;
    public bool abortTrialFlag = false;


    # region Paradigm specific variables
    public float nextTrialEndTeleportCenterDist = 35f; // TD
    public float nextTrialEndTeleportCenterAngle = 0f; // TA
    # endregion
    
    [HideInInspector] public float trialStartTimestamp;
    private int trialStartFrameID;
    private long trialStartTimestampMicroseconds;
    private long trialEndTimestampMicroseconds;
    private long trialDuration;

    // Triallogging information
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
        this.trialPackageVariables = sessionMetaData.trialPackageVariables;
        this.sessionDescription = sessionMetaData.sessionDescription;
    }
    
    public void updateTrialEndTeleportCenterAngle(float newTrialEndTeleportCenterAngle) 
    {
        if (newTrialEndTeleportCenterAngle > -360 && newTrialEndTeleportCenterAngle < 360)
            nextTrialEndTeleportCenterAngle = newTrialEndTeleportCenterAngle;
        else
            Debug.Log($"newTrialEndTeleportCenterAngle {newTrialEndTeleportCenterAngle} is out of bounds");
    }

    public void updateTrialEndTeleportCenterDist(float newTrialEndTeleportCenterDist) 
    {
        if (newTrialEndTeleportCenterDist >= 0 && newTrialEndTeleportCenterDist <= 100)
            nextTrialEndTeleportCenterDist = newTrialEndTeleportCenterDist;
        else
            Debug.Log($"newTrialEndTeleportCenterDist {newTrialEndTeleportCenterDist} is out of bounds");
    }

    public void newTrial() {
        trialCount++;
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
}
