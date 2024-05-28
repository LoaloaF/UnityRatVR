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
    public  float maximumTrialLength;
    public string trialPackageVariables;
    public int rewardedPillarsMin;
    public int rewardedPillarsMax;
    public float pillarTransparencyMin;
    public float pillarTransparencyMax;
    public int pillarPunishmentMin;
    public int pillarPunishmentMax;

    public  bool sessionRunning = false;
    public  bool trialRunning = false;
    public  bool abortTrialFlag = false;


    // TODO: needs link to excel sheet
    // dynamicSessionParameters
    public  static float trialEndTeleportCenterDistMin = 12f;
    public  static float trialEndTeleportCenterDistMax = 100f;
    public  static float trialEndTeleportCenterAngleMin = -90f;
    public  static float trialEndTeleportCenterAngleMax = 90f;
    public  static int[] rewardedPillars = new int[] {1};
    public  static float[] pillarTransparency = new float[] {1};
    // public  int[] rewardedPillars = new int[] {1, 2, 3, 4};
    // public  float[] pillarTransparency = new float[] {1, 1, 1, 1};

    // set from outside (FSM actions) not all paradigms will use all of these
    public  float nextTrialEndTeleportCenterDist = trialEndTeleportCenterDistMin;
    public  float nextTrialEndTeleportCenterAngle = trialEndTeleportCenterAngleMin;
    public  int[] nextTrialRewardedPillars = rewardedPillars;
    public  float[] nextTrialPillarTransparency = pillarTransparency;
    
    public float trialStartTimestamp;
    public int trialStartFrameID;
    public long trialStartTimestampMicroseconds;
    public long trialEndTimestampMicroseconds;
    private long trialDuration;

    // general trial logging parameters
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
        this.rewardedPillarsMin = sessionMetaData.rewardedPillarsMin;
        this.rewardedPillarsMax = sessionMetaData.rewardedPillarsMax;
        this.pillarTransparencyMin = sessionMetaData.pillarTransparencyMin;
        this.pillarTransparencyMax = sessionMetaData.pillarTransparencyMax;
        this.pillarPunishmentMin = sessionMetaData.pillarPunishmentMin;
        this.pillarPunishmentMax = sessionMetaData.pillarPunishmentMax;
    }
    
    public void updateTrialEndTeleportCenterAngle(float newTrialEndTeleportCenterAngle) 
    {
        if (newTrialEndTeleportCenterAngle > trialEndTeleportCenterAngleMin && 
            newTrialEndTeleportCenterAngle < trialEndTeleportCenterAngleMax)
        {
            nextTrialEndTeleportCenterAngle = newTrialEndTeleportCenterAngle;
        } else {
            Debug.Log($"newTrialEndTeleportCenterAngle {newTrialEndTeleportCenterAngle} is out of bounds");
        }
    }

    public void updateTrialEndTeleportCenterDist(float newTrialEndTeleportCenterDist) 
    {
        if (newTrialEndTeleportCenterDist > trialEndTeleportCenterDistMin && 
            newTrialEndTeleportCenterDist < trialEndTeleportCenterDistMax)
        {
            nextTrialEndTeleportCenterDist = newTrialEndTeleportCenterDist;
        } else {
            Debug.Log($"newTrialEndTeleportCenterDist {newTrialEndTeleportCenterDist} is out of bounds");
        }
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

        Debug.Log($"Calling Push with End Trial Pckg {trialPackage}");

        trialPackage = $"N:T,ID:{trialCount},SFID:{trialStartFrameID},"+
                       $"SPCT:{trialStartTimestampMicroseconds},EFID:{Time.frameCount},"+
                       $"EPCT:{trialEndTimestampMicroseconds},TD:{trialDuration},"+
                       $"O:{outcome}{paradigmTrialSpecficValues}";

        Debug.Log($"Calling Push with End Trial Pckg {trialPackage}");
        GetComponent<UnityFrameLogger>().unityOutputSHMInterface.Push("<{"+trialPackage+"}>\r\n");
    }
}
