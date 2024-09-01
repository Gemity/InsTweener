using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gemity.InsTweener
{
    [CustomPropertyDrawer(typeof(iTween), true)]
    public class iTweenerDrawer : PropertyDrawer
    {
        private Dictionary<Type, string> _itweens = new();
        private Dictionary<Type, List<Attribute>> _itweensAttribute = new();
        private const string NotSet = "Not set";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_itweens.Count == 0)
            {
                _itweens = Assembly.GetAssembly(typeof(iTween))
                                   .GetTypes()
                                   .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(iTween)))
                                   .ToDictionary(x => x, x => x.GetCustomAttribute<TweenPathAttribute>().Path);

                _itweensAttribute = _itweens.Keys.ToDictionary(x => x, x => x.GetCustomAttributes().ToList());
            }

            if (_itweens.Count == 0)
                return;

            EditorGUI.BeginProperty(position, label, property);

            Type type = property.managedReferenceValue?.GetType();
            string typeName;
            if (type == null)
                typeName = NotSet;
            else
                typeName = _itweens.ContainsKey(type) ? _itweens[type] : NotSet;


            Rect dropdownRect = position;
            dropdownRect.x += EditorGUIUtility.labelWidth + 2;
            dropdownRect.width -= EditorGUIUtility.labelWidth + 2;
            dropdownRect.height = EditorGUIUtility.singleLineHeight;

            if (EditorGUI.DropdownButton(dropdownRect, new(typeName), FocusType.Keyboard))
            {
                GenericMenu _menu = new();

                _menu.AddItem(new GUIContent(NotSet), property.managedReferenceValue == null, () =>
                {
                    property.managedReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                });

                foreach (var tween in _itweens)
                {
                    _menu.AddItem(new GUIContent(tween.Value), typeName.Equals(tween.Value), () =>
                    {
                        object obj = tween.Key.GetConstructor(Type.EmptyTypes).Invoke(null);
                        MonoBehaviour mono = property.serializedObject.targetObject as MonoBehaviour;

                        FieldInfo compInfo = tween.Key.GetField("_component", BindingFlags.NonPublic | BindingFlags.Instance);
                        compInfo?.SetValue(obj, mono.GetComponent(compInfo.FieldType));
                        property.managedReferenceValue = obj;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                _menu.ShowAsContext();
            }

            if (!typeName.Equals(NotSet) && property.managedReferenceValue is iTween itween && _itweensAttribute.TryGetValue(type, out var listAttr))
            {
                if (listAttr.FirstOrDefault(x => x is DrawExtenInspector) is DrawExtenInspector drawExten)
                {
                    GUILayout.Space(20);
                    GUILayout.BeginHorizontal();
                    if (drawExten.Draw.HasFlag(DrawInspector.ModifyValue))
                    {
                        if (GUILayout.Button("Get start value"))
                        {
                            object value = type.GetMethod("GetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?.Invoke(itween, null);
                            type.GetField("_startValue", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(itween, value);
                        }

                        if (GUILayout.Button("Get end value"))
                        {
                            object value = type.GetMethod("GetCurrentValue", BindingFlags.Public | BindingFlags.Instance)?.Invoke(itween, null);
                            type.GetField("_endValue", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.SetValue(itween, value);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }

            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }
    }
}