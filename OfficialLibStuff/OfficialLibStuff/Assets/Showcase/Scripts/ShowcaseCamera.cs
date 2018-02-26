//
//  Yet Another Naive Unity Library
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  YANUL is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !YANUL_DEBUG
#define YANUL_DEBUG
#endif //UNITY_EDITOR && !YANUL_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

#endregion Namespaces

//-----------------------------------------------------------------------------
namespace Showcase
{
    //-------------------------------------------------------------------------
    // HOW-TO SETUP
    //-------------------------------------------------------------------------
    public partial class ShowcaseCamera : MonoBehaviour
    {
        //---------------------------------------------------------------------
        #region Declarations
        public const string activateRotation = "ActivateRotation";
        public const string rotationAxisX = "rotationAxisX";
        public const string rotationAxisY = "rotationAxisY";
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        public Transform zAxisRotatingObject = null;
        [SerializeField]
        public Transform zMovingObject = null;
        [Space]
        [SerializeField]
        public Transform cameraPositionningObject = null;
        [SerializeField]
        public Transform cameraLookAtObject = null;
        [Space]
        public Vector2 moveSpeed = new Vector2(2, 0.5f);
        #endregion Settings

        //---------------------------------------------------------------------
        #region Unity Defaults
        private void Update()
        {
            if (Input.GetButton(activateRotation))
            {
                var moveX = Input.GetAxis(rotationAxisX);
                var moveY = Input.GetAxis(rotationAxisY);

                if (zAxisRotatingObject != null)
                {
                    var fw = zAxisRotatingObject.transform.rotation * Vector3.forward;
                    fw.y = 0;
                    fw = fw.normalized;
                    zAxisRotatingObject.transform.rotation *= Quaternion.Euler(0, moveX * moveSpeed.x, 0);
                }

                if (zMovingObject != null)
                {
                    var pos = zMovingObject.transform.position;
                    pos.y += moveY * moveSpeed.y;
                    zMovingObject.transform.position = pos;
                }
            }

            var mainCamera = Camera.main;
            if (mainCamera != null)
            {
                if (cameraPositionningObject != null)
                {
                    mainCamera.transform.position = cameraPositionningObject.transform.position;
                }

                if (cameraLookAtObject != null)
                {
                    mainCamera.transform.rotation = Quaternion.LookRotation((cameraLookAtObject.transform.position - mainCamera.transform.position).normalized, Vector3.up);
                }
            }
        }
        #endregion Unity Defaults
    }
}