/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
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
