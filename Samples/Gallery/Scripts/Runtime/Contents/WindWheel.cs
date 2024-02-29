/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
