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
using System.Collections;
using UnityEngine;
using Meta.WitAi;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;

namespace Oculus.Voice.Toolkit.Samples
{
    public class TeleportStation : MonoBehaviour,IResponseState, IListeningState
    {

        [SerializeField]private TextMeshProUGUI _locationText;
        [SerializeField]private TextMeshProUGUI _commandText;
        [SerializeField]private TeleportSpot _teleportStation;
        [SerializeField]private CharacterController _player;
        private static string _commandLabelContent = "Did you check all the samples?  Want to try something again? Try saying ";
        private string _commandLabelBeginning = " \"";
        private string _commandDictation = "Teleport me to the";
        private string _commandLabelEnding = " ...\"";
        private List<TeleportSpot> _teleportSpots;
        private Dictionary<string, TeleportSpot> _teleportSpotsDictionary = new Dictionary<string, TeleportSpot>();

        void Awake()
        {
            string content = _locationText.text;
            _teleportSpots = FindObjectsByType<TeleportSpot>(FindObjectsSortMode.None).ToList();
            for (int i = 0; i < _teleportSpots.Count; i++)
            {
                if (_teleportSpots[i] != _teleportStation)
                {
                    _teleportSpotsDictionary.Add(_teleportSpots[i].StationName, _teleportSpots[i]);
                }
            }
            ResetLabel();
        }

        void ResetLabel()
        {
            _locationText.text = TeleportSpotStringBuilder("");
            _commandText.text = CommandStringBuilder("");
        }

        public void ResponseHandler(VoiceDataBase dataObj)
        {
            NLUResponseData data = dataObj as NLUResponseData;
            if (data.response.AsObject.HasChild(WitResultUtilities.WIT_KEY_INTENTS) &&
                data.response.AsObject.HasChild(WitResultUtilities.WIT_KEY_ENTITIES))
            {
                var intent = WitResultUtilities.GetIntentName(data.response);

                string actionEntity = WitResultUtilities.GetFirstEntityValue(data.response, "teleport_location:teleport_location");
                if (actionEntity != String.Empty)
                {
                    bool success = Teleport(actionEntity);
                }
            }
        }

        bool Teleport(string location)
        {
            if (_teleportSpotsDictionary.ContainsKey(location))
            {
                StartCoroutine(TeleportDelay(location));
                return true;
            }
            return false;
        }

        IEnumerator TeleportDelay(string location)
        {
            yield return new WaitForSeconds(0.3f);
            Transform spot = _teleportSpotsDictionary[location].GetSpot;
            _player.enabled = false;
            _player.transform.position = new Vector3(spot.position.x,_player.transform.position.y,spot.position.z);
            _player.transform.rotation = spot.rotation;
            _player.enabled = true;
            ResetLabel();
        }

        public void ListeningHandler(VoiceDataBase dataObj)
        {
            ListeningData data = dataObj as ListeningData;
            _locationText.text = TeleportSpotStringBuilder(data.transcription);
            _commandText.text = CommandStringBuilder(data.transcription);
        }

        string TeleportSpotStringBuilder(string transcription, string highlightColor = "#0196FF", string defaultColor = "#FFFFFF")
        {
            StringBuilder output = new StringBuilder();
            string trans = transcription.ToLower();
            int index = 0;
            foreach(var stationName in _teleportSpotsDictionary.Keys)
            {
                StringBuilder newStationName = new StringBuilder();
                string[] stationNameWords = stationName.Split(' ');
                for (int i = 0; i < stationNameWords.Length; i++)
                {
                    if (trans.Contains(stationNameWords[i].ToLower()))
                    {
                        newStationName.Append(GetColorString(highlightColor,stationNameWords[i]));
                    }
                    else
                    {
                        newStationName.Append(GetColorString(defaultColor,stationNameWords[i]));
                    }
                    newStationName.Append(" ");
                }
                output.Append($"({index}) {newStationName}");
                index++;
            }

            return output.ToString();
        }

        string CommandStringBuilder(string transcription, string highlightColor = "#0196FF",  string defaultColor = "#FFFFFF")
        {
            StringBuilder newDictation = new StringBuilder();
            string trans = transcription.ToLower();
            string[] words = _commandDictation.Split(' ');
            for (int i = 0; i< words.Length; i++)
            {
                if (trans.Contains(words[i].ToLower()))
                {
                    newDictation.Append(GetColorString(highlightColor,words[i]));
                    newDictation.Append(" ");
                }
                else
                {
                    newDictation.Append(GetColorString(defaultColor, words[i]));
                    newDictation.Append(" ");
                }
            }
            StringBuilder output = new StringBuilder();
            output.Append(_commandLabelContent);
            output.Append(GetColorString(defaultColor, _commandLabelBeginning));
            output.Append(newDictation.ToString());
            output.Append(GetColorString(defaultColor, _commandLabelEnding));
            return output.ToString();
        }

        string GetColorString(string color, string word)
        {
            return $"<color={color}>{word}</color>";
        }
    }
}
