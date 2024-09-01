using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gemity.InsTweener
{
    interface IModifyValue<T>
    {
        public T GetCurrentValue();
        public void SetCurrentValue(T value);
    }

    #region Transform/Position
    [TweenPath("Transform/Global/DoMove")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.position;
        }

        public override Tween Play()
        {
            return _component.DOMove(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.position = value;
        }
    }

    [TweenPath("Transform/Global/DoMoveX")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveXTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.x;
        }

        public override Tween Play()
        {
            return _component.DOMoveX(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.position = new Vector3(value, _component.position.y, _component.position.z);
        }
    }

    [TweenPath("Transform/Global/DoMoveY")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveYTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.y;
        }

        public override Tween Play()
        {
            return _component.DOMoveY(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.position = new Vector3(_component.position.x, value, _component.position.z);
        }
    }

    [TweenPath("Transform/Global/DoMoveZ")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoMoveZTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.z;
        }

        public override Tween Play()
        {
            return _component.DOMoveZ(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.position = new Vector3(_component.position.x, _component.position.y,value);
        }
    }

    [TweenPath("Transform/Local/DoLocalMove")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoLocalMoveTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localPosition;
        }

        public override Tween Play()
        {
            return _component.DOLocalMove(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.localPosition = value;
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveX")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoLocalMoveXTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localPosition.x;
        }

        public override Tween Play()
        {
            return _component.DOLocalMoveX(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.localPosition = new Vector3(value, _component.localPosition.y, _component.localPosition.z);
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveY")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromLocalDoMoveYTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.y;
        }

        public override Tween Play()
        {
            return _component.DOMoveY(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.localPosition = new Vector3(_component.localPosition.x, value, _component.localPosition.z);
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveZ")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoLocalMoveZTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.z;
        }

        public override Tween Play()
        {
            return _component.DOMoveZ(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.localPosition = new Vector3(_component.localPosition.x, _component.localPosition.y, value);
        }
    }
    #endregion

    #region Transform/Rotate

    [TweenPath("Transform/Global/DORotate")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransformDORotateTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.eulerAngles;
        }

        public override Tween Play()
        {
            return _component.DORotate(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.eulerAngles = value;
        }
    }

    [TweenPath("Transform/Global/DORotateQuaternion")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransformDORotateQuaternionTween : iTween<Transform, Quaternion>, IModifyValue<Quaternion>
    {
        public Quaternion GetCurrentValue()
        {
            return _component.rotation;
        }

        public override Tween Play()
        {
            return _component.DORotateQuaternion(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Quaternion value)
        {
            _component.rotation = value;
        }
    }

    [TweenPath("Transform/Local/DOLocalRotate")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransformDOLocalRotateTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localEulerAngles;
        }

        public override Tween Play()
        {
            return _component.DOLocalRotate(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.localEulerAngles = value;
        }
    }

    [TweenPath("Transform/Local/DOLocalRotateQuaternion")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransformDOLocalRotateQuaternionTween : iTween<Transform, Quaternion>, IModifyValue<Quaternion>
    {
        public Quaternion GetCurrentValue()
        {
            return _component.localRotation;
        }

        public override Tween Play()
        {
            return _component.DOLocalRotateQuaternion(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Quaternion value)
        {
            _component.localRotation = value;
        }
    }
    #endregion

    #region Transform/Scale
    [TweenPath("Transform/Local/DoScale")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localScale;
        }

        public override Tween Play()
        {
            return _component.DOScale(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.localScale = value;
        }
    }

    [TweenPath("Transform/Local/DoScaleX")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleXTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.x;
        }

        public override Tween Play()
        {
            return _component.DOScaleX(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.localScale = new Vector3(value, _component.localScale.y, _component.localScale.z);
        }
    }

    [TweenPath("Transform/Local/DoScaleY")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleYTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.y;
        }

        public override Tween Play()
        {
            return _component.DOScaleY(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.localScale = new Vector3(_component.localScale.x, value, _component.localScale.z);
        }
    }

    [TweenPath("Transform/Local/DoScaleZ")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class TransfromDoScaleZTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.z;
        }

        public override Tween Play()
        {
            return _component.DOScaleZ(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.localScale = new Vector3(_component.localScale.x, _component.localScale.y, value);
        }
    }
    #endregion

    #region SpriteRenderer
    [TweenPath("SpriteRendere/DoColor")]
    public class SpriteRendererColorTween : iTween<SpriteRenderer, Color>
    {
        public override Tween Play()
        {
            return _component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }
    }

    [TweenPath("SpriteRendere/DOFade")]
    public class SpriteRendererFadeTween : iTween<SpriteRenderer, float>
    {
        public override Tween Play()
        {
            return _component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }
    }
    #endregion

    #region RectTransform
    [TweenPath("RectTransform/DOAnchorPos")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class RectTransformAnchorPos : iTween<RectTransform, Vector2>, IModifyValue<Vector2>
    {
        public Vector2 GetCurrentValue()
        {
            return _component.anchoredPosition;
        }

        public override Tween Play()
        {
            return _component.DOAnchorPos(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(Vector2 value)
        {
            _component.anchoredPosition = value;
        }
    }

    [TweenPath("RectTransform/DOAnchorPosX")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class RectTransformAnchorPosX : iTween<RectTransform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.anchoredPosition.x;
        }

        public override Tween Play()
        {
            _component.anchoredPosition = new Vector2(_startValue, _component.anchoredPosition.y);
            return _component.DOAnchorPosX(_endValue, _duration).SetEase(_ease).SetDelay(_delay);
        }

        public void SetCurrentValue(float value)
        {
            _component.anchoredPosition = new Vector2(value, _component.anchoredPosition.y);
        }
    }

    [TweenPath("RectTransform/DOAnchorPosY")]
    [DrawExtenInspector(DrawInspector.ModifyValue | DrawInspector.PlayEditMode)]
    public class RectTransformAnchorPosY : iTween<RectTransform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.anchoredPosition.y;
        }

        public override Tween Play()
        {
            _component.anchoredPosition = new Vector2(_component.anchoredPosition.x, _startValue);
            return _component.DOAnchorPosY(_endValue, _duration).SetEase(_ease).SetDelay(_delay);
        }
        public void SetCurrentValue(float value)
        {
            _component.anchoredPosition = new Vector2(_component.anchoredPosition.x, value);
        }

    }
    #endregion

    #region Image
    [TweenPath("Image/DoColor")]
    public class ImageColorTween : iTween<SpriteRenderer, Color>
    {
        public override Tween Play()
        {
            return _component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }
    }

    [TweenPath("Image/DOFade")]
    public class ImageFadeTween : iTween<SpriteRenderer, float>
    {
        public override Tween Play()
        {
            return _component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }
    }
    #endregion

    #region Other
    [TweenPath("CanvasGroup/DoColor")]
    public class CanvasGroupFadeTween : iTween<CanvasGroup, float>
    {
        public override Tween Play()
        {
            return _component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay);
        }
    }

    [TweenPath("Await")]
    public class AwaitTween : iTween
    {
        public override Tween Play()
        {
            return null;
        }
    }
    #endregion
}
