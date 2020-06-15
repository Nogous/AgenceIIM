using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoManager : MonoBehaviour
{
    public VideoClip video;

    public VideoPlayer videoPlayer;
    private GameObject obj;

    private RenderTexture texture;
    private Image image;

    private void Start()
    {
        GameManager.instance.player.videoEnded = false;
        obj = GetComponent<Transform>().gameObject;
        obj.AddComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            videoPlayer = obj.GetComponent<VideoPlayer>();
            videoPlayer.playOnAwake = false;
        }
        videoPlayer.clip = video;
        Play();
    }

    public void Play()
    {
        videoPlayer.Play();
    }

    public void Pause()
    {
        videoPlayer.Pause();
    }

    public void Stop(bool closeVideo = false)
    {
        videoPlayer.Stop();
        if (closeVideo)
        {
            GameManager.instance.player.VideoTutoEnded();
        }
    }

    public void Restart()
    {
        Stop();
        Play();
    }
}
