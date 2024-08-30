using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gemity.InsTweener
{
    interface IGetterCurrent<T>
    {
        public T GetCurrentValue();
    }

    #region Transform/Position
    [TweenPath("Transform/Global/DoMove")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveTween : iTween<Transform, Vector3>, IGetterCurrent<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.position;
        }

        public override Tween Play()
        {
            return _component.DOMove(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Global/DoMoveX")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveXTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.x;
        }

        public override Tween Play()
        {
            return _component.DOMoveX(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Global/DoMoveY")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveYTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.y;
        }

        public override Tween Play()
        {
            return _component.DOMoveY(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Global/DoMoveZ")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveZTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.z;
        }

        public override Tween Play()
        {
            return _component.DOMoveZ(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DoLocalMove")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoLocalMoveTween : iTween<Transform, Vector3>, IGetterCurrent<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localPosition;
        }

        public override Tween Play()
        {
            return _component.DOLocalMove(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveX")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoLocalMoveXTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.localPosition.x;
        }

        public override Tween Play()
        {
            return _component.DOLocalMoveX(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveY")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromLocalDoMoveYTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.y;
        }

        public override Tween Play()
        {
            return _component.DOMoveY(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveZ")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoLocalMoveZTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.z;
        }

        public override Tween Play()
        {
            return _component.DOMoveZ(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }
    #endregion

    #region Transform/Rotate

    [TweenPath("Transform/Global/DORotate")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransformDORotateTween : iTween<Transform, Vector3>, IGetterCurrent<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.eulerAngles;
        }

        public override Tween Play()
        {
            return _component.DORotate(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Global/DORotateQuaternion")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransformDORotateQuaternionTween : iTween<Transform, Quaternion>, IGetterCurrent<Quaternion>
    {
        public Quaternion GetCurrentValue()
        {
            return _component.rotation;
        }

        public override Tween Play()
        {
            return _component.DORotateQuaternion(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DOLocalRotate")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransformDOLocalRotateTween : iTween<Transform, Vector3>, IGetterCurrent<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.eulerAngles;
        }

        public override Tween Play()
        {
            return _component.DOLocalRotate(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DOLocalRotateQuaternion")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransformDOLocalRotateQuaternionTween : iTween<Transform, Quaternion>, IGetterCurrent<Quaternion>
    {
        public Quaternion GetCurrentValue()
        {
            return _component.rotation;
        }

        public override Tween Play()
        {
            return _component.DOLocalRotateQuaternion(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }
    #endregion

    #region Transform/Scale
    [TweenPath("Transform/Local/DoScale")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleTween : iTween<Transform, Vector3>, IGetterCurrent<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localScale;
        }

        public override Tween Play()
        {
            return _component.DOScale(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DoScaleX")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleXTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.x;
        }

        public override Tween Play()
        {
            return _component.DOScaleX(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DoScaleY")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleYTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.y;
        }

        public override Tween Play()
        {
            return _component.DOScaleY(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Transform/Local/DoScaleZ")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleZTween : iTween<Transform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.z;
        }

        public override Tween Play()
        {
            return _component.DOScaleZ(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }
    #endregion

    #region SpriteRenderer
    [TweenPath("SpriteRendere/DoColor")]
    public class SpriteRendererColorTween : iTween<SpriteRenderer, Color>
    {
        public override Tween Play()
        {
            return _component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("SpriteRendere/DOFade")]
    public class SpriteRendererFadeTween : iTween<SpriteRenderer, float>
    {
        public override Tween Play()
        {
            return _component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }
    #endregion

    #region RectTransform
    [TweenPath("RectTransform/DOAnchorPos")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class RectTransformAnchorPos : iTween<RectTransform, Vector2>, IGetterCurrent<Vector2>
    {
        public Vector2 GetCurrentValue()
        {
            return _component.anchoredPosition;
        }

        public override Tween Play()
        {
            return _component.DOAnchorPos(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("RectTransform/DOAnchorPosX")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class RectTransformAnchorPosX : iTween<RectTransform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.anchoredPosition.x;
        }

        public override Tween Play()
        {
            _component.anchoredPosition = new Vector2(_startValue, _component.anchoredPosition.y);
            return _component.DOAnchorPosX(_endValue, _duration).SetEase(_ease);
        }
    }

    [TweenPath("RectTransform/DOAnchorPosY")]
    [DrawExtenInspector(DrawInspector.GetterValue | DrawInspector.PlayEditMode)]
    public class RectTransformAnchorPosY : iTween<RectTransform, float>, IGetterCurrent<float>
    {
        public float GetCurrentValue()
        {
            return _component.anchoredPosition.y;
        }

        public override Tween Play()
        {
            _component.anchoredPosition = new Vector2(_component.anchoredPosition.x, _startValue);
            return _component.DOAnchorPosY(_endValue, _duration).SetEase(_ease);
        }
    }
    #endregion

    #region Image
    [TweenPath("Image/DoColor")]
    public class ImageColorTween : iTween<SpriteRenderer, Color>
    {
        public override Tween Play()
        {
            return _component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }

    [TweenPath("Image/DOFade")]
    public class ImageFadeTween : iTween<SpriteRenderer, float>
    {
        public override Tween Play()
        {
            return _component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }
    #endregion

    #region Other
    [TweenPath("CanvasGroup/DoColor")]
    public class CanvasGroupFadeTween : iTween<CanvasGroup, float>
    {
        public override Tween Play()
        {
            return _component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease);
        }
    }
    #endregion
}
