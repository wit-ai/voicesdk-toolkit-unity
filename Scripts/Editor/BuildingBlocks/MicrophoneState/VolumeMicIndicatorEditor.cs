/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Oculus.Voice.Toolkit.MicrophoneState;
using UnityEditor;
using UnityEngine;

namespace Oculus.Voice.Toolkit.BuildingBlocks.MicrophoneState
{
    [CustomEditor(typeof(VolumeMicIndicator), true)]
    public class VolumeMicIndicatorEditor : Editor
    {
        private VolumeMicIndicator _indicator;

        private void OnEnable()
        {
            _indicator = (VolumeMicIndicator) target;
            _indicator.OnMicFill.AddListener(OnMicFill);
        }

        private void OnDisable()
        {
            _indicator.OnMicFill.RemoveListener(OnMicFill);
        }

        void OnMicFill(float level)
        {
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                GUILayout.Space(16);
                GUILayout.Label("Mic Status");
                _indicator.MicLevel = GUILayout.HorizontalSlider(_indicator.MicLevel,
                    0, 1);

                if (_indicator.MinVol < float.MaxValue && _indicator.MaxVol > float.MinValue)
                {
                    GUILayout.Space(16);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(_indicator.MinVol.ToString("F4"));
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(_indicator.MaxVol.ToString("F4"));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(16);
                }

                GUILayout.Space(16);
            }
        }
    }
}
