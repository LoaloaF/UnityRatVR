// Jeremias Baur, 16.10.2023

using Experiment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{
    private GameObject player;
    private int agentMode = 0;
    public int AgentMode
    {
        get { return agentMode; }
        set { agentMode = value; Behavior(); }
    }

    private bool spawned = false;

    private GameObject experimentManager;
    private NavMeshAgent agent;
    private bool agentHasGoal = false;
    

    private System.Random rand;

    public float scaler;

    private Coroutine attentionSeekingCoroutine;
    public float integrator=0;
    public bool attentionSeeking = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        experimentManager = GameObject.FindWithTag("ExperimentManager");
        agent = GetComponent<NavMeshAgent>();

        rand = new System.Random();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            agentMode = 0;
        } else if (Input.GetKeyDown (KeyCode.Alpha1))
        {
            agentMode = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            attentionSeeking = true;
            agentMode = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            agentMode = 3;
        }

        Behavior();
    }

    void Behavior()
    {
        switch (agentMode)
        {
            case 0: // static mode
                if (attentionSeekingCoroutine != null)
                {
                    StopCoroutine(attentionSeekingCoroutine);
                    attentionSeekingCoroutine = null;
                }
                StaticAgentMode();
                break;
            case 1: // movement no collision mode
                if (attentionSeekingCoroutine != null)
                {
                    StopCoroutine(attentionSeekingCoroutine);
                    attentionSeekingCoroutine = null;
                }

                agent.avoidancePriority = 1;
                RandomMovement();                
                break;
            case 2: // movement to checkpoint with coorporation
                MovementCoorporation();
                break;
            case 3: // competitive with player
                break;
            default: 
                break;

        }
    }

    void StaticAgentMode()
    {
        if (!spawned)
        {
            spawned = true;
            transform.position = player.transform.TransformPoint(Vector3.forward * (-10)) + Vector3.up;
            //transform.position.Set(transform.position.x, 2f, transform.position.z);
        }
    }

    void RandomMovement()
    {
        if ((agent.remainingDistance < 0.3 && !agent.pathPending) || rand.Next(2000)==6)
        {
            Debug.Log("New random destination for agent");
            // GameObject newGoal = experimentManager.GetComponent<PillarManager>().GetRandomPillar();
            // agent.SetDestination(newGoal.transform.position-Vector3.up * newGoal.transform.position.y);
            
            agentHasGoal = true;
        }

        // collision avoidance
        Vector3 dir = (transform.position - player.transform.position);
        if (dir.sqrMagnitude < 14) {
            agent.Move(Vector3.Normalize(dir) * 0.15f / (0.1f + dir.magnitude));
        }
    }

    void MovementCoorporation()
    {
        if (attentionSeeking && attentionSeekingCoroutine==null)
        {
            attentionSeekingCoroutine = StartCoroutine(AttentionSeekingMovement());
        }
        else if (!attentionSeeking)
        {
            if (attentionSeekingCoroutine != null)
            {
                StopCoroutine(attentionSeekingCoroutine);
                attentionSeekingCoroutine = null;
            }
            
            if ((agent.remainingDistance < 0.2 && !agent.pathPending))
            {
                // GameObject newGoal = experimentManager.GetComponent<PillarManager>().ActiveCheckpointPillar.gameObject;
                // agent.SetDestination(newGoal.transform.position - Vector3.up * newGoal.transform.position.y);
                agentHasGoal = true;
            }

            // collision avoidance
            Vector3 dir = (transform.position - player.transform.position);
            if (dir.sqrMagnitude < 16)
            {
                agent.Move(Vector3.Normalize(dir) * 0.15f / (0.1f + dir.magnitude));
            }
        }
    }

    private IEnumerator AttentionSeekingMovement()
    {
        while (true)
        {
            attentionSeeking = true;
            Vector3 distance2Player = transform.position - player.transform.position;
            // dot product with forward vector of player and the vector from the player to the agent to figure out if the agent is in FOV
            float currentViewingDirection = Vector3.Dot(player.transform.TransformDirection(Vector3.forward).normalized, distance2Player.normalized);

            if (currentViewingDirection > 0.7f && distance2Player.sqrMagnitude < 20)
            {
                integrator += .1f;
                if (integrator > 4f)
                {
                    attentionSeeking = false;
                    integrator = 0f;
                    agent.ResetPath();
                    yield break;
                }
                yield return new WaitForSeconds(.1f);
                continue;
            }

            // agent should be 4 units in front of player to attract attention
            Vector3 goalPoint = player.transform.position + player.transform.TransformVector(Vector3.forward * 4f);
            // if agent is behind player
            /*if (currentViewingDirection < -0.3f)
            {
                goalPoint = player.transform.position + player.transform.TransformVector(Vector3.right * 7f);
            }*/

            Vector3 distance2Goal = goalPoint - player.transform.position;
            goalPoint.y = 1;

            Debug.Log(string.Format("Distance2Player: {0},currentViewingDirection: {1},goalPoint: {2}", distance2Player.magnitude.ToString(), currentViewingDirection.ToString(), goalPoint.ToString()));

            agent.SetDestination(goalPoint);
            yield return new WaitForSecondsRealtime(2f);
        }
    }
}
