using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortentaInputInterface : MonoBehaviour
{
    private CyclicPackagesSHMInterface portentaInputSHMInterface;

    // Start is called before the first frame update
    void Start()
    {    
        portentaInputSHMInterface = new CyclicPackagesSHMInterface("../tmp_shm_structure_JSONs/portentainput_shmstruct.json");
    }

    public void sendSwitchAirvalve()
    {
        send("A\r\n");
    }
    public void sendSuccess(int rewardDelay, int rewardLength)
    {
        send($"{rewardLength},{rewardDelay}\r\n");
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
