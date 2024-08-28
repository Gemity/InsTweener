using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gemity.InsTweener
{
    internal enum TweenLink
    {
        Join, Appear
    }

    [Serializable]
    public abstract class iTween
    {
        [SerializeField] internal TweenLink _link;
        [SerializeField] protected float _duration = 0.4f;
        internal TweenLink Link => _link;

        public abstract Tween Play();
        public iTween() { }
    }

    public abstract class iTween<T, T1> : iTween where T : Component
    {
        [SerializeField] protected T _component;
        [SerializeField] protected T1 _startValue;
        [SerializeField] protected T1 _endValue;

        public virtual T1 GetCurrentValue() => default;
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
#endif
    public class InsTweener : MonoBehaviour
    {
        internal enum PlayAtTime
        {
            Manual, Awake, OnEnable, Start
        }

        #region Static field, method
        private static Dictionary<string, InsTweener> _allInsTweens = new();
        internal static void Add(InsTweener insTweener)
        {
            if (!_allInsTweens.ContainsKey(insTweener.Id))
                _allInsTweens.Add(insTweener.Id, insTweener);
            else
                _allInsTweens[insTweener.Id] = insTweener;
        }

        internal static void Remove(InsTweener insTweener)
        {
            _allInsTweens.Remove(insTweener.Id);
        }

        public static InsTweener FindTweenById(string id)
        {
            if (!_allInsTweens.ContainsKey(id))
            {
                Debug.Log($"Can't find tween id {id}");
                return null;
            }

            return _allInsTweens[id];
        }
        #endregion

        [SerializeField] private string _id;
        [SerializeField] private PlayAtTime _playAtTime;
        [SerializeField] private bool _autoKill;
        [SerializeField] private bool _hideOnAwake;
        [SerializeReference] private iTween[] _iTweens;

        public Action onComplete;
        public string Id => _id;

        private void Awake()
        {
            Add(this);

            if (_hideOnAwake)
                gameObject.SetActive(false);
            else if (_playAtTime == PlayAtTime.Awake)
                Play();
        }

        private void OnEnable()
        {
            if (_playAtTime == PlayAtTime.OnEnable)
                Play();
        }

        private void Start()
        {
            if (_playAtTime == PlayAtTime.Start)
                Play();
        }

        private void OnDestroy()
        {
            Remove(this);
        }

        public void Play()
        {
            if (gameObject.activeInHierarchy)
                return;

            if (_iTweens.Length == 1)
                _iTweens[0].Play().onComplete += () =>
                {
                    onComplete?.Invoke();
                    if (_autoKill)
                        Kill();
                };
            else
            {
                Sequence sq = DOTween.Sequence();
                foreach (var i in _iTweens)
                {
                    if (i.Link == TweenLink.Join)
                        sq.Join(i.Play());
                    else if (i.Link == TweenLink.Appear)
                        sq.Append(i.Play());
                }

                sq.onComplete += () =>
                {
                    onComplete?.Invoke();
                    if (_autoKill)
                        Kill();
                };
            }
        }

        private void Kill()
        {
            Remove(this);
            Destroy(this);
        }
    }
}