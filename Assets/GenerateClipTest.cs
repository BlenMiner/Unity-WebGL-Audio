using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateClipTest : MonoBehaviour
{
    public int clipGeneratedLengthInMin = 10;

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
        float[] samples = new float[samplerate * clipGeneratedLengthInMin * 60];

        for (int i = 0; i < samples.Length; ++i)
        {
            float time = i / (float)samplerate;

            float n0 = Mathf.Min(1f, Mathf.Abs(Mathf.PerlinNoise(time, 0f)));
            float n1 = Mathf.Min(1f, Mathf.Abs(Mathf.PerlinNoise(time * 2, time)));

            float f = (frequency * n0 * n1);

            samples[i] = Mathf.Sin(2 * Mathf.PI * f + i / samplerate);
        }

        clip.SetData(samples);
    }

    void Update()
    {
        if (clip.IsPlaying)
            time += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (time > clip.Length)
            {
                paused = false;
                clip.Stop();
                time = 0f;
            }
            else paused = !paused;

            if (paused) clip.Stop();
            else clip.Play(time);
        }
    }
}
