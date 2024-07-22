using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class PillarCollision : MonoBehaviour
{
    public bool PlayerDetected;
    private float colliderRadius;
    private Vector3 colliderBottomPoint;
    private Vector3 colliderTopPoint;
    private bool playerDetected;
    public BaseStateMachine stateMachine;

    void Start()
    {
        PlayerDetected = false;
        Transform ColliderTransform = transform.Find("Collider");
        colliderRadius = ColliderTransform.localScale.x / 2 / 1.5f ;
        colliderTopPoint = new Vector3(ColliderTransform.position.x, transform.localPosition.y*2, ColliderTransform.position.z);
        colliderBottomPoint = new Vector3(ColliderTransform.position.x, 0, ColliderTransform.position.z);
        
        stateMachine = GetComponentInParent<BaseStateMachine>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(colliderBottomPoint, colliderRadius);
        Gizmos.DrawWireSphere(colliderTopPoint, colliderRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stateMachine._sessionManager.trialRunning)
        {
            // UnityEngine.Debug.Log("trial not running");
            return;
        }
        
        playerDetected = false;
        // Collider[] hitColliders = Physics.OverlapBox(transform.position, detectionBoxSize / 2);
        Collider[] hitColliders = Physics.OverlapCapsule(colliderBottomPoint, colliderTopPoint, colliderRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                PlayerDetected = true;
                playerDetected = true;
                break;
            }
        }
        if (!playerDetected)
        {
            PlayerDetected = false;
        }
    }
}
    /*
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player detected in pillar collision script");
            //return true;
        }
        else
        {
            //return false;
        }
        
    }
    */

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("player is detected in pillar collision script");
    //         // Debug.Log(transform.position);
    //         PlayerDetected = true;
    //     }

    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("player exits collider");
    //         // Debug.Log(transform.position);
    //         PlayerDetected = false;
    //     }
    // }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("player is detected in pillar collision script");
    //         // Debug.Log(transform.position);
    //         PlayerDetected = true;
    //     }

    // }


