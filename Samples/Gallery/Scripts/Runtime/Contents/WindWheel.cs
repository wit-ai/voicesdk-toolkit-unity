using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oculus.Voice.Toolkit.Samples
{
    public class WindWheel : MonoBehaviour
    {
        [SerializeField] private Transform _volumeVis;
        [SerializeField] private float _minVol = 0;
        [SerializeField] private float _maxVol = 0.5f;

        [SerializeField] private float _minScale = 1;
        [SerializeField] private float _maxScale = 2;
        [SerializeField] private float _speed = 0;
        [SerializeField] private float _speedLimit = 20;
        [SerializeField] private float _speedFriction = 0.95f;

        public void ListeningHandler(VoiceDataBase dataObj)
        {
            ListeningData data = dataObj as ListeningData;
            float normal = Mathf.InverseLerp(_minVol, _maxVol, data.micLevel);
            _speed += Mathf.Lerp(_minScale, _maxScale, normal);
            Mathf.Clamp(_speed, 0, _speedLimit);
        }

        void Update()
        {
            _volumeVis.localEulerAngles += new Vector3(0, 0, _speed * Time.deltaTime);
            _speed *= _speedFriction;
        }
    }

}
