using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniRx.Examples
{
    public class PositionDisplay : MonoBehaviour
    {
        [SerializeField] private string topic;
        [SerializeField] private Text titleText;
        [SerializeField] private Text dataText;

        private void Start()
        {
            titleText.text = topic;
            dataText.text = "";

            TopicBroker.Default.Receive(topic).Subscribe(p => {
                var position = (Vector3)p;

                if (position != null)
                {
                    Debug.Log(position);
                    dataText.text = position.ToString();
                }
            });
        }
    }
}
