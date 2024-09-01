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
        private void Awake()
        {
            _insTweener = target as InsTweener;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                PlayButton();
                GUI.enabled = false;
                ResetButton();
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
                PlayButton();
                GUI.enabled = true;
                ResetButton();
            }
            GUILayout.EndHorizontal();
        }

        private void PlayButton()
        {
            if (GUILayout.Button("Play"))
            {
                _insTweener.GetType().GetMethod("CreateTween", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(_insTweener, null);
                _insTweener.Play();
            }
        }

        private void ResetButton()
        {
            GUIContent content = new GUIContent("Reset", "Reset all component to start value");
            if (GUILayout.Button(content))
            {
                var itweens = _insTweener.GetType().GetField("_iTweens", BindingFlags.NonPublic | BindingFlags.Instance)
                                         .GetValue(_insTweener) as iTween[];
                if (itweens == null)
                    return;

                foreach (var tween in itweens)
                {
                    if (tween == null)
                        continue;

                    Type t = tween.GetType();

                    var startField = t.GetField("_startValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    if(startField == null)
                        return ;

                    t.GetMethod("SetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?
                     .Invoke(tween, new object[] { startField.GetValue(tween) });
                }
            }
        }
    }
}
