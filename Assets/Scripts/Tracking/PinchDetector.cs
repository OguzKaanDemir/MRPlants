using UnityEngine;

namespace Scripts.Tracking
{
    public class PinchDetector : MonoBehaviour
    {
        public bool IsPinching
        {
            get
            {
                if (m_Hand)
                    return m_Hand.GetFingerIsPinching(OVRHand.HandFinger.Index);

                return false;
            }
        }

        private OVRHand m_Hand;

        private void Start()
        {
            m_Hand = GetComponent<OVRHand>();
        }
    }
}