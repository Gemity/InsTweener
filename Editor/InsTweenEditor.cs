using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Reflection;

namespace Gemity.InsTweener
{
    [CustomEditor(typeof(InsTweener))]
    public class InsTweenerEditor : Editor
    {
        private InsTweener _insTweener;
        private iTween[] _iTweens;
        private ReorderableList _reorderableList;
        private SerializedProperty _iTweensProp;

        iTween[] ITweens
        {
            get
            {
                _iTweens = _insTweener.GetType().GetField("_iTweens", BindingFlags.NonPublic | BindingFlags.Instance)
                                 .GetValue(_insTweener) as iTween[];

                return _iTweens;
            }
        }

        private void Awake()
        {
            _insTweener = target as InsTweener;
        }

        private void OnEnable()
        {
            _insTweener = target as InsTweener;
            _iTweensProp = serializedObject.FindProperty("_iTweens");
            SetupReorderableList();
        }

        private void SetupReorderableList()
        {
            _reorderableList = new ReorderableList(serializedObject, _iTweensProp, true, true, true, true);

            _reorderableList.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, $"Tweens ({_iTweensProp.arraySize})");
            };

            _reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                if (index >= _iTweensProp.arraySize) return;
                var element = _iTweensProp.GetArrayElementAtIndex(index);
                rect.y += 2;
                rect.height = EditorGUI.GetPropertyHeight(element);
                EditorGUI.PropertyField(rect, element, new GUIContent($"Tween {index}"), true);
            };

            _reorderableList.elementHeightCallback = index =>
            {
                if (index >= _iTweensProp.arraySize) return EditorGUIUtility.singleLineHeight;
                var element = _iTweensProp.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(element) + 4;
            };

            // Add: always create null entry (no copy)
            _reorderableList.onAddCallback = list =>
            {
                int newIndex = _iTweensProp.arraySize;
                _iTweensProp.arraySize++;
                var newElement = _iTweensProp.GetArrayElementAtIndex(newIndex);
                newElement.managedReferenceValue = null;
                serializedObject.ApplyModifiedProperties();
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Draw all properties except _iTweens (we handle it with ReorderableList)
            DrawPropertiesExcluding(serializedObject, "_iTweens");

            EditorGUILayout.Space(5);
            _reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

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
                PreviewButton();
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

        private void PreviewButton()
        {
            EditorGUILayout.Space(5);
            bool isPreviewing = EditModePreview.IsPreviewing && EditModePreview.CurrentTarget == _insTweener;

            if (isPreviewing)
            {
                GUI.backgroundColor = new Color(1f, 0.5f, 0.5f);
                if (GUILayout.Button("Stop Preview"))
                    EditModePreview.StopPreview();
                GUI.backgroundColor = Color.white;
            }
            else
            {
                GUI.backgroundColor = new Color(0.5f, 1f, 0.5f);
                if (GUILayout.Button("Preview"))
                    EditModePreview.StartPreview(_insTweener);
                GUI.backgroundColor = Color.white;
            }
        }

        private void ModifyValue()
        {
            EditorGUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUIContent getStart = new GUIContent("Get start", "Get current value of component and set to start value");
            if(GUILayout.Button(getStart, GUILayout.ExpandWidth(true)))
            {
                Undo.RecordObject(_insTweener, "Capture Start Values");
                foreach (var tween in ITweens)
                {
                    if (tween == null)
                        continue;

                    Type t = tween.GetType();

                    var startField = t.GetField("_startValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (startField == null)
                        continue;

                    var current = t.GetMethod("GetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?.Invoke(tween, null);
                    startField.SetValue(tween, current);
                }

                EditorUtility.SetDirty(_insTweener);
                PrefabUtility.RecordPrefabInstancePropertyModifications(_insTweener);
            }

            GUIContent getEnd = new GUIContent("Get end", "Get current value of component and set to end value");
            if (GUILayout.Button(getEnd, GUILayout.ExpandWidth(true)))
            {
                Undo.RecordObject(_insTweener, "Capture End Values");
                foreach (var tween in ITweens)
                {
                    if (tween == null)
                        continue;

                    Type t = tween.GetType();

                    var endField = t.GetField("_endValue", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (endField == null)
                        continue;

                    var current = t.GetMethod("GetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?.Invoke(tween, null);
                    endField.SetValue(tween, current);
                }
                EditorUtility.SetDirty(_insTweener);
                PrefabUtility.RecordPrefabInstancePropertyModifications(_insTweener);
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
                        continue;

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
                        continue;

                    t.GetMethod("SetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?
                     .Invoke(tween, new object[] { endField.GetValue(tween) });
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
