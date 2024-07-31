using UnityEngine;
using Dreamteck.Splines;
using System.Collections.Generic;

namespace Scripts.Plant
{
    public class LeafSpawnSettings : MonoBehaviour
    {
        private static LeafSpawnSettings m_Ins;
        public static LeafSpawnSettings Ins
        {
            get
            {
                if(!m_Ins)
                    m_Ins = FindObjectOfType<LeafSpawnSettings>();

                return m_Ins;
            }
        }

        [Header("Leaf Settings")]

        [SerializeField] private List<SplineFollower> m_Leafs;
        [SerializeField, Range(0.0f, 1.0f)] private float m_SpawnRate;
        [SerializeField] private int m_MinSpawnLength;
        [SerializeField] private float m_AnimationDuration;

        public List<SplineFollower> GetLeafs => m_Leafs;
        public float GetSpawnRate => m_SpawnRate;
        public int GetMinSpawnLength => m_MinSpawnLength;
        public float GetAnimationDuration => m_AnimationDuration;
    }
}
