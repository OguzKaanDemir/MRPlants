using UnityEngine;
using System.Linq;
using Scripts.Tracking;
using Dreamteck.Splines;
using System.Collections.Generic;

namespace Scripts.Plant
{
    public class PlantBodySpawner : MonoBehaviour
    {
        [SerializeField] private SplineComputer m_SplinePrefab;
        [SerializeField] private Transform m_TargetPositionTransform;
        [SerializeField] private float m_MinPointDistance;
        [SerializeField] private int m_MaxBodyLength;

        private PinchDetector m_Hand;
        private List<Vector3> m_Positions = new();
        private List<SplinePoint> m_Points = new();

        private SplineComputer m_LastSpline;

        private bool m_ShouldRePinch;

        private void Start()
        {
            m_Hand = GetComponent<PinchDetector>();
        }

        private void Update()
        {
            HandlePinch();
        }

        private void HandlePinch()
        {
            if (m_Hand && m_Hand.IsPinching && !m_ShouldRePinch)
            {
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
        }

        private void AddSplinePoint()
        {
            m_Points = m_LastSpline.GetPoints().ToList();

            m_Points.Add(new SplinePoint(m_Positions[^1]));
            m_LastSpline.SetPoints(m_Points.ToArray());
            m_LastSpline.Rebuild();
        }

        private void ResetPinch()
        {
            m_Positions.Clear();
            m_ShouldRePinch = false;
        }
    }
}