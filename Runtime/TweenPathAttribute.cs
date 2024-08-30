using DG.Tweening;
using System;
using UnityEngine;

namespace Gemity.InsTweener
{
    [Flags]
    public enum DrawInspector
    {
        GetterValue = 1, PlayEditMode = 2
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TweenPathAttribute : Attribute
    {
        public string Path { get; }

        public TweenPathAttribute(string path)
        {
            Path = path;
        }
    }

    public sealed class DrawExtenInspector : Attribute
    {
        public DrawInspector Draw { get; }
        public DrawExtenInspector(DrawInspector type)
        {
            Draw = type;
        }
    }
}
