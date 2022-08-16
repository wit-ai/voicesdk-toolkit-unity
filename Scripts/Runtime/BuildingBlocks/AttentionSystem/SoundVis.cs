/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEngine;
namespace Oculus.Voice.Toolkit
{
    public class SoundVis : MonoBehaviour, IListeningState
    {
        [SerializeField] private Transform _volumeVis;
        [SerializeField] private float _minVol = 0;
        [SerializeField] private float _maxVol = 0.5f;

        [SerializeField] private float _minScale = 1;
        [SerializeField] private float _maxScale = 2;
        private Vector3 _targetScale;
        private bool _isAnimated = false;

        public void ListeningHandler(VoiceDataBase dataObj)
        {
            ListeningData data = dataObj as ListeningData;
            float normal = Mathf.InverseLerp(_minVol, _maxVol, data.micLevel);
            float value = Mathf.Lerp(_minScale, _maxScale, normal);
            _targetScale = value * Vector3.one;
        }

        public void MicOnHandler(VoiceDataBase data)
        {
            _targetScale = _minScale * Vector3.one;
            _volumeVis.localScale = _targetScale;
            _isAnimated = true;
        }

        public void MicOffHandler(VoiceDataBase data)
        {
            _isAnimated = false;
            _targetScale = _minScale * Vector3.one;
            _volumeVis.localScale = _targetScale;

        }
        void Update()
        {
            if (_isAnimated)
            {
                _volumeVis.localScale += (_targetScale - _volumeVis.localScale) / 10;
            }
        }
    }

}
