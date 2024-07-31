using DG.Tweening;
using UnityEngine;

namespace Scripts.Plant
{
    public class Leaf : MonoBehaviour
    {
        private bool m_IsTriggered;

        private void Start()
        {
            transform.DOScale(Vector3.one, LeafSpawnSettings.Ins.GetAnimationDuration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Flower") && !m_IsTriggered)
            {
                m_IsTriggered = true;

                transform.DOKill();
                Destroy(gameObject);
            }
        }
    }
}
