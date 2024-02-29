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
    public class TeleportSpot : MonoBehaviour
    {
        [SerializeField]private string _stationName;
        [SerializeField] private Transform _stationVis;
        [SerializeField] private float _forwardOffset = 0.5f;
        private Vector3 _frontBottomPoint = new Vector3(0,-0.5f,-0.5f);
        private GameObject _teleportSpot;
        public string StationName => _stationName;
        public Transform GetSpot => _teleportSpot.transform;
        protected void OnValidate()
        {
            if (!_stationVis) _stationVis = transform.Find("Station - Vis");
        }

        protected void Awake()
        {
            CreateTeleportSpot();
        }

        private void CreateTeleportSpot()
        {
            if (!_stationVis) return;
            if (!_teleportSpot)
            {
                _teleportSpot = new GameObject();
                _teleportSpot.name = "TeleportAnchor";
            }
            _teleportSpot.transform.SetParent(transform.parent);
            _teleportSpot.transform.localScale = Vector3.one;
            _teleportSpot.transform.position = transform.position +
                                               _stationVis.transform.parent.TransformVector(Vector3.Scale(_stationVis.transform.localScale,new Vector3(0,-1,-0.5f)))
                                               + _forwardOffset * Vector3.Normalize(_stationVis.transform.forward);
            Vector3 lookAtRotation = Quaternion.LookRotation(transform.position -_teleportSpot.transform.position)
                .eulerAngles;
            _teleportSpot.transform.rotation = Quaternion.Euler(Vector3.Scale(lookAtRotation, new Vector3(0,1,0)));

        }

        #if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (_teleportSpot)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_teleportSpot.transform.position, 0.2f);
            }
        }
        #endif

    }
}
