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

using System;
using Meta.WitAi;
using UnityEngine;

namespace Oculus.Voice.Toolkit.Samples
{
    public class Oppy : MonoBehaviour,IResponseState, IListeningState
    {
        [SerializeField] private AudioSource _as;
        [SerializeField] private AudioClip[] _clips;
        [SerializeField] private Animator animator;
        [SerializeField] private ThoughtBubble _thoughtBubble;

        public void PlaySound(int soundIndex)
        {
            if(_as.enabled == false) return;
            if (soundIndex < _clips.Length)
            {
                _as.PlayOneShot(_clips[soundIndex]);
            }
            else
            {
                Debug.Log("Error: invalid sound index");
            }
        }

        public void ListeningHandler(VoiceDataBase dataObj)
        {
            ListeningData data = dataObj as ListeningData;
            if (data != null && !String.IsNullOrEmpty(data.transcription))
            {
                _thoughtBubble.UpdateText(data.transcription);
            }
        }

        public void ResponseHandler(VoiceDataBase dataObj)
        {
            NLUResponseData data = dataObj as NLUResponseData;
            if (data.response.AsObject.HasChild(WitResultUtilities.WIT_KEY_INTENTS) &&
                data.response.AsObject.HasChild(WitResultUtilities.WIT_KEY_ENTITIES))
            {
                var intent = WitResultUtilities.GetIntentName(data.response);
                string actionEntity = WitResultUtilities.GetFirstEntityValue(data.response, "Action:Action");
                if (intent == "Oppy_Action" && actionEntity == "Jump")
                {
                    Oppy_Jump();
                    return;
                }
            }
            if (data.response.AsObject.HasChild(WitResultUtilities.WIT_KEY_TRAITS))
            {

                string trait = data.response?[WitResultUtilities.WIT_KEY_TRAITS]?["wit$greetings"]?[0]?["value"]?.Value;
                bool flag;
                if (Boolean.TryParse(trait, out flag))
                {
                    if (flag)
                    {
                        Oppy_Greeting();
                        return;
                    }
                }
            }
            FailListening();
        }

        public void ResetOppy()
        {
            animator.SetBool("Listening",false);
            ResetTriggers();
        }

        #region oppyAnimation Handlers
        public void Oppy_Greeting()
        {
            PlaySound(3);
            animator.SetTrigger("Wave");
            animator.SetBool("Listening",false);
        }
        public void Oppy_Jump()
        {
            PlaySound(2);
            animator.SetTrigger("Jumping");
            animator.SetBool("Listening",false);
        }

        public void StartListening()
        {
            PlaySound(0);
            ResetTriggers();
            animator.SetBool("Listening", true);
        }

        public void StopListening()
        {
            ResetTriggers();
            animator.SetBool("Listening", false);
        }

        public void FailListening()
        {
            PlaySound(1);
            animator.SetTrigger("ListeningFail");
            animator.SetBool("Listening",false);
        }

        void ResetTriggers()
        {
            animator.ResetTrigger("Jumping");
            animator.ResetTrigger("Wave");
            animator.ResetTrigger("ListeningFail");
        }

        #endregion
    }

}
