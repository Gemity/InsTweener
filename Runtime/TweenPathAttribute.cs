using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class TweenPathAttribute : Attribute
{
    public string Path { get; }

    public TweenPathAttribute(string path)
    {
        Path = path;
    }
}

