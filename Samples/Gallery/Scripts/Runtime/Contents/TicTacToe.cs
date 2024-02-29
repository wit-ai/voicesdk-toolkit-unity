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
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;
using TMPro;
using Random = System.Random;

namespace Oculus.Voice.Toolkit.Samples
{
    public class TicTacToe : MonoBehaviour,IResponseState
    {

        [SerializeField]private CollisionInvocation _collisionInvocation;
        [SerializeField]private TimerInvocation _timerInvocation;
        [SerializeField]private TMP_Text _dictationLabel;
        [SerializeField]private GameObject[] _items;
        [SerializeField]private GameObject _botInterface;
        [SerializeField]private GameObject _playerInterface;
        [SerializeField]private GameObject _gameOverInterface;
        [SerializeField]private TMP_Text _gameOverLabel;
        [SerializeField]private GameObject _button;
        [SerializeField]private Transform _container;
        [SerializeField]private AudioSource _audioSource;
        [SerializeField]private AudioClip[] _resultAC;
        private const string _tieResult = "It's a tie";
        private const string _timeoutResult = "Time out";
        private const string _userLeaveGameResult = "You left the game";
        private const string _playerWinResult = "You win";
        private const string _botWinResult = "You win";
        private const string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private Dictionary<string, Vector3> GridLayout = new Dictionary<string, Vector3>();
        private int[] _board = new int[9];
        private List<List<int>> _ruleBook = new List<List<int>>();
        private int _robotIndex = 1;
        private int _playerIndex = 2;
        private int _moveCount = 0;
        private bool _isPlayerTurn = false;
        private bool isStarted = false;
        private void Awake()
        {
            GridPositionGenerator();
            CreateRule();
            CreateBoard();
            InterfaceInitialized();
        }

        protected void OnEnable()
        {
            _collisionInvocation.On += OnHandler;
            _collisionInvocation.Off += OffHandler;
        }

        protected void OnDisable()
        {
            _collisionInvocation.On -= OnHandler;
            _collisionInvocation.Off -= OffHandler;
        }
        private void OnHandler()
        {
            if (!isStarted)
            {
                _button.SetActive(true);
            }
        }
        private void OffHandler()
        {
            if (isStarted)
            {
                StartCoroutine(GameOver(-2));
            }
            _button.SetActive(false);
        }

