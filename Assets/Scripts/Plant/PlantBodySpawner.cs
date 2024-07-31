using UnityEngine;
using System.Linq;
using Scripts.Tracking;
using Dreamteck.Splines;
using System.Collections.Generic;

namespace Scripts.Plant
{
    public class PlantBodySpawner : MonoBehaviour
    {
        [SerializeField] private PinchDetector m_Hand;
        [SerializeField] private FlowerSpawner m_FlowerSpawner;
        [SerializeField] private SplineComputer m_SplinePrefab;
        [SerializeField] private Transform m_TargetPositionTransform;
        [SerializeField] private float m_MinPointDistance;
        [SerializeField] private int m_MaxBodyLength;

        private LeafSpawner m_LeafSpawner;
        private List<Vector3> m_Positions = new();
        private List<SplinePoint> m_Points = new();

        private SplineComputer m_LastSpline;

        private bool m_ShouldRePinch;
        private bool m_PinchStarted;

        private void Update()
        {
            HandlePinch();
        }

        private void HandlePinch()
        {
            if (m_Hand && m_Hand.IsPinching && !m_ShouldRePinch)
            {
                m_PinchStarted = true;
                m_Positions.Add(m_TargetPositionTransform.position);

                if (m_Positions.Count == 1)
                {
                    CreateSpline();
                    AddSplinePoint();
                }

                else if (Vector3.Distance(m_Positions[^1], m_Positions[^2]) > m_MinPointDistance)
                    AddSplinePoint();

                else
                    m_Positions.RemoveAt(m_Positions.Count - 1);

                if (m_Positions.Count >= m_MaxBodyLength)
                    m_ShouldRePinch = true;
            }
            else if (m_Hand && !m_Hand.IsPinching)
            {
                ResetPinch();
            }
        }

        private void CreateSpline()
        {
            m_LastSpline = Instantiate(m_SplinePrefab, Vector3.zero, Quaternion.identity);
            m_LeafSpawner = m_LastSpline.GetComponent<LeafSpawner>();
        }

        private void AddSplinePoint()
        {
            m_Points = m_LastSpline.GetPoints().ToList();

            var point = new SplinePoint(m_Positions[^1]);

            m_Points.Add(point);
            m_LastSpline.SetPoints(m_Points.ToArray());
            m_LastSpline.Rebuild();

            m_LeafSpawner.AddNewLeaf(point);
        }

        private void ResetPinch()
        {
            m_Positions.Clear();
            m_ShouldRePinch = false;

            if (m_PinchStarted)
                m_FlowerSpawner.SpawnFlower(m_LastSpline, m_LeafSpawner);

            m_PinchStarted = false;
        }
    }
}