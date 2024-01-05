using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioSyncController : MonoBehaviour
{
    public AudioSpectrum valleySpectrum;
    public ValleyAudioSyncer valleySync;
    public Color stage1ValleyRestColor;
    public Color stage1ValleyBeatColor;
    public Color stage2ValleyRestColor;
    public Color stage2ValleyBeatColor;
    public float stage1ValleyBias;
    public float stage2ValleyBias;
    public AudioSpectrum bloomSpectrum;
    public AudioSyncBloom bloomSync;
    public AudioSyncSlider stage1SliderSync;
    public AudioSyncSlider stage2SliderSync;
    public float stage1BloomRestIntensity;
    public float stage1BloomBeatIntensity;
    public float stage1BloomRestDiffusion;
    public float stage1BloomBeatDiffusion;
    public float stage2BloomRestIntensity;
    public float stage2BloomBeatIntensity;
    public float stage2BloomRestDiffusion;
    public float stage2BloomBeatDiffusion;
    public float stage1BloomBias;
    public float stage2BloomBias;
    public float stage3BloomBias;


    public AudioSource stage1Drums;
    public AudioSource stage1Others;
    public AudioSource stage2Music;
    public void Stage1Sync()
    {
        valleySpectrum.speaker = stage1Drums;
        bloomSpectrum.speaker = stage1Others;
        valleySync.restColor = stage1ValleyRestColor;
        valleySync.beatColor = stage1ValleyBeatColor;
        valleySync.bias = stage1ValleyBias;

        bloomSync.restIntensity = stage1BloomRestIntensity;
        bloomSync.beatIntensity = stage1BloomBeatIntensity;

        bloomSync.restDiffusion = stage1BloomRestDiffusion;
        bloomSync.beatDiffusion = stage1BloomBeatDiffusion;
        bloomSync.bias = stage1BloomBias;

        stage1SliderSync.speaker = stage1Others;
        stage2SliderSync.speaker = stage1Others;
    }
    public void Stage2Sync()
    {
        valleySpectrum.speaker = stage2Music;
        bloomSpectrum.speaker = stage2Music;
        valleySync.restColor = stage2ValleyRestColor;
        valleySync.beatColor = stage2ValleyBeatColor;
        valleySync.bias = stage2ValleyBias;
        bloomSync.restIntensity = stage2BloomRestIntensity;
        bloomSync.beatIntensity = stage2BloomBeatIntensity;

        bloomSync.restDiffusion = stage2BloomRestDiffusion;
        bloomSync.beatDiffusion = stage2BloomBeatDiffusion;

        bloomSync.bias = stage2BloomBias;

        stage1SliderSync.speaker = stage2Music;
        stage2SliderSync.speaker = stage2Music;
    }

    public void Stage3Sync()
    {
        bloomSync.bias = stage3BloomBias;
    }
}
