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

using System.Text;
using UnityEngine;
using TMPro;

namespace Oculus.Voice.Toolkit.Samples
{
    public class ThoughtBubble : MonoBehaviour
    {
        [SerializeField]private TextMeshProUGUI _thoughtText;
        [SerializeField]private Transform _mainDisplay;
        [SerializeField]private Transform _bubble1;
        [SerializeField]private Transform _bubble2;
        [SerializeField]private Transform _oppyHeadBone;
        [SerializeField]private float scaleMultipier = 0.7f;
        [SerializeField]private float bubbleOffsetY = 0.5f;
        Vector3 b2Offset = Vector3.zero;
        float countdownTimer = 0.0f;

        private void Awake()
        {
            _thoughtText.fontMaterial.renderQueue = 4501;
            b2Offset = _bubble2.localPosition;
            Hide();
        }

        void Update()
        {
            ForceSizeUpdate();

            // animate things in code
            _bubble1.transform.localRotation = Quaternion.Euler(0, 0, Time.time * 10);
            _bubble2.transform.localRotation = Quaternion.Euler(0, 0, -Time.time * 15);

            _bubble1.transform.localPosition = Vector3.right * Mathf.Sin(Time.time * 1.6f) * 0.005f;
            _bubble2.transform.localPosition = b2Offset + Vector3.right * Mathf.Cos(Time.time * 1.5f) * 0.01f;

            _mainDisplay.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time) * 5);

            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0.0f)
            {
                UpdateText("...");
            }
        }

        public void ForceSizeUpdate()
        {
            if (_oppyHeadBone)
            {
                transform.position = _oppyHeadBone.position - _oppyHeadBone.right * bubbleOffsetY;
            }

            Vector3 objFwd = transform.position - Camera.main.transform.position;

            // face camera
            transform.rotation = Quaternion.LookRotation(objFwd, Vector3.up);

            // keep uniform size on screen
            objFwd.y = 0;
            transform.localScale = Vector3.one * Mathf.Clamp(objFwd.magnitude * scaleMultipier, 0.8f, 4.0f);
        }

        public void UpdateText(string message, float thoughtDuration = 2)
        {
            countdownTimer = thoughtDuration;
            _thoughtText.text = message;
            gameObject.SetActive(true);
        }

        public void ShowHint(float hintDuration = 5)
        {
            countdownTimer = hintDuration;
            StringBuilder sb = new StringBuilder();
            sb.Append(GetColorString("#000000", "Ask me to "));
            sb.Append(GetColorString("#FF0000", "jump "));
            sb.Append(GetColorString("#000000", "or<br> say "));
            sb.Append(GetColorString("#FF0000", "hi "));
            sb.Append(GetColorString("#000000", "to me"));
            _thoughtText.text = sb.ToString();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        string GetColorString(string color, string word)
        {
            return $"<color={color}>{word}</color>";
        }
    }
}
