using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateClipTest : MonoBehaviour
{
    public int samplerate = 44100;

    public int frequency = 440;

    bool paused = false;

    float time = 0f;

    WebAudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        clip = new WebAudioClip(samplerate);

        GenerateSound();

        clip.Play(0f);
    }

    void GenerateSound()
    {
        float[] samples = new float[samplerate * 2];
        float normalizedTime = 0f;

        for (int i = 0; i < samples.Length; ++i)
        {
            normalizedTime = (1f + (i / (float)samples.Length)) / 2f;
            samples[i] = Mathf.Sin(2 * Mathf.PI * (frequency * normalizedTime) * i / samplerate);
        }

        clip.SetData(samples);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (time > 2f)
            {
                paused = false;
                
                if (clip.IsPlaying)
                    clip.Stop();

                time = 0f;
            }
            else paused = !paused;

            if (paused) clip.Stop();
            else clip.Play(time);
        }

        if (!paused)
            time += Time.deltaTime;
    }
}
