using UnityEngine;
using Dreamteck.Splines;
using System.Collections.Generic;
using System.Collections;

namespace Scripts.Plant
{
    public class FlowerSpawner : MonoBehaviour
    {
        [SerializeField] private List<SplineFollower> m_Flowers;

        public void SpawnFlower(SplineComputer spline)
        {
            var flower = Instantiate(m_Flowers[Random.Range(0, m_Flowers.Count)]);

            flower.spline = spline;
            StartCoroutine(ResetObjects(spline, flower));
        }

        private IEnumerator ResetObjects(SplineComputer spline, SplineFollower follower)
        {
            yield return new WaitForSeconds(.5f);

            Destroy(follower);
            Destroy(spline.GetComponent<TubeGenerator>());
            Destroy(spline);
        }
    }
}
