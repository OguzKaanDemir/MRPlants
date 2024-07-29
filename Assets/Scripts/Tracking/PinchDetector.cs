using UnityEngine;
using Scripts.Enums;

namespace Scripts.Tracking
{
    public class PinchDetector : MonoBehaviour
    {
        public static (bool isPinching, HandType handType) IsPinching
        {
            get
            {
                if (m_Hand)
                    return (m_Hand.GetFingerIsPinching(OVRHand.HandFinger.Index), m_HandType);

                return (false, m_HandType);
            }
        }

        [SerializeField] private HandType m_HandTypeRef;

        private static OVRHand m_Hand;
        private static HandType m_HandType;

        private void Start()
        {
            m_Hand = GetComponent<OVRHand>();
            m_HandType = m_HandTypeRef;
        }
    }
}