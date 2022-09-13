/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEngine;
using Facebook.WitAi.Lib;
using Facebook.WitAi.Events;
using Facebook.WitAi;
using System;
using System.Collections.Generic;

namespace Oculus.Voice.Toolkit
{
    public class VoiceUXAdapter : MonoBehaviour
    {
        [SerializeField] protected VoiceService voiceService;
        public Action<VoiceState, VoiceDataBase> voiceUIEvent;

        [Header("Voice Data")]
        private NLUResponseData _responseStateData;
        private ListeningData _listeningData ;
        //TODO: this is the temp variable , will need to remove once the miclevelchanged start to send normalized data
        private float _micLevelMultiplyer = 100;

        protected virtual void OnValidate()
        {
            if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
        }

        protected virtual void Awake()
        {
            if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
        }

        protected virtual void OnEnable()
        {
            if (!voiceService) voiceService = FindObjectOfType<VoiceService>();
            voiceService.VoiceEvents.OnResponse.AddListener(ResponseHandler);
            voiceService.VoiceEvents.OnStartListening.AddListener(StartListeningHandler);
            voiceService.VoiceEvents.OnStoppedListening.AddListener(StopListeningHandler);
            voiceService.VoiceEvents.OnMicDataSent.AddListener(MicDataSentHandler);
            voiceService.VoiceEvents.OnPartialTranscription.AddListener(TranscriptionHandler);
            voiceService.VoiceEvents.OnFullTranscription.AddListener(TranscriptionHandler);
            voiceService.VoiceEvents.OnMicLevelChanged.AddListener(MicLevelChangedHandler);

            voiceService.VoiceEvents.OnError.AddListener(ErrorHandler);
        }

        protected virtual void OnDisable()
        {
            voiceService.VoiceEvents.OnResponse.RemoveListener(ResponseHandler);
            voiceService.VoiceEvents.OnStartListening.RemoveListener(StartListeningHandler);
            voiceService.VoiceEvents.OnStoppedListening.RemoveListener(StopListeningHandler);
            voiceService.VoiceEvents.OnMicDataSent.RemoveListener(MicDataSentHandler);
            voiceService.VoiceEvents.OnPartialTranscription.RemoveListener(TranscriptionHandler);
            voiceService.VoiceEvents.OnFullTranscription.RemoveListener(TranscriptionHandler);
            voiceService.VoiceEvents.OnMicLevelChanged.RemoveListener(MicLevelChangedHandler);
            voiceService.VoiceEvents.OnError.RemoveListener(ErrorHandler);
        }
        public void Activate()
        {
            voiceService.Activate();
        }

        public void Deactivate()
        {
            voiceService.Deactivate();
        }

        private void TranscriptionHandler(string trascription)
        {
            if (_listeningData == null)
            {
                return;
            }
            _listeningData.transcription = trascription;
            voiceUIEvent?.Invoke(VoiceState.Listening, _listeningData);
        }

        private void MicLevelChangedHandler(float micLevel)
        {
            if (_listeningData == null)
            {
                return;
            }
            _listeningData.micLevel = micLevel * _micLevelMultiplyer;
            voiceUIEvent?.Invoke(VoiceState.Listening, _listeningData);
        }

        #region VoiceSDK
        private void StartListeningHandler()
        {
            if (_listeningData == null) _listeningData = new ListeningData();
            _responseStateData = null;
            voiceUIEvent?.Invoke(VoiceState.MicOn, null);
        }
        private void StopListeningHandler()
        {
            voiceUIEvent?.Invoke(VoiceState.MicOff, null);
            _listeningData = null;

            //TODO: it's hack, need to figure out why it doesn't get OnMicDataSent call back.
            MicDataSentHandler();
        }
        private void MicDataSentHandler()
        {
            _responseStateData = new NLUResponseData();
            _responseStateData.requestTime = DateTime.UtcNow;
            _responseStateData.responseTime = DateTime.UtcNow;
            voiceUIEvent?.Invoke(VoiceState.StartProcessing, _responseStateData);
        }

        private void ResponseHandler(WitResponseNode responseData)
        {
            if (_responseStateData != null  && responseData!= null)
            {
                _responseStateData.responseTime = DateTime.UtcNow;
                _responseStateData.response = responseData;
                voiceUIEvent?.Invoke(VoiceState.Response, _responseStateData);
            }
        }

        private void ErrorHandler(string type, string message)
        {
            ErrorData data = new ErrorData(type, message);
            voiceUIEvent?.Invoke(VoiceState.Error, data);
        }
        #endregion VoiceSDK
    }
}
