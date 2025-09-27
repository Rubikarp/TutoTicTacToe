using NaughtyAttributes;
using UnityEngine;
using System;

public class ScreenCaptureTool : MonoBehaviour
{
    [SerializeField] string filename;

    [Button]
    public void MakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot($"{filename}_{DateTime.Now.Ticks}.png");
    }
}