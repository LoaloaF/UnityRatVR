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
    public  float interTrialIntervalTrialLength = 5;

    // TODO: needs link to excel sheet
    // dynamicSessionParameters
    public  float trialEndTeleportCenterDistMin = 10f;
    public  float trialEndTeleportCenterDistMax = 30f;
    public  float trialEndTeleportCenterAngleMin = 0f;
    public  float trialEndTeleportCenterAngleMax = 180f;
    public  int[] rewardedPillars = new int[] {1, 2, 3, 4};

    // set from outside (FSM actions) not all paradigms will use all of these
    public  float currentTrialEndTeleportCenterDist;
    public  float currentTrialEndTeleportCenterAngle;
    public  int currentTrialrewardedPillar;

    // general trial logging parameters
    private float frameTime;
    private int trialCount = 0;
    private string trialPackage;

    public void logNewTrial(string packageValues) {
        frameTime = Time.realtimeSinceStartup;

        trialPackage = $"N:TN,ID:{trialCount},FID:{Time.frameCount},"+
                       $"PCT:{frameTime},{packageValues}";

        Debug.Log($"Calling Push with New Trial Pckg {trialPackage}");
        GetComponent<UnityFrameLogger>().unityOutputSHMInterface.Push("<{"+trialPackage+"}>\r\n");
        trialCount++;
    }

    public void logEndTrial(string packageValues) {
        frameTime = Time.realtimeSinceStartup;

        trialPackage = $"N:TE,ID:{trialCount},FID:{Time.frameCount},"+
                       $"PCT:{frameTime},{packageValues}";

        Debug.Log($"Calling Push with End Trial Pckg {trialPackage}");
        GetComponent<UnityFrameLogger>().unityOutputSHMInterface.Push("<{"+trialPackage+"}>\r\n");
    }
}
