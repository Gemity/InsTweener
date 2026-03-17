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
    public class TransformDoMoveTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.position;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOMove(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.position = value;
        }
    }

    [TweenPath("Transform/Global/DoMoveX")]
    public class TransformDoMoveXTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.x;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOMoveX(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.position = new Vector3(value, _component.position.y, _component.position.z);
        }
    }

    [TweenPath("Transform/Global/DoMoveY")]
    public class TransformDoMoveYTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.y;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOMoveY(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.position = new Vector3(_component.position.x, value, _component.position.z);
        }
    }

    [TweenPath("Transform/Global/DoMoveZ")]
    public class TransformDoMoveZTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.position.z;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOMoveZ(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.position = new Vector3(_component.position.x, _component.position.y, value);
        }
    }

    [TweenPath("Transform/Global/DoMoveToTarget")]
    public class TransformDoMoveToTargetTween : iTween<Transform, Transform>
    {
        public override Tween Play()
        {
            if (_endValue == null) return null;
            return ApplyCommon(_component.DOMove(_endValue.transform.position, _duration).SetEase(_ease).SetDelay(_delay));
        }
    }

    [TweenPath("Transform/Local/DoLocalMove")]
    public class TransformDoLocalMoveTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localPosition;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOLocalMove(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.localPosition = value;
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveX")]
    public class TransformDoLocalMoveXTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localPosition.x;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOLocalMoveX(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.localPosition = new Vector3(value, _component.localPosition.y, _component.localPosition.z);
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveY")]
    public class TransformLocalDoMoveYTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localPosition.y;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOLocalMoveY(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.localPosition = new Vector3(_component.localPosition.x, value, _component.localPosition.z);
        }
    }

    [TweenPath("Transform/Local/DoLocalMoveZ")]
    public class TransformDoLocalMoveZTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localPosition.z;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOLocalMoveZ(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.localPosition = new Vector3(_component.localPosition.x, _component.localPosition.y, value);
        }
    }
    #endregion

    #region Transform/Rotate

    [TweenPath("Transform/Global/DORotate")]
    public class TransformDORotateTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.eulerAngles;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DORotate(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.eulerAngles = value;
        }
    }

    [TweenPath("Transform/Global/DORotateQuaternion")]
    public class TransformDORotateQuaternionTween : iTween<Transform, Quaternion>, IModifyValue<Quaternion>
    {
        public Quaternion GetCurrentValue()
        {
            return _component.rotation;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DORotateQuaternion(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Quaternion value)
        {
            _component.rotation = value;
        }
    }

    [TweenPath("Transform/Local/DOLocalRotate")]
    public class TransformDOLocalRotateTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localEulerAngles;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOLocalRotate(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.localEulerAngles = value;
        }
    }

    [TweenPath("Transform/Local/DOLocalRotateQuaternion")]
    public class TransformDOLocalRotateQuaternionTween : iTween<Transform, Quaternion>, IModifyValue<Quaternion>
    {
        public Quaternion GetCurrentValue()
        {
            return _component.localRotation;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOLocalRotateQuaternion(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Quaternion value)
        {
            _component.localRotation = value;
        }
    }
    #endregion

    #region Transform/Scale
    [TweenPath("Transform/Local/DoScale")]
    public class TransformDoScaleTween : iTween<Transform, Vector3>, IModifyValue<Vector3>
    {
        public Vector3 GetCurrentValue()
        {
            return _component.localScale;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOScale(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Vector3 value)
        {
            _component.localScale = value;
        }
    }

    [TweenPath("Transform/Local/DoScaleX")]
    public class TransformDoScaleXTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.x;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOScaleX(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.localScale = new Vector3(value, _component.localScale.y, _component.localScale.z);
        }
    }

    [TweenPath("Transform/Local/DoScaleY")]
    public class TransformDoScaleYTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.y;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOScaleY(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.localScale = new Vector3(_component.localScale.x, value, _component.localScale.z);
        }
    }

    [TweenPath("Transform/Local/DoScaleZ")]
    public class TransformDoScaleZTween : iTween<Transform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.localScale.z;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOScaleZ(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.localScale = new Vector3(_component.localScale.x, _component.localScale.y, value);
        }
    }
    #endregion

    #region SpriteRenderer
    [TweenPath("SpriteRenderer/DoColor")]
    public class SpriteRendererColorTween : iTween<SpriteRenderer, Color>, IModifyValue<Color>
    {
        public Color GetCurrentValue()
        {
            return _component.color;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Color value)
        {
            _component.color = value;
        }
    }

    [TweenPath("SpriteRenderer/DOFade")]
    public class SpriteRendererFadeTween : iTween<SpriteRenderer, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.color.a;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            Color c = _component.color;
            c.a = value;
            _component.color = c;
        }
    }
    #endregion

    #region RectTransform
    [TweenPath("RectTransform/DOAnchorPos")]
    public class RectTransformAnchorPos : iTween<RectTransform, Vector2>, IModifyValue<Vector2>
    {
        public Vector2 GetCurrentValue()
        {
            return _component.anchoredPosition;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOAnchorPos(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Vector2 value)
        {
            _component.anchoredPosition = value;
        }
    }

    [TweenPath("RectTransform/DOAnchorPosX")]
    public class RectTransformAnchorPosX : iTween<RectTransform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.anchoredPosition.x;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOAnchorPosX(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.anchoredPosition = new Vector2(value, _component.anchoredPosition.y);
        }
    }

    [TweenPath("RectTransform/DOAnchorPosY")]
    public class RectTransformAnchorPosY : iTween<RectTransform, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.anchoredPosition.y;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOAnchorPosY(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.anchoredPosition = new Vector2(_component.anchoredPosition.x, value);
        }
    }
    #endregion

    #region Image
    [TweenPath("Image/DoColor")]
    public class ImageColorTween : iTween<Image, Color>, IModifyValue<Color>
    {
        public Color GetCurrentValue()
        {
            return _component.color;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Color value)
        {
            _component.color = value;
        }
    }

    [TweenPath("Image/DOFade")]
    public class ImageFadeTween : iTween<Image, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.color.a;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            Color c = _component.color;
            c.a = value;
            _component.color = c;
        }
    }
    #endregion

    #region Other
    [TweenPath("CanvasGroup/DoFade")]
    public class CanvasGroupFadeTween : iTween<CanvasGroup, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.alpha;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.alpha = value;
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

    #region AudioSource
    [TweenPath("AudioSource/DOFade")]
    public class AudioSourceFadeTween : iTween<AudioSource, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.volume;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.volume = value;
        }
    }

    [TweenPath("AudioSource/DOPitch")]
    public class AudioSourcePitchTween : iTween<AudioSource, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.pitch;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOPitch(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.pitch = value;
        }
    }
    #endregion

    #region Camera
    [TweenPath("Camera/DOFieldOfView")]
    public class CameraFieldOfViewTween : iTween<Camera, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.fieldOfView;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOFieldOfView(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            _component.fieldOfView = value;
        }
    }

    [TweenPath("Camera/DOColor")]
    public class CameraColorTween : iTween<Camera, Color>, IModifyValue<Color>
    {
        public Color GetCurrentValue()
        {
            return _component.backgroundColor;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Color value)
        {
            _component.backgroundColor = value;
        }
    }
    #endregion

    #region Renderer/Material
    [TweenPath("Renderer/DOColor")]
    public class RendererColorTween : iTween<Renderer, Color>, IModifyValue<Color>
    {
        public Color GetCurrentValue()
        {
            return _component.sharedMaterial != null ? _component.sharedMaterial.color : Color.white;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.material.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Color value)
        {
            _component.sharedMaterial.color = value;
        }
    }

    [TweenPath("Renderer/DOFade")]
    public class RendererFadeTween : iTween<Renderer, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.sharedMaterial != null ? _component.sharedMaterial.color.a : 1f;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.material.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            if (_component.sharedMaterial == null) return;
            Color c = _component.sharedMaterial.color;
            c.a = value;
            _component.sharedMaterial.color = c;
        }
    }
    #endregion

    #region TextMeshPro
#if TMP_PRESENT
    [TweenPath("TextMeshPro/DOColor")]
    public class TMPTextColorTween : iTween<TMPro.TMP_Text, Color>, IModifyValue<Color>
    {
        public Color GetCurrentValue()
        {
            return _component.color;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOColor(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(Color value)
        {
            _component.color = value;
        }
    }

    [TweenPath("TextMeshPro/DOFade")]
    public class TMPTextFadeTween : iTween<TMPro.TMP_Text, float>, IModifyValue<float>
    {
        public float GetCurrentValue()
        {
            return _component.color.a;
        }

        public override Tween Play()
        {
            return ApplyCommon(_component.DOFade(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }

        public void SetCurrentValue(float value)
        {
            Color c = _component.color;
            c.a = value;
            _component.color = c;
        }
    }

    [TweenPath("TextMeshPro/DOText")]
    public class TMPTextDoTextTween : iTween<TMPro.TMP_Text, string>
    {
        public override Tween Play()
        {
            return ApplyCommon(_component.DOText(_endValue, _duration).From(_startValue).SetEase(_ease).SetDelay(_delay));
        }
    }
#endif
    #endregion
}
