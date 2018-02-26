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
using UnityEngine.SceneManagement;

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
    public partial class ShowcaseMapLoader : MonoBehaviour
    {
        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        public List<string> scenes = new List<string>();
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        private bool menuIsVisible = false;
        private string lastSceneLoaded = string.Empty;

        private GUIStyle stlDefault;
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        #endregion Properties

        //---------------------------------------------------------------------
        #region Unity Defaults
        private void OnGUI()
        {
            InitStyles();

            float maxWidth = 0;
            foreach(var scene in scenes)
            {
                maxWidth = Mathf.Max(maxWidth, 10 + stlDefault.CalcSize(new GUIContent(scene)).x);
            }
            maxWidth += stlDefault.CalcSize(new GUIContent("Load ")).x;

            var screenSize = new Vector2(Screen.width, Screen.height);
            var margin = new Vector2(6, 4);
            var buttonSize = menuIsVisible ? new Vector2(maxWidth, 30) : new Vector2(90, 30);
            var buttonCount = menuIsVisible ? scenes.Count + 1 : 1;
            var mousePos = Input.mousePosition;
            mousePos.y = screenSize.y - mousePos.y;

            GUI.Label(new Rect(mousePos, buttonSize), "<color=#ff00ffff>test</color>", stlDefault);
            var bgSize = new Vector2(margin.x * 2 + buttonSize.x, margin.y * (buttonCount + 1) + buttonSize.y * buttonCount);
            var rect = new Rect(margin, bgSize);
            GUI.Box(rect, GUIContent.none);
            rect.position += margin;
            for (int b = 0; b < buttonCount; b++)
            {
                var scene = string.Empty;
                rect.size = buttonSize;
                GUI.Box(rect, GUIContent.none);
                if (b == 0)
                {
                    GUI.Label(rect, string.Format("<color={0}</color>", menuIsVisible ? "#999999ff>Hide menu" : "#ddddddff>Scene menu"), stlDefault);
                }
                else
                {
                    scene = scenes[b - 1];
                    bool isLoaded = scene == lastSceneLoaded;
                    GUI.Label(rect, string.Format("<color={0}>{1}{2}</color>", isLoaded ? "#00ff00ff" : "#ffffffff", "Load ", scene), stlDefault);
                }

                if (Event.current.type == EventType.MouseUp)
                {
                    if (rect.Contains(mousePos))
                    {
                        if (b == 0)
                        {
                            menuIsVisible = !menuIsVisible;
                        }
                        else
                        {
                            if (lastSceneLoaded != scene)
                            {
                                if (lastSceneLoaded != string.Empty)
                                {
                                    SceneManager.UnloadSceneAsync(lastSceneLoaded);
                                    lastSceneLoaded = string.Empty;
                                }

                                lastSceneLoaded = scene;
                                SceneManager.LoadSceneAsync(lastSceneLoaded, LoadSceneMode.Additive);
                            }
                        }
                    }
                }
                rect.position += new Vector2(0, margin.y + buttonSize.y);
            }
        }
        #endregion Unity Defaults

        //---------------------------------------------------------------------
        private void InitStyles()
        {
            if (stlDefault == null)
            {
                stlDefault = new GUIStyle();
                stlDefault.alignment = TextAnchor.MiddleCenter;
                stlDefault.font = Font.CreateDynamicFontFromOSFont("Consolas", 15);
                stlDefault.active.textColor = Color.white;
                stlDefault.richText = true;
            }
        }
    }
}