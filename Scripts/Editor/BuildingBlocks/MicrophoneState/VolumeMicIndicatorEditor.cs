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
