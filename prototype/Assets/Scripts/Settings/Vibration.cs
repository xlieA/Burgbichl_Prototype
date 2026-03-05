// https://gist.github.com/aVolpe/707c8cf46b1bb8dfb363
using UnityEngine;
using System.Runtime.InteropServices;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    private static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#elif UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void Vibrate(int ms);

    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject vibrator;
#else
    // No vibration support for other platforms
    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate");
        }
        else if (IsWebGL())
        {
            Vibrate();
        }
        
#if UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }

    public static void Vibrate(long milliseconds)
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", milliseconds);
        }
        else if (IsWebGL())
        {
            Vibrate(milliseconds);
        }

#if UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (IsAndroid())
        {
            vibrator.Call("vibrate", pattern, repeat);
        }
        else if (IsWebGL())
        {
            Vibrate();
        }

#if UNITY_IOS && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }

    public static bool HasVibrator()
    {
        return IsAndroid();
    }

    public static void Cancel()
    {
        if (IsAndroid())
        {
            vibrator.Call("cancel");
        }
    }

    private static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    private static bool IsIOS()
    {
#if UNITY_IOS && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

    private static bool IsWebGL()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
