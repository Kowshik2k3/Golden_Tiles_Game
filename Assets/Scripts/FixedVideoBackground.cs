/*
using UnityEngine;
using UnityEngine.Video;

public class FixedVideoBackground : MonoBehaviour
{
    public VideoClip videoClip;

    void Start()
    {
        // Add VideoPlayer to main camera
        VideoPlayer videoPlayer = gameObject.AddComponent<VideoPlayer>();

        // Video settings
        videoPlayer.clip = videoClip;
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCamera = GetComponent<Camera>();

        // Make video fit vertically
        videoPlayer.aspectRatio = VideoAspectRatio.FitVertically;

        // Ensure video renders behind everything
        videoPlayer.targetCameraAlpha = 1f;

        videoPlayer.Play();
        Debug.Log("Video background started - Fit Vertically");
    }
}
*/


using UnityEngine;
using UnityEngine.Video;

public class FixedVideoBackground : MonoBehaviour
{
    public VideoClip videoClip;

    void Start()
    {
        // Add VideoPlayer to main camera
        VideoPlayer videoPlayer = gameObject.AddComponent<VideoPlayer>();

        // Video settings
        videoPlayer.clip = videoClip;
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCamera = GetComponent<Camera>();

        // Make video fit vertically
        videoPlayer.aspectRatio = VideoAspectRatio.FitVertically;

        // Ensure video renders behind everything
        videoPlayer.targetCameraAlpha = 1f;

        videoPlayer.Play();
        Debug.Log("Video background started - Fit Vertically");
    }
}