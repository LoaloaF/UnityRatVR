using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experiment
{
    public class PillarManager : MonoBehaviour
    {
        /*
            PillarManager manages all pillars.
            It creates 8*8 pillars and is access point for FSM to activate and check if player is under pillar
         */
        private Dictionary<Tuple<int, int>, GameObject> pillars;
        public Dictionary<Tuple<int, int>, GameObject> Pillars
        {
            get { return pillars; }
        }

        public List<Material> Materials;

        public float distance = 10f;

        public List<Tuple<int, int>> checkpointPillars;
        public Vector3 GroundPosition;

        public Pillar ActiveCheckpointPillar;
        public GameObject Player;

        private System.Random _random;

        private void Start()
        {
            _random = new System.Random();
            pillars = new Dictionary<Tuple<int, int>, GameObject>();
            checkpointPillars = new List<Tuple<int, int>>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var index = new Tuple<int, int>(i, j);
                    GameObject pillar = Instantiate(Resources.Load<GameObject>("Pillar"), this.transform);

                    pillar.name = "Pillar" + index.ToString();
                    pillar.transform.position = new Vector3(i * distance, 20 + UnityEngine.Random.Range(-2, 2), j * distance) + GroundPosition;
                    pillar.GetComponentInChildren<MeshRenderer>().material = Materials[_random.Next(Materials.Count)];
                    pillar.GetComponent<Pillar>().Index = index;

                    pillars.Add(index, pillar);

                    if ((i == 2 || i == 5) && (j == 2 || j == 5))
                    {
                        checkpointPillars.Add(index);
                    }
                }
            }

            ActiveCheckpointPillar = pillars[checkpointPillars[_random.Next(checkpointPillars.Count)]].GetComponent<Pillar>();
        }

        public bool CheckPlayerPillar()
        {
            // TODO: Update so actually collider is checked and not just single point of player object: https://discussions.unity.com/t/check-if-position-is-inside-a-collider/12667/4
            return ActiveCheckpointPillar.GetComponentInChildren<Collider>().bounds.Contains(Player.transform.position);
        }

        public void CreateNewCheckpoint()
        {
            // used by the FSM to generate a new checkpoint

            ActiveCheckpointPillar.DeactivatePillar();

            Tuple<int, int> newIndex;
            do
            {
                newIndex = checkpointPillars[_random.Next(checkpointPillars.Count)];
            } while (newIndex == ActiveCheckpointPillar.Index);
            Debug.Log("New index is " + newIndex.ToString());

            ActiveCheckpointPillar = pillars[newIndex].GetComponent<Pillar>();
            ActiveCheckpointPillar.ActivatePillar();
        }

        public GameObject GetRandomPillar()
        {
            List<Tuple<int,int>> keyList = new List<Tuple<int, int>> (pillars.Keys);
            var newIndex = keyList[_random.Next(keyList.Count)];
            return pillars[newIndex];
        }
    }
}