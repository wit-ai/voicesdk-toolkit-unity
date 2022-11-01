/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oculus.Voice.Toolkit.Samples
{
    public class UIMenu : MonoBehaviour
    {
        //Menu
        [SerializeField]private KeyCode _showTeleportMenuKey;
        private bool _showMenu = false;
        //Teleport

        private List<TeleportSpot> _teleportSpots;
        [SerializeField]private CharacterController _player;
        private void Awake()
        {
            _teleportSpots = FindObjectsOfType<TeleportSpot>().ToList();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_showTeleportMenuKey))
            {
                _showMenu = !_showMenu;
            }
        }

#if UNITY_EDITOR
        void OnGUI()
        {
            GUI.contentColor = Color.grey;

            GUI.Label(new Rect(5, 10, 250, 20), $"Use {_showTeleportMenuKey} to show control help");
            if (_showMenu)
            {
                GUI.Label(new Rect(5, 35, 250, 20), "WASD keys to walk");
                GUI.Label(new Rect(5, 55, 250, 20), "Mouse left button hold to rotate");
                GUI.Label(new Rect(5, 75, 250, 20), "Teleport to ...");
                GUI.contentColor = Color.white;
                int row = 5;
                int width = 130;
                int height = 20;
                for (int i = 0; i < _teleportSpots.Count; i++)
                {
                    if (GUI.Button(new Rect(5 + Mathf.CeilToInt(i/row)*width, 100 + i%row *height, width, height), $"{_teleportSpots[i].StationName}"))
                    {
                        TeleportSpot script = _teleportSpots[i].GetComponent<TeleportSpot>();
                        _player.enabled = false;
                        _player.transform.position = new Vector3(script.GetSpot.position.x,_player.transform.position.y,script.GetSpot.position.z);
                        _player.transform.rotation = script.GetSpot.rotation;
                        _player.enabled = true;
                    }
                }
            }
        }
        #endif
    }
}
