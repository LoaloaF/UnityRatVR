using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experiment
{
    public class Pillar : MonoBehaviour
    {
        public Tuple<int, int> Index;

        /*public void OnTriggerEnter()
        {
            Debug.Log("hit pillar " + gameObject.name);
        }*/

        public void ActivatePillar()
        {
            MeshRenderer mr = gameObject.GetComponentInChildren<MeshRenderer>();
            mr.material.color = Color.green;
        }

        public void DeactivatePillar()
        {
            MeshRenderer mr = gameObject.GetComponentInChildren<MeshRenderer>();
            mr.material.color = Color.white;
        }
    }
}
