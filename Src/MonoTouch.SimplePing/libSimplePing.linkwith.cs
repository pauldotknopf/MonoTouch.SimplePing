using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libSimplePing.a", LinkTarget.Simulator | LinkTarget.ArmV6 | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, ForceLoad = true)]
