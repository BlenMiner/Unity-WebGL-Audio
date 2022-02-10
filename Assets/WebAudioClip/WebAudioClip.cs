using System;
using UnityEngine;

public class WebAudioClip : IDisposable
{
    int m_frequency;

    int m_channelCount;

    int m_clipId;

    bool m_isPlaying = false;

    public bool IsPlaying => m_isPlaying;

    public float Length {get; private set;} = 0;

#if UNITY_EDITOR
    AudioSource m_unitySource;

    AudioClip m_unityClip;
#endif

    public WebAudioClip(int freq, int channels = 1)
    {
        // Make sure the audio controller was created
        WebAudioController.Init();

        m_frequency = freq;
        m_channelCount = channels;

#if UNITY_EDITOR
        m_unitySource = WebAudioController.AllocateAudioSource();
#else
        // Creates clip in the browser via JS
        m_clipId = WebAudioController.NewClip();
#endif
    }

    public void Dispose()
    {
#if UNITY_EDITOR
        WebAudioController.FreeAudioSource(m_unitySource);
#else
        // Clean up memory
        WebAudioController.DisposeClip(m_clipId);
#endif
    }

    public void SetData(float[] data, int offsetSamples = 0)
    {
        Length = ((float)data.Length / m_frequency) / m_channelCount;
#if UNITY_EDITOR
        m_unityClip = AudioClip.Create("TMP", data.Length, m_channelCount, m_frequency, false);
        m_unityClip.SetData(data, offsetSamples);
        m_unitySource.clip = m_unityClip;
#else
        WebAudioController.SetDataClip(m_clipId, data, offsetSamples, m_channelCount, m_frequency);
#endif
    }

    public void Play(float timeInSeconds = 0f)
    {
        if (m_isPlaying) return;
#if UNITY_EDITOR
        m_unitySource.Play();
        m_unitySource.time = timeInSeconds;
#else
        WebAudioController.StartClip(m_clipId, timeInSeconds);
#endif
        m_isPlaying = true;
    }

    public void Stop()
    {
        if (!m_isPlaying) return;
#if UNITY_EDITOR
        m_unitySource.Stop();
#else
        WebAudioController.StopClip(m_clipId);
#endif
        m_isPlaying = false;
    }
}
