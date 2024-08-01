using UnityEngine;
using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Plant
{
    public class LeafSpawner : MonoBehaviour
    {
        private Queue<SplineRequest> m_RequestQueue = new Queue<SplineRequest>();
        private bool m_IsProcessing = false;
        private SplineComputer m_Spline;

        private LeafSpawnSettings m_PlantSpawner;
        private bool m_IsFlowerSpawned;

        private void Start()
        {
            m_PlantSpawner = LeafSpawnSettings.Ins;
            m_Spline = GetComponent<SplineComputer>();
            m_IsProcessing = false;
        }

        private void Update()
        {
            if (!m_IsProcessing && m_IsFlowerSpawned)
            {
                Destroy(m_Spline.GetComponent<TubeGenerator>());
                Destroy(m_Spline);
                Destroy(this);
            }
        }

        public void SetFlowerSpawned(bool flowerSpawned)
        {
            m_IsFlowerSpawned = flowerSpawned;
        }

        public void AddNewLeaf(SplinePoint point)
        {
            if (m_Spline.GetPoints().Length < m_PlantSpawner.GetMinSpawnLength || Random.Range(0.0f, 1.0f) > m_PlantSpawner.GetSpawnRate) return;

            m_RequestQueue.Enqueue(new SplineRequest(point));

            if (!m_IsProcessing)
            {
                StartCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            m_IsProcessing = true;

            while (m_RequestQueue.Count > 0)
            {
                SplineRequest request = m_RequestQueue.Dequeue();

                yield return StartCoroutine(NewLeaf(request.point));
            }

            m_IsProcessing = false;
        }

        private IEnumerator NewLeaf(SplinePoint point)
        {
            var leaf = CreateLeaf();

            yield return new WaitForSeconds(.1f);
            SetLeaf(leaf, point);
            Destroy(leaf);

            yield return new WaitForSeconds(m_PlantSpawner.GetAnimationDuration / 3);
        }

        private SplineFollower CreateLeaf()
        {
            var leafs = m_PlantSpawner.GetLeafs;
            var leaf = Instantiate(leafs[Random.Range(0, leafs.Count)]);

            leaf.spline = m_Spline;

            return leaf;
        }

        private void SetLeaf(SplineFollower leaf, SplinePoint point)
        {
            var sample = new SplineSample();
            leaf.spline.Project(point.position, ref sample);
            leaf.SetPercent(sample.percent);
            leaf.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
        }

        private class SplineRequest
        {
            public SplinePoint point;

            public SplineRequest(SplinePoint point)
            {
                this.point = point;
            }
        }
    }
}
