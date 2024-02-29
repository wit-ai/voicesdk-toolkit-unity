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
using UnityEngine.UI;

namespace Oculus.Voice.Toolkit.MicrophoneState
{
    public class ScaledUIMicIndicator : VolumeMicIndicator
    {
        [Header("Effected Transforms")]
        [SerializeField] private Transform[] scaledTransforms;

        [Header("Graphic Settings")]
        [SerializeField] private Graphic[] graphics;
        [SerializeField] private Color activeColor = Color.white;
        [SerializeField] private Color inactiveColor = Color.white;

        [Header("Scaling")]
        [SerializeField] private bool scaleVertically;
        [SerializeField] private bool scaleHorizontally;

        private Vector3 _initialScale;
        private Color _color;

        public Color GraphicColor
        {
            get => _color;
            set
            {
                _color = value;
                for (int i = 0; i < graphics.Length; i++)
                {
                    graphics[i].color = value;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (null == scaledTransforms || scaledTransforms.Length == 0)
            {
                _initialScale = transform.localScale;
            }
            else if (scaledTransforms.Length == 1)
            {
                _initialScale = scaledTransforms[0].localScale;
            }
            else
            {
                _initialScale = Vector3.one;
            }
            GraphicColor = inactiveColor;
        }

        protected override void OnListening()
        {
            base.OnListening();
            GraphicColor = activeColor;
        }

        protected override void OnStoppedListening()
        {
            base.OnStoppedListening();
            GraphicColor = inactiveColor;
        }

        protected override void Update()
        {
            base.Update();
            var scale = new Vector3(
                _initialScale.x * (scaleHorizontally ? DisplayedLevel : 1),
                _initialScale.y * (scaleVertically ? DisplayedLevel : 1),
                _initialScale.z);

            if (null != scaledTransforms && scaledTransforms.Length > 0)
            {
                for (int i = 0; i < scaledTransforms.Length; i++)
                {
                    scaledTransforms[i].localScale = scale;
                }
            }
            else
            {
                transform.localScale = scale;
            }
        }
    }
}
