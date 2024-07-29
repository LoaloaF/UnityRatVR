using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P0300_LandmarkFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform playerPosition;
    private Vector3 initPosition;

    [HideInInspector] public bool finishInit = false;
    [HideInInspector] public Vector3 initLandmarkPosition = new Vector3(0, 0, 0);

    void Start()
    {
        playerPosition = GameObject.Find("Player").transform;
        initPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (finishInit)
        {
            if (gameObject.name == "LandmarkX")
            {
                transform.position = new Vector3(playerPosition.position.x - initPosition.x + initLandmarkPosition.x, transform.position.y, playerPosition.position.z);
            }
            else if (gameObject.name == "LandmarkY")
            {
                transform.position = new Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z - initPosition.z + initLandmarkPosition.z);
            }
        }

    }
}
