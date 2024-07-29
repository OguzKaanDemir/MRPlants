using UnityEngine;
using Scripts.Tracking;
using System.Collections.Generic;
using Dreamteck.Splines;
using System.Linq;

namespace Scripts.Plant
{
    public class PlantBodySpawner : MonoBehaviour
    {
        [SerializeField] private SplineComputer m_SplinePrefab;
        [SerializeField] private Transform m_TargetPositionTransform;

        private PinchDetector m_Hand;
        private List<Vector3> m_Positions = new();

        private SplineComputer m_LastSpline;

        private bool m_Test;
        private bool m_ShouldRePinch;

        private void Start()
        {
            m_Hand = GetComponent<PinchDetector>();
        }

        private void Update()
        {
            if (m_Hand && m_Hand.IsPinching || !m_ShouldRePinch)
            {
                m_Positions.Add(m_TargetPositionTransform.position);
                if (m_Positions.Count == 1)
                {
                    m_LastSpline = Instantiate(m_SplinePrefab, Vector3.zero, Quaternion.identity);
                    m_LastSpline.runInEditMode = true;
                }

                var points = m_LastSpline.GetPoints().ToList();

                if (m_Positions.Count >= 2)
                {
                    if (Vector3.Distance(m_Positions[^1], m_Positions[m_Positions.Count - 2]) > .5f)
                    {
                        points.Add(new SplinePoint(m_Positions[^1]));
                        m_LastSpline.SetPoints(points.ToArray());
                        m_LastSpline.Rebuild();
                    }
                    else
                    {
                        m_Positions.Remove(m_Positions[^1]);
                    }

                }

                if (m_Positions.Count >= 50)
                    m_ShouldRePinch = true;
            }
            else if (m_Hand && !m_Hand.IsPinching)
            {
                m_Positions.Clear();
                m_ShouldRePinch = false;
            }
        }
    }
}