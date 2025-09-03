// File: Editor/InsTweenerWatcherWindow.cs
// Purpose: Monitor every InsTweener in the loaded scenes, preview runtime state,
//          and control Play/Pause/Stop/Restart/Rebuild per tweener or in batch.
//
// Style notes (as requested):
// - Comments in English
// - Keep fields [SerializeField] private when serialized
// - Private/protected fields use _camelCase
//
// Requirements:
// - Assembly references: UnityEditor, UnityEngine, DG.Tweening, (your) Instween.Runtime
// - Works even if InsTweener hides internal fields: uses safe reflection fallbacks
// - Does NOT modify your runtime instance unless you click control buttons

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// Namespace can match your editor assembly namespace
namespace Gemity.InsTweener
{
    public class InsTweenerWatcherWindow : EditorWindow
    {
        // -------- UI state --------
        [SerializeField] private bool _autoRefresh = true;
        [SerializeField] private float _refreshInterval = 0.5f;
        [SerializeField] private string _search = "";
        [SerializeField] private bool _onlyPlaying = false;
        [SerializeField] private bool _expandItems = false;

        private Vector2 _scroll;
        private double _lastRefreshTime;

        // Cached snapshot of found tweeners (updated on refresh)
        private readonly List<InsTweener> _items = new();

        // Reflection cache for InsTweener
        private static Type _insTweenerType;
        private static MethodInfo _miCreateTween, _miPlay, _miPause, _miStop, _miPlayBackwards;
        private static FieldInfo _fiITweens; // private iTween[] _iTweens
        private static PropertyInfo _piTween; // public Tween Tween {get;} OR fallback to field
        private static FieldInfo _fiTween;    // private Tween _tween

        // Reflection cache for iTween items
        private static PropertyInfo _piLink;            // TweenLink Link {get;}
        private static FieldInfo _fiDuration, _fiDelay, _fiEase, _fiComponent; // common private fields
        private static Type _tweenPathAttrType;
        private static PropertyInfo _piPathAttrPath;

        // Colors
        private static readonly Color _rowPlaying = new Color(0.20f, 0.35f, 0.20f, 0.25f);
        private static readonly Color _rowInactive = new Color(0.35f, 0.20f, 0.20f, 0.20f);

        [MenuItem("Window/InsTweener/Scene Watcher")]
        public static void Open()
        {
            var win = GetWindow<InsTweenerWatcherWindow>("InsTweener Watcher");
            win.minSize = new Vector2(640, 320);
            win.Focus();
        }

        private void OnEnable()
        {
            CacheReflection();

            // Auto refresh hooks
            EditorApplication.hierarchyChanged += RequestRefresh;
            EditorApplication.playModeStateChanged += _ => RequestRefresh();

            _lastRefreshTime = 0;
            RequestRefresh();
        }

        private void OnDisable()
        {
            EditorApplication.hierarchyChanged -= RequestRefresh;
        }

