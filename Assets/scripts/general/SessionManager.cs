using UnityEngine;

/// <summary>
/// This class manages provided a reference to session parameters from the excel sheet
/// And it manages single trial logging
/// </summary>
public sealed class SessionManager : MonoBehaviour
{        
    // TODO: needs link to excel sheet
    // staticSessionParameters
    public  bool sessionRunning = false;
    public  float successSequenceLength = 3.3f;
    public  float maximumTrialLength = 30.4f;
    public  float interTrialIntervalTrialLength = 1;

    // TODO: needs link to excel sheet
    // dynamicSessionParameters
    public  static float trialEndTeleportCenterDistMin = 7.1f;
    public  static float trialEndTeleportCenterDistMax = 20f;
    public  static float trialEndTeleportCenterAngleMin = 0f;
    public  static float trialEndTeleportCenterAngleMax = 0f;
    public  static int[] rewardedPillars = new int[] {1};
    public  static float[] pillarTransparency = new float[] {1};
    // public  int[] rewardedPillars = new int[] {1, 2, 3, 4};
    // public  float[] pillarTransparency = new float[] {1, 1, 1, 1};

    // set from outside (FSM actions) not all paradigms will use all of these
    public  float nextTrialEndTeleportCenterDist = trialEndTeleportCenterDistMin;
    public  float nextTrialEndTeleportCenterAngle = trialEndTeleportCenterAngleMin;
    public  int[] nextTrialRewardedPillars = rewardedPillars;
    public  float[] nextTrialPillarTransparency = pillarTransparency;

    // general trial logging parameters
    private float trialStartTimestamp = 0f;
    private float previousTrialDuration = -1f;
    private int trialCount = 0;
    private string trialPackage = "";

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

    public void logNewTrial(string packageValues) {
        trialStartTimestamp = Time.realtimeSinceStartup;
        trialPackage = $"N:TN,ID:{trialCount},FID:{Time.frameCount},"+
                       $"PCT:{Time.realtimeSinceStartup},{packageValues}";

        Debug.Log($"Calling Push with New Trial Pckg {trialPackage}");
        GetComponent<UnityFrameLogger>().unityOutputSHMInterface.Push("<{"+trialPackage+"}>\r\n");
        trialCount++;
    }

    public void logEndTrial(string packageValues) {
        previousTrialDuration = Time.realtimeSinceStartup - trialStartTimestamp;
        trialPackage = $"N:TE,ID:{trialCount},FID:{Time.frameCount},"+
                       $"PCT:{Time.realtimeSinceStartup},TD:{previousTrialDuration},"+
                       $"{packageValues}";

        Debug.Log($"Calling Push with End Trial Pckg {trialPackage}");
        GetComponent<UnityFrameLogger>().unityOutputSHMInterface.Push("<{"+trialPackage+"}>\r\n");
    }
}
