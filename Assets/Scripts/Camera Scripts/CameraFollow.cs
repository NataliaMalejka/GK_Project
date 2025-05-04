using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour, IUpdateObserver
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 offset;

        private Vector3 targetPos;

        public void ObserveUpdate()
        {
            if (target == null) return;

            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }

        private void OnEnable()
        {
            UpdateManager.AddToList(this);
        }

        private void OnDisable()
        {
            UpdateManager.RemoveFromList(this);
        }

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;
        }
    }
}
