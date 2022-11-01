/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */
using UnityEngine;

namespace Oculus.Voice.Toolkit.Samples
{
    public class PlayerKeyboardController : MonoBehaviour
    {
        [SerializeField] private KeyCode _rotationKey = KeyCode.Mouse0;
        private float _velocityY = 0;
        private CharacterController _controller;

        public float speed = 5;
        public float gravity = -5;
        public float rotationSensitity = 10;

        void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        void Update()
        {

            //Walking
            _velocityY += gravity * Time.deltaTime;

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            input = input.normalized;

            Vector3 temp = Vector3.zero;
            if (input.z == 1)
            {
                temp += transform.forward;
            }
            else if (input.z == -1)
            {
                temp += transform.forward * -1;
            }

            if (input.x == 1)
            {
                temp += transform.right;
            }
            else if (input.x == -1)
            {
                temp += transform.right * -1;
            }

            Vector3 velocity = temp * speed;
            velocity.y = _velocityY;

            _controller.Move(velocity * Time.deltaTime);

            if (_controller.isGrounded)
            {
                _velocityY = 0;
            }

            if (Input.GetKey(_rotationKey))
            {
                //Looking direction
                float inputX = Input.GetAxis("Mouse X");
                float perScreen = inputX * 180f / (float)Screen.width;
                transform.localEulerAngles += new Vector3(0f, perScreen * rotationSensitity, 0f);
            }

        }
    }

}