        private void CacheReflection()
        {
            if (_insTweenerType != null) return;

            // Runtime type
            _insTweenerType = typeof(InsTweener);

            // Public instance methods (if exposed)
            _miPlay = _insTweenerType.GetMethod("Play", BindingFlags.Instance | BindingFlags.Public);
            _miPause = _insTweenerType.GetMethod("Pause", BindingFlags.Instance | BindingFlags.Public);
            _miStop = _insTweenerType.GetMethod("Stop", BindingFlags.Instance | BindingFlags.Public);
            _miPlayBackwards = _insTweenerType.GetMethod("PlayBackwards", BindingFlags.Instance | BindingFlags.Public);

            // Non-public CreateTween (commonly needed for rebuild)
            _miCreateTween = _insTweenerType.GetMethod("CreateTween", BindingFlags.Instance | BindingFlags.NonPublic);

            // Fields/props
            _fiITweens = _insTweenerType.GetField("_iTweens", BindingFlags.Instance | BindingFlags.NonPublic);
            _piTween = _insTweenerType.GetProperty("Tween", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _fiTween = _insTweenerType.GetField("_tween", BindingFlags.Instance | BindingFlags.NonPublic);

            // iTween fields (by name convention)
            var baseITween = typeof(iTween);
            _piLink = baseITween.GetProperty("Link", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            _fiDuration = baseITween.GetField("_duration", BindingFlags.Instance | BindingFlags.NonPublic);
            _fiDelay = baseITween.GetField("_delay", BindingFlags.Instance | BindingFlags.NonPublic);
            _fiEase = baseITween.GetField("_ease", BindingFlags.Instance | BindingFlags.NonPublic);
            _fiComponent = baseITween.GetField("_component", BindingFlags.Instance | BindingFlags.NonPublic);

            // TweenPathAttribute
            _tweenPathAttrType = typeof(TweenPathAttribute);
            _piPathAttrPath = _tweenPathAttrType.GetProperty("Path", BindingFlags.Instance | BindingFlags.Public);
        }

        private void RequestRefresh()
        {
            _lastRefreshTime = 0; // force refresh in next OnGUI
            Repaint();
        }

        private void Update()
        {
            if (!_autoRefresh) return;
            var now = EditorApplication.timeSinceStartup;
            if (now - _lastRefreshTime >= _refreshInterval)
            {
                RefreshList();
                _lastRefreshTime = now;
            }
        }

        private void RefreshList()
        {
            _items.Clear();

            // Find all Instancers in scene(s)
            // Use Resources.FindObjectsOfTypeAll to also see inactive objects in the loaded scenes.
#if UNITY_2022_1_OR_NEWER
            var all = Resources.FindObjectsOfTypeAll<InsTweener>();
#else
            var all = Resources.FindObjectsOfTypeAll(typeof(InsTweener)).Cast<InsTweener>();
#endif
            foreach (var it in all)
            {
                if (it == null) continue;

                // Skip assets/prefabs in Project view
                if (EditorUtility.IsPersistent(it)) continue;

                // Only scene objects
                if (!it.gameObject.scene.IsValid()) continue;

                _items.Add(it);
            }

            // Sorting: playing first, then by hierarchy name
            _items.Sort((a, b) =>
            {
                int ap = IsPlaying(a) ? 0 : 1;
                int bp = IsPlaying(b) ? 0 : 1;
                if (ap != bp) return ap.CompareTo(bp);
                return string.Compare(a.name, b.name, StringComparison.Ordinal);
            });
        }

        private bool IsPlaying(InsTweener ins)
        {
            var t = GetTween(ins);
            return t != null && t.IsActive() && t.IsPlaying();
        }

        private Tween GetTween(InsTweener ins)
        {
            // Prefer property
            if (_piTween != null)
            {
                var tv = _piTween.GetValue(ins) as Tween;
                if (tv != null) return tv;
            }
            // Fallback field
            if (_fiTween != null)
                return _fiTween.GetValue(ins) as Tween;
            return null;
        }

        private void OnGUI()
        {
            DrawToolbar();

            // Force one refresh if needed (first time or manual)
            if (_lastRefreshTime <= 0)
            {
                RefreshList();
                _lastRefreshTime = EditorApplication.timeSinceStartup;
            }

            // Filters
            IEnumerable<InsTweener> filtered = _items;
            if (!string.IsNullOrEmpty(_search))
                filtered = filtered.Where(x => x.name.IndexOf(_search, StringComparison.OrdinalIgnoreCase) >= 0);
            if (_onlyPlaying)
                filtered = filtered.Where(IsPlaying);

            // Draw table header
            DrawHeader();

            // Rows
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            foreach (var ins in filtered)
                DrawRow(ins);
            EditorGUILayout.EndScrollView();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(70)))
                {
                    RefreshList();
                    _lastRefreshTime = EditorApplication.timeSinceStartup;
                }

                GUILayout.Space(8);

                _autoRefresh = GUILayout.Toggle(_autoRefresh, "Auto", EditorStyles.toolbarButton, GUILayout.Width(50));
                using (new EditorGUI.DisabledScope(!_autoRefresh))
                {
                    GUILayout.Label("every", GUILayout.Width(38));
                    _refreshInterval = EditorGUILayout.Slider(_refreshInterval, 0.1f, 2.0f, GUILayout.Width(200));
                    GUILayout.Label("s", GUILayout.Width(12));
                }

                GUILayout.FlexibleSpace();

                _expandItems = GUILayout.Toggle(_expandItems, "Details", EditorStyles.toolbarButton, GUILayout.Width(70));
                _onlyPlaying = GUILayout.Toggle(_onlyPlaying, "Only Playing", EditorStyles.toolbarButton, GUILayout.Width(100));

                GUILayout.Space(8);

                _search = GUILayout.TextField(_search, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.MinWidth(160));
                if (GUILayout.Button(GUIContent.none, GUI.skin.FindStyle("ToolbarSeachCancelButton")))
                    _search = string.Empty;
            }

