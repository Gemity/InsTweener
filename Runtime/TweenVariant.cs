using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gemity.InsTweener
{
    [TweenPath("RectTransform/AnchorPosition")]
    public class AnchorPosTween : iTween<RectTransform, Vector2>
    {
        public override Vector2 GetCurrentValue()
        {
            return _component == null ? Vector2.zero : _component.anchoredPosition;
        }

        public override Tween Play()
        {
            return _component.DOAnchorPos(_endValue, _duration).From(_startValue);
        }
    }

    [TweenPath("Transform/Scale")]
    public class LocalScaleTween : iTween<Transform, float>
    {
        public override float GetCurrentValue()
        {
            return _component == null ? 0 : _component.localScale.x;
        }

        public override Tween Play()
        {
            return _component.DOScale(_endValue, _duration).From(_startValue);
        }
    }

    [TweenPath("Transform/Position")]
    public class PositionTween : iTween<Transform, Vector3>
    {
        public Ease ease;
        public override Vector3 GetCurrentValue()
        {
            return _component == null ? Vector3.zero : _component.position;
        }

        public override Tween Play()
        {
            return _component.DOMove(_endValue, _duration).From(_startValue);
        }
    }

    [TweenPath("SpriteRendererTween")]
    public class SpriteRendererTween : iTween<SpriteRenderer, Color>
    {
        public UnityEngine.Events.UnityEvent oncomplete;
        public override Tween Play()
        {
            var t = _component.DOColor(_endValue, _duration).From(_startValue);
            t.onComplete += () => oncomplete?.Invoke();
            return t;
        }
    }

    [TweenPath("Image/Color")]
    public class ImageTween : iTween<Image, Color>
    {
        public UnityEngine.Events.UnityEvent oncomplete;
        public override Tween Play()
        {
            var t = _component.DOColor(_endValue, _duration).From(_startValue);
            t.onComplete += () => oncomplete?.Invoke();
            return t;
        }
    }
}