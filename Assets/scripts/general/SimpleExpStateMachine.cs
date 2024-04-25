using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleExpStateMachine : MonoBehaviour
{
    public GameObject player;
    public CharacterController playerController;

    private List<Vector3> velocityHistory = new List<Vector3>();
    private Vector3 velocitySum;
    private float toCheckVelSum;
    
    public float MinInterRewardInterval = 2f;
    private float deltaTimeLastReward = 10000f; // arbitrary large number to avoid reward at start

    public bool forwardOnly = false;
    public int rewardVelThreshold = 30;
    CyclicPackagesSHMInterface portInputSHMInterface;

    // Start is called before the first frame update
    void Start()
    {
        portInputSHMInterface = new CyclicPackagesSHMInterface("portentainput_shmstruct.json");
    }

    // Update is called once per frame
    void Update()
    {
        velocityHistory.Add(playerController.velocity);
        if (velocityHistory.Count > 5)
        {
            velocityHistory.RemoveAt(0);
        }
        velocitySum = Vector3.zero;
        velocityHistory.ForEach(vel => velocitySum += vel);
        
        // Debug.Log(velocitySum);
        if (forwardOnly) {
            toCheckVelSum = velocitySum.z;
        } else {
            toCheckVelSum = Mathf.Max(Math.Abs(velocitySum.y), Math.Abs(velocitySum.x), Math.Abs(velocitySum.z));
        }
        
        if (toCheckVelSum > rewardVelThreshold && deltaTimeLastReward > MinInterRewardInterval) {
            deltaTimeLastReward = 0;
            Debug.Log("Reward");

            if (!(portInputSHMInterface == null)) {
                portInputSHMInterface.Push($"R100,100");
            }
            else {
                Debug.Log("SHM not linked. Can't write reward");
            }
        } else {
            deltaTimeLastReward += Time.deltaTime;
        }
    }
}
