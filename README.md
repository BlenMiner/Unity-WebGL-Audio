# Unity-WebGL-Audio

This works similar to AudioClip but using AudioContext of the browser instead.

Why?

To bypass this error:

"Data too long to fit the audioclip: CLIP x sample(s) discarded"

Usage:

```c#
WebAudioClip m_audioClip = new WebAudioClip(Frequency, ChannelCount);

m_audioClip.SetData(Samples, Offset);

m_audioClip.Length // Duration in seconds

m_audioClip.Play(Time); // Start playing at that time

m_audioClip.Stop();
```

Notice how it doesn't use an AudioSource, this is because we use the AudioContext of the browser to play it.