            // Batch controls
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Rebuild All", GUILayout.Height(20)))
                {
                    foreach (var it in _items) SafeCreateTween(it);
                }
                if (GUILayout.Button("Play All", GUILayout.Height(20)))
                {
                    foreach (var it in _items) SafeCall(_miPlay, it);
                }
                if (GUILayout.Button("Pause All", GUILayout.Height(20)))
                {
                    foreach (var it in _items) SafeCall(_miPause, it);
                }
                if (GUILayout.Button("Stop All", GUILayout.Height(20)))
                {
                    foreach (var it in _items) SafeCall(_miStop, it);
                }
                EditorGUILayout.Space();
            }
        }

        private void DrawHeader()
        {
            var r = EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Object", EditorStyles.boldLabel, GUILayout.Width(220));
            GUILayout.Label("State", EditorStyles.boldLabel, GUILayout.Width(110));
            GUILayout.Label("Progress", EditorStyles.boldLabel, GUILayout.Width(180));
            GUILayout.FlexibleSpace();
            GUILayout.Label("Actions", EditorStyles.boldLabel, GUILayout.Width(330));
            EditorGUILayout.EndHorizontal();

            var line = GUILayoutUtility.GetRect(1, 1);
            EditorGUI.DrawRect(line, new Color(0, 0, 0, 0.2f));
        }

        private void DrawRow(InsTweener ins)
        {
            // Row background color by state
            var tween = GetTween(ins);
            bool active = tween != null && tween.IsActive();
            bool playing = active && tween.IsPlaying();

            var row = GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight * (_expandItems ? 3f : 1.4f));
            var bg = playing ? _rowPlaying : (!active ? _rowInactive : new Color(0, 0, 0, 0));
            if (bg.a > 0f) EditorGUI.DrawRect(row, bg);
            GUILayout.BeginArea(row);
            using (new EditorGUILayout.HorizontalScope())
            {
                // Object column
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(220)))
                {
                    EditorGUILayout.ObjectField(GUIContent.none, ins.gameObject, typeof(GameObject), true);
                    if (_expandItems)
                    {
                        EditorGUILayout.LabelField($"Path: {ins.gameObject.scene.path}", EditorStyles.miniLabel);
                    }
                }

                // State column
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(110)))
                {
                    string st = !active ? "Idle" : (playing ? "Playing" : "Paused");
                    EditorGUILayout.LabelField(st);

                    if (_expandItems)
                    {
                        var go = ins.gameObject;
                        EditorGUILayout.LabelField($"Active: {(go.activeInHierarchy ? "Yes" : "No")}", EditorStyles.miniLabel);
                    }
                }

                // Progress column
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(180)))
                {
                    float pct = 0f;
                    if (tween != null && tween.IsActive())
                    {
                        try { pct = tween.ElapsedPercentage(true); } catch { pct = 0f; }
                    }
                    var pr = GUILayoutUtility.GetRect(1, 18);
                    EditorGUI.ProgressBar(pr, Mathf.Clamp01(pct), $"{Mathf.RoundToInt(pct * 100f)}%");
                }

                GUILayout.FlexibleSpace();

                // Actions column
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(330)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("Select", GUILayout.Width(70)))
                        {
                            Selection.activeObject = ins.gameObject;
                            EditorGUIUtility.PingObject(ins.gameObject);
                        }
                        if (GUILayout.Button("Rebuild", GUILayout.Width(70))) SafeCreateTween(ins);
                        if (GUILayout.Button("Play", GUILayout.Width(60))) SafeCall(_miPlay, ins);
                        if (GUILayout.Button("Pause", GUILayout.Width(60))) SafeCall(_miPause, ins);
                        if (GUILayout.Button("Stop", GUILayout.Width(60))) SafeCall(_miStop, ins);
                    }

                    if (_expandItems)
                    {
                        DrawTweensDetail(ins);
                    }
                }
            }
            GUILayout.EndArea();

            GUILayout.Space(4);
        }

        private void DrawTweensDetail(InsTweener ins)
        {
            var arr = _fiITweens?.GetValue(ins) as Array;
            if (arr == null) return;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    var item = arr.GetValue(i);
                    if (item == null)
                    {
                        EditorGUILayout.LabelField($"[{i}] <null>");
                        continue;
                    }

                    var type = item.GetType();
                    string path = GetPathFromAttribute(type);
                    string link = _piLink != null ? Convert.ToString(_piLink.GetValue(item)) : "Appear";

                    float dur = _fiDuration != null ? Convert.ToSingle(_fiDuration.GetValue(item)) : 0f;
                    float delay = _fiDelay != null ? Convert.ToSingle(_fiDelay.GetValue(item)) : 0f;
                    string ease = _fiEase != null ? Convert.ToString(_fiEase.GetValue(item)) : "-";

                    var comp = _fiComponent?.GetValue(item) as Component;
                    string compInfo = comp != null ? $"{comp.GetType().Name} ({comp.gameObject.name})" : "<none>";

                    EditorGUILayout.LabelField(
                        $"[{i}] {path} | Link: {link} | Dur: {dur:0.###}s | Delay: {delay:0.###}s | Ease: {ease} | Target: {compInfo}",
                        EditorStyles.miniLabel
                    );
                }
            }
        }

        private string GetPathFromAttribute(Type t)
        {
            try
            {
                var attr = t.GetCustomAttributes(_tweenPathAttrType, false).FirstOrDefault();
                if (attr == null) return t.Name;
                var p = _piPathAttrPath?.GetValue(attr) as string;
                return string.IsNullOrEmpty(p) ? t.Name : p;
            }
            catch
            {
                return t.Name;
            }
        }

        private void SafeCreateTween(InsTweener ins)
        {
            // Prefer calling internal CreateTween if exposed; else fallback to: Stop + Play for rebuild in your implementation
            if (_miCreateTween != null)
            {
                try { _miCreateTween.Invoke(ins, null); }
                catch (Exception e) { Debug.LogException(e); }
            }
            else
            {
                // Fallback: Stop then Play (your InsTweener.Play should internally rebuild)
                SafeCall(_miStop, ins);
                SafeCall(_miPlay, ins);
            }
        }

        private void SafeCall(MethodInfo mi, InsTweener ins)
        {
            if (mi == null) return;
            try { mi.Invoke(ins, null); }
            catch (Exception e) { Debug.LogException(e); }
        }
    }
}
