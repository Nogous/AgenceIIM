using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoManager : MonoBehaviour
{
    public VideoClip video;
    private VideoPlayer videoPlayer;
    public GameObject obj;
    private Material mat;
    private RenderTexture texture;
    private Image image;

    private void Start()
    {
        obj.AddComponent<VideoPlayer>();
        videoPlayer = obj.GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.clip = video;
        mat = new Material(obj.GetComponent<Renderer>().material);
        texture = new RenderTexture((int)obj.transform.localScale.x * 640, (int)obj.transform.localScale.y*640, 640);
        videoPlayer.targetTexture = texture;
        mat.mainTexture = texture;
        obj.GetComponent<Renderer>().material = mat;

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

    public void Stop()
    {
        videoPlayer.Stop();
    }

    public void Restart()
    {
        Stop();
        Play();
    }
}
