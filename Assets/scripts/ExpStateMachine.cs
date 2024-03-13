using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpStateMachine : MonoBehaviour
{
    public GameObject player;
    public CharacterController playerController;

    // public AudioSource audioSrc;

    private List<Vector3> velocityHistory = new List<Vector3>();
    private Vector3 velocitySum;
    private float toCheckVelSum;
    
    public float MinInterRewardInterval = 2f;
    private float deltaTimeLastReward = 10000f;

    private bool forwardOnly = false;
    public int rewardVelThreshold = 30;
    CyclicPackagesSHMInterface portInputSHMInterface;

    // Start is called before the first frame update
    void Start()
    {
        portInputSHMInterface = new CyclicPackagesSHMInterface("../tmp_shm_structure_JSONs/portentainput_shmstruct.json");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(player.transform.position);
        // Debug.Log(playerController.velocity);

        velocityHistory.Add(playerController.velocity);
        if (velocityHistory.Count > 5)
        {
            velocityHistory.
            RemoveAt(0);
        }
        velocitySum = Vector3.zero;
        velocityHistory.ForEach(vel => velocitySum += vel);

        
        // Debug.Log(velocitySum);
        if (forwardOnly) {
            toCheckVelSum = Math.Abs(velocitySum.y);
        } else {
            toCheckVelSum = Mathf.Max(Math.Abs(velocitySum.y), Math.Abs(velocitySum.x), Math.Abs(velocitySum.z));
        }
        
        // Debug.Log(toCheckVelSum);
        if (toCheckVelSum > rewardVelThreshold && deltaTimeLastReward > MinInterRewardInterval) {
            deltaTimeLastReward = 0;
            // Debug.Log("REWARD!!");
            Debug.Log("Reward");
            portInputSHMInterface.Push($"R100,100");

            // audioSrc.Play();
        } else {
            deltaTimeLastReward += Time.deltaTime;
        }
        // Debug.Log(toCheckVelSum);
        // Debug.Log(rewardVelThreshold);
        // Debug.Log(deltaTimeLastReward);
        // Debug.Log(MinInterRewardInterval);
        // Debug.Log("-------------------");

        // velocityHistory.ForEach(p => Debug.Log(p));
        // Debug.Log("-----------------");
        // Debug.Log(velocitySum);
        // Debug.Log("=================");
    }
}
