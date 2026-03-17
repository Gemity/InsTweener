using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gemity.InsTweener
{
    /// <summary>
    /// Handles Edit Mode preview for InsTweener by manually interpolating values
    /// using EditorApplication.update, without requiring DOTween runtime.
    /// </summary>
    public static class EditModePreview
    {
        private struct SavedValue
        {
            public iTween Tween;
            public object OriginalValue;
        }

        private static InsTweener _currentTarget;
        private static List<SavedValue> _savedValues = new();
        private static float _previewTime;
        private static float _totalDuration;
        private static double _lastEditorTime;
        private static bool _isPreviewing;

        public static bool IsPreviewing => _isPreviewing;
        public static InsTweener CurrentTarget => _currentTarget;

        public static void StartPreview(InsTweener target)
        {
            if (_isPreviewing)
                StopPreview();

            _currentTarget = target;
            _savedValues.Clear();

            var iTweens = typeof(InsTweener)
                .GetField("_iTweens", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(target) as iTween[];

            if (iTweens == null || iTweens.Length == 0) return;

            // Save original values
            _totalDuration = 0f;
            foreach (var tween in iTweens)
            {
                if (tween == null) continue;

                var type = tween.GetType();
                var getCurrentMethod = type.GetMethod("GetCurrentValue", BindingFlags.Public | BindingFlags.Instance);
                if (getCurrentMethod != null)
                {
                    var saved = new SavedValue
                    {
                        Tween = tween,
                        OriginalValue = getCurrentMethod.Invoke(tween, null)
                    };
                    _savedValues.Add(saved);
                }

                var durationField = typeof(iTween).GetField("_duration", BindingFlags.NonPublic | BindingFlags.Instance);
                var delayField = type.GetField("_delay", BindingFlags.NonPublic | BindingFlags.Instance);
                float dur = durationField != null ? (float)durationField.GetValue(tween) : 0.4f;
                float delay = delayField != null ? (float)delayField.GetValue(tween) : 0f;

                var linkProp = typeof(iTween).GetProperty("Link", BindingFlags.NonPublic | BindingFlags.Instance);
                var link = linkProp != null ? (TweenLink)linkProp.GetValue(tween) : TweenLink.Appear;

                if (link == TweenLink.Join)
                    _totalDuration = Mathf.Max(_totalDuration, delay + dur);
                else
                    _totalDuration += delay + dur;
            }

            if (_totalDuration <= 0f) _totalDuration = 0.4f;

            _previewTime = 0f;
            _lastEditorTime = EditorApplication.timeSinceStartup;
            _isPreviewing = true;

            EditorApplication.update += UpdatePreview;
        }

        public static void StopPreview()
        {
            if (!_isPreviewing) return;

            EditorApplication.update -= UpdatePreview;

            // Restore original values
            foreach (var saved in _savedValues)
            {
                if (saved.Tween == null) continue;
                var type = saved.Tween.GetType();
                var setMethod = type.GetMethod("SetCurrentValue", BindingFlags.Public | BindingFlags.Instance);
                setMethod?.Invoke(saved.Tween, new[] { saved.OriginalValue });
            }

            _savedValues.Clear();
            _isPreviewing = false;
            _currentTarget = null;

            SceneView.RepaintAll();
        }

        private static void UpdatePreview()
        {
            if (!_isPreviewing || _currentTarget == null)
            {
                StopPreview();
                return;
            }

            double now = EditorApplication.timeSinceStartup;
            float delta = (float)(now - _lastEditorTime);
            _lastEditorTime = now;

            _previewTime += delta;
            float t = Mathf.Clamp01(_previewTime / _totalDuration);

            // Interpolate each tween
            foreach (var saved in _savedValues)
            {
                if (saved.Tween == null) continue;
                var type = saved.Tween.GetType();

                var startField = type.GetField("_startValue", BindingFlags.NonPublic | BindingFlags.Instance);
                var endField = type.GetField("_endValue", BindingFlags.NonPublic | BindingFlags.Instance);
                var setMethod = type.GetMethod("SetCurrentValue", BindingFlags.Public | BindingFlags.Instance);

                if (startField == null || endField == null || setMethod == null)
                    continue;

                object startVal = startField.GetValue(saved.Tween);
                object endVal = endField.GetValue(saved.Tween);

                object interpolated = Interpolate(startVal, endVal, t);
                if (interpolated != null)
                    setMethod.Invoke(saved.Tween, new[] { interpolated });
            }

            SceneView.RepaintAll();

            // Auto-stop when done
            if (_previewTime >= _totalDuration)
            {
                StopPreview();
            }
        }

        private static object Interpolate(object start, object end, float t)
        {
            if (start is float sf && end is float ef)
                return Mathf.Lerp(sf, ef, t);
            if (start is Vector2 sv2 && end is Vector2 ev2)
                return Vector2.Lerp(sv2, ev2, t);
            if (start is Vector3 sv3 && end is Vector3 ev3)
                return Vector3.Lerp(sv3, ev3, t);
            if (start is Color sc && end is Color ec)
                return Color.Lerp(sc, ec, t);
            if (start is Quaternion sq && end is Quaternion eq)
                return Quaternion.Lerp(sq, eq, t);

            return null;
        }
    }
}
