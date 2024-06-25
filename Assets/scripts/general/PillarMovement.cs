using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class PillarMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public int childcount;
    public float height;
    public float frequency;
    public BaseStateMachine stateMachine;
    void Start()
    {
        stateMachine = GetComponentInParent<BaseStateMachine>();
        childcount = stateMachine.transform.childCount;
        height = 5f;
        frequency = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < childcount; i++)
        {
            Transform child = stateMachine.transform.GetChild(i);
            
            Transform CylinderTransform = child.Find("Cylinder");

            Vector3 pillarPosition = CylinderTransform.position;

            // use the height of pillar when initializing the pillar
            float pillarHeight = stateMachine._sceneController.scene.Pillars[i].Height + stateMachine._sceneController.scene.Pillars[i].Position.z;
            
            float newY = Mathf.Sin(2* Mathf.PI*Time.time * frequency) * height + pillarHeight;

            CylinderTransform.position = new Vector3(pillarPosition.x, newY, pillarPosition.z);
            // UnityEngine.Debug.Log("Pillar moved" + newY);
        }
    }






}
