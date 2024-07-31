using UnityEngine;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Plant
{
    public class FlowerSpawner : MonoBehaviour
    {
        [SerializeField] private List<SplineFollower> m_Flowers;

        public void SpawnFlower(SplineComputer spline, LeafSpawner leafSpawner)
        {
            var flower = Instantiate(m_Flowers[Random.Range(0, m_Flowers.Count)]);

            flower.spline = spline;
            leafSpawner.SetFlowerSpawned(true);
            StartCoroutine(ResetObjects(flower));
        }

        private IEnumerator ResetObjects(SplineFollower follower)
        {
            yield return new WaitForSeconds(.5f);

            Destroy(follower);
        }
    }
}
