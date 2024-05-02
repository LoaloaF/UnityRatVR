using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortentaInputInterface : MonoBehaviour
{
    private CyclicPackagesSHMInterface portentaInputSHMInterface;
    private bool onoff = false;

    // Start is called before the first frame update
    void Start()
    {    
        portentaInputSHMInterface = new CyclicPackagesSHMInterface("portentainput_shmstruct.json");
    }

    public void sendSwitchAirvalve()
    {
        string s1 = "A1\r\n";
        string s2 = "A0\r\n";
        send(onoff ? s1 : s2);
        onoff = !onoff;
    }
    public void sendSuccess(int rewardDelay, int rewardLength)
    {
        send($"S{rewardLength},{rewardDelay}\r\n");
    }
    
    public void sendFailure()
    {
        send("F\r\n");
    }
    
    public void sendPunishment(int punishmentLength)
    {
        send($"P{punishmentLength}\r\n");
    }

    private void send(string cmd)
    {
        Debug.Log($"Sent {cmd} to portenta-input SHM");
        portentaInputSHMInterface.Push(cmd);
    }
}
