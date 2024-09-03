using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace Gemity.InsTweener
{
    [CustomEditor(typeof(InsTweener))]
    public class InsTweenerEditor : Editor
    {
        private InsTweener _insTweener;
        private iTween[] _iTweens;

        iTween[] ITweens
        {
            get
            {
                _iTweens ??= _insTweener.GetType().GetField("_iTweens", BindingFlags.NonPublic | BindingFlags.Instance)
                                 .GetValue(_insTweener) as iTween[];

                return _iTweens;
            }
        }

        private void Awake()
        {
            _insTweener = target as InsTweener;

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (EditorApplication.isPlaying)
            {
                GUI.enabled = false;
                ModifyValue();
                GUI.enabled = true;
                PlayButton();
            }
            else
            {
                ModifyValue();
                GUI.enabled = false;
                PlayButton();
                GUI.enabled = true;
            }
        }

        private void PlayButton()
        {
            if (GUILayout.Button("Play"))
            {
                _insTweener.GetType().GetMethod("CreateTween", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(_insTweener, null);
                _insTweener.Play();
            }
        }

        private void ModifyValue()
        {
            EditorGUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUIContent getStart = new GUIContent("Get start", "Get current value of component and set to start value");
            if(GUILayout.Button(getStart, GUILayout.ExpandWidth(true)))
            {
                foreach (var tween in ITweens)
                {
                    if (tween == null)
                        continue;

                    Type t = tween.GetType();

                    var startField = t.GetField("_startValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (startField == null)
                        return;

                    var current = t.GetMethod("GetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?.Invoke(tween, null);
                    startField.SetValue(tween, current);
                }
            }

            GUIContent getEnd = new GUIContent("Get end", "Get current value of component and set to end value");
            if (GUILayout.Button(getEnd, GUILayout.ExpandWidth(true)))
            {
                foreach (var tween in ITweens)
                {
                    if (tween == null)
                        continue;

                    Type t = tween.GetType();

                    var endField = t.GetField("_endValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (endField == null)
                        return;

                    var current = t.GetMethod("GetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?.Invoke(tween, null);
                    endField.SetValue(tween, current);
                }
            }

            GUIContent setStart = new GUIContent("Set start", "Set current value of component to start value");
            if (GUILayout.Button(setStart, GUILayout.ExpandWidth(true)))
            {
                foreach (var tween in ITweens)
                {
                    if (tween == null)
                        continue;

                    Type t = tween.GetType();

                    var startField = t.GetField("_startValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (startField == null)
                        return;

                    t.GetMethod("SetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?
                     .Invoke(tween, new object[] { startField.GetValue(tween) });
                }
            }


            GUIContent setEnd = new GUIContent("Set end", "Set current value of component to end value");
            if (GUILayout.Button(setEnd, GUILayout.ExpandWidth(true)))
            {
                foreach (var tween in ITweens)
                {
                    if (tween == null)
                        continue;

                    Type t = tween.GetType();

                    var endField = t.GetField("_endValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (endField == null)
                        return;

                    t.GetMethod("SetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?
                     .Invoke(tween, new object[] { endField.GetValue(tween) });
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
