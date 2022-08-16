/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEngine;
using TMPro;
namespace Oculus.Voice.Toolkit
{
    public class Dictation : MonoBehaviour, IListeningState
    {
        [SerializeField] private TMP_Text _transcriptionTextField;
        [SerializeField] private Color _idelColor;
        public void ListeningHandler(VoiceDataBase dataObj)
        {
            ListeningData data = dataObj as ListeningData;
            _transcriptionTextField.text = string.IsNullOrEmpty(data.transcription) ? "..." : data.transcription;
            _transcriptionTextField.color = Color.white;
        }
        public void MicOnHandler()
        {
            _transcriptionTextField.text = "...";
            _transcriptionTextField.color = _idelColor;
        }
    }
}
