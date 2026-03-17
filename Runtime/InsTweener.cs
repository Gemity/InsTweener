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
        Appear, Join, Await
    }

    [Serializable]
    public abstract class iTween
    {
        [SerializeField] internal TweenLink _link;
        [SerializeField] protected float _duration = 0.4f;
        [SerializeField] protected int _loopCount = 0;
        [SerializeField] protected LoopType _loopType = LoopType.Restart;
        [SerializeField] protected bool _isRelative = false;
        internal TweenLink Link => _link;
        internal float Duration => _duration;

        public abstract Tween Play();
        public iTween() {}

        protected Tween ApplyCommon(Tween tween)
        {
            if (tween == null) return null;
            if (_loopCount != 0)
                tween.SetLoops(_loopCount, _loopType);
            if (_isRelative)
                tween.SetRelative(true);
            return tween;
        }
    }

    public abstract class iTween<T, T1> : iTween where T : Component
    {
        [SerializeField] protected T _component;
        [SerializeField] protected T1 _startValue;
        [SerializeField] protected T1 _endValue;
        [SerializeField] protected Ease _ease;
        [SerializeField] protected float _delay;
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
#endif
    public class InsTweener : MonoBehaviour
    {
        internal enum PlayAtTime
        {
            Manual, OnEnable, Start
        }

        #region Static field, method
        private static Dictionary<string, InsTweener> _allInsTweens = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _allInsTweens = new Dictionary<string, InsTweener>();
        }
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
        [SerializeField] private int _sequenceLoopCount = 0;
        [SerializeField] private LoopType _sequenceLoopType = LoopType.Restart;
        [SerializeReference] private iTween[] _iTweens;

        private Tween _tween;
        public Tween Tween
        {
            get
            {
                if (_tween == null)
                    CreateTween();

                return _tween;
            }
        }

        public float Duration => Tween.Duration();
        public string Id => _id;


        [Space(20)]
        public UnityEngine.Events.UnityEvent onComplete;

        private void Awake()
        {
            Add(this);
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
            if(Tween.IsComplete())
                Tween.Rewind();

            Tween.Play();
        }

        private void CreateTween()
        {
            if (_iTweens.Length == 1)
            {
                var t = _iTweens[0];
                if (t == null) { _tween = DOTween.Sequence().Pause().SetAutoKill(false); return; }

                if (t.Link == TweenLink.Await)
                {
                    var sq = DOTween.Sequence().AppendInterval(t.Duration);
                    _tween = sq.OnComplete(() => onComplete?.Invoke()).SetAutoKill(false);
                }
                else
                {
                    var tw = t.Play();
                    if (tw == null) { _tween = DOTween.Sequence().Pause().SetAutoKill(false); return; }
                    _tween = tw.OnComplete(() => onComplete?.Invoke()).SetAutoKill(false);
                }
            }
            else
            {
                Sequence sq = DOTween.Sequence();
                bool anyStep = false;
                foreach (var i in _iTweens)
                {
                    if (i == null) continue;
                    anyStep = true;
                    if (i.Link == TweenLink.Join) sq.Join(i.Play());
                    else if (i.Link == TweenLink.Appear) sq.Append(i.Play());
                    else if (i.Link == TweenLink.Await) sq.AppendInterval(i.Duration);
                }
                if (!anyStep) { sq.AppendInterval(0f); }
                _tween = sq.OnComplete(() => onComplete?.Invoke()).SetAutoKill(false);
            }
            if (_sequenceLoopCount != 0)
                _tween.SetLoops(_sequenceLoopCount, _sequenceLoopType);
            _tween.Pause();
        }


        public void PlayBackwards()
        {
            if (!Tween.IsComplete())
                Tween.Complete();

            Tween.PlayBackwards();
        }

        private void Reset()
        {
           _id = Guid.NewGuid().ToString();
        }
    }
}