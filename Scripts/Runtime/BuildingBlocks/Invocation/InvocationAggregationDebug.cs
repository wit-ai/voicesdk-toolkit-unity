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
using Meta.WitAi;

namespace Oculus.Voice.Toolkit
{
    [RequireComponent(typeof(InvocationAggregation))]
    public class InvocationAggregationDebug : MonoBehaviour
    {
        [SerializeField] protected InvocationAggregation invocationAggregation;
        [SerializeField] protected VoiceService voiceService;
        [SerializeField] private Vector3 _offset;

        protected virtual void OnValidate()
        {
            if (!invocationAggregation) invocationAggregation = gameObject.GetComponent<InvocationAggregation>();
            if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
        }

        protected void Awake()
        {
            if (!invocationAggregation) invocationAggregation = gameObject.GetComponent<InvocationAggregation>();
            if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
        }


        //Debug Visualizaton
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = invocationAggregation.IsOn ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position+transform.InverseTransformDirection(_offset), 0.01f);

            Gizmos.color = voiceService.MicActive ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + Vector3.up * 0.05f+transform.InverseTransformDirection(_offset), 0.01f);

            Gizmos.color = voiceService.Active? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + Vector3.up * 0.1f+transform.InverseTransformDirection(_offset), 0.01f);
        }
    }
}
