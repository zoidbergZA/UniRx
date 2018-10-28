using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx.Examples
{
    public class PositionSender : MonoBehaviour
    {
        [SerializeField] private string topic;

        private Vector3 lastPosition;

        private void Start()
        {
            lastPosition = transform.position;
            TopicBroker.Default.Publish(topic, lastPosition);
        }

        private void Update()
        {
            if (transform.position != lastPosition)
            {
                lastPosition = transform.position;

                TopicBroker.Default.Publish(topic, lastPosition);
            }
        }
    }
}