        #region Initialization
        private void CreateRule()
        {
            _ruleBook.Clear();
            _ruleBook.Add(new List<int>(){0,1,2});
            _ruleBook.Add(new List<int>(){3,4,5});
            _ruleBook.Add(new List<int>(){6,7,8});
            _ruleBook.Add(new List<int>(){0,3,6});
            _ruleBook.Add(new List<int>(){1,4,7});
            _ruleBook.Add(new List<int>(){2,5,8});
            _ruleBook.Add(new List<int>(){0,4,8});
            _ruleBook.Add(new List<int>(){2,4,6});
        }
        private void GridPositionGenerator()
        {
            float size = 0.11f;
            GridLayout.Clear();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Vector3 pos = new Vector3( (x-1)*size,(3-y-2)*size, -0.025f);
                    GridLayout.Add($"{_letters[y]}{x+1}", pos);
                }
            }
        }

        private void CreateBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                _board[i] = 0;
            }

            if (_container.childCount > 0)
            {
                for (var i = _container.childCount - 1; i >= 0; --i) {
                    Destroy(_container.GetChild(i).gameObject);
                }
                _container.DetachChildren();
            }
        }

        private void InterfaceInitialized()
        {
            _gameOverInterface.SetActive(false);
            _botInterface.SetActive(false);
            _playerInterface.SetActive(false);
        }
        #endregion

        public void StartGame()
        {
            _gameOverInterface.SetActive(false);
            _botInterface.SetActive(false);
            _playerInterface.SetActive(false);
            BotTurn();
            _timerInvocation.StartTimers();
            isStarted = true;
        }

        IEnumerator GameOver(int who)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverInterface.SetActive(true);
            if (who == -2)
            {
                _gameOverLabel.text = _userLeaveGameResult;
            }else if (who == -1)
            {
                _gameOverLabel.text = _timeoutResult;
            }else if (who == 0)
            {
                _gameOverLabel.text = _tieResult;
            }
            else
            {
                _gameOverLabel.text = who == _playerIndex ? _playerWinResult : _botWinResult;
                _audioSource.PlayOneShot(who == _playerIndex?_resultAC[0]: _resultAC[1]);
            }
            _dictationLabel.text = "...";
            _timerInvocation.StopTimers();
            _moveCount = 0;
            CreateBoard();
            isStarted = false;
            if (who != -2)
            {
                _button.SetActive(true);
            }
        }

        void BotTurn()
        {
            _isPlayerTurn = false;
            StartCoroutine(BotTurnDelay());
        }

        IEnumerator BotTurnDelay()
        {
            yield return new WaitForSeconds(1f);
            int positionIndex = FindEmptySpot();
            Place(ConvertPosiitonIndexToString(positionIndex),_robotIndex);
            yield return new WaitForSeconds(1f);
            EndTurn();
        }
        //Used on timerInvocation callback
        public void BotTimeout()
        {
            StartCoroutine(GameOver(-1));
        }
        //Used on timerInvocation callback
        public void PlayerTimeout()
        {
            StartCoroutine(GameOver(-1));
        }

        void PlayerTurn()
        {
            _isPlayerTurn = true;
            //TimerInvocation kick starts, and wait for user's response
        }
        void EndTurn()
        {
            _moveCount++;
            //check winner
            foreach (var line in _ruleBook)
            {
                if (_board[line[0]] != 0 && _board[line[0]] == _board[line[1]] && _board[line[0]] == _board[line[2]])
                {
                    StartCoroutine(GameOver(_board[line[0]]));
                    return;
                }
            }
            //check withdraw
            if(_moveCount >= 9)
            {
                StartCoroutine(GameOver(0));
                return;
            }

            // Change Sides
            if (_isPlayerTurn)
            {
                BotTurn();
            }
            else
            {
                PlayerTurn();
            }
            _timerInvocation.SkipAndMoveNext();
        }
        int FindEmptySpot()
        {
            List<int> empty = new List<int>();
            for (int i = 0; i < _board.Length; i++)
            {
                if (_board[i] == 0)
                {
                    empty.Add(i);
                }
            }
            var random = new Random();
            int index = random.Next(empty.Count);
            return empty[index];
        }

        string ConvertPosiitonIndexToString(int index)
        {
            int px = index % 3;
            int py = Mathf.CeilToInt(index / 3);
            return $"{_letters[py]}{px+1}";
        }
        int ConvertPositionStringToIndex(string location)
        {
            int row = char.ToUpper(location[0]) - 64;//index A = 1
            int column = Int32.Parse(location[1].ToString());
            int output = (row - 1) * 3 + (column - 1);
            return output;
        }

        bool Place(string location, int who)
        {
            int pos = ConvertPositionStringToIndex(location);
            if (_board != null && (pos <0 || pos>=_board.Length || _board[pos] != 0))
            {
                return false;
            }
            GameObject newItem = Instantiate(_items[who-1],_container);
            newItem.transform.localPosition = GridLayout[location];
            newItem.transform.localScale = Vector3.one;
            _board[pos] = who;
            return true;
        }
        #region VoiceUI events
        public void ListeningHandler(VoiceDataBase dataObj)
        {
            ListeningData data = dataObj as ListeningData;
            _dictationLabel.text = string.IsNullOrEmpty(data.transcription) ? "..." : data.transcription;
        }

        public void ResponseHandler(VoiceDataBase dataObj)
        {
            NLUResponseData data = dataObj as NLUResponseData;
            if (data.response.AsObject.HasChild(WitResultUtilities.WIT_KEY_INTENTS) &&
                data.response.AsObject.HasChild(WitResultUtilities.WIT_KEY_ENTITIES))
            {
                string actionEntity = WitResultUtilities.GetFirstEntityValue(data.response, "grid_location:grid_location");
                if (actionEntity != String.Empty)
                {
                    bool success = Place(actionEntity,_playerIndex);
                    if (success)
                    {
                        EndTurn();
                    }
                }
            }
        }
        #endregion
    }

}
