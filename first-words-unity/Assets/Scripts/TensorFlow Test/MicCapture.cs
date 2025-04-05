using System.Linq;
using UnityEngine;

public class MicCapture : MonoBehaviour
{
    public int sampleRate = 16000; // Recommended sample rate for TFLite models
    public int recordingDuration = 1; // Length of each recording segment in seconds
    private AudioClip microphoneClip;
    private string micName;
    private bool isRecording = false;

    private SpellRecognition spellRecognition;

    void Awake()
    {
        spellRecognition = FindFirstObjectByType<SpellRecognition>();
    }

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];  // Use the first available microphone
            StartRecording();
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
    }

    void StartRecording()
    {
        if (!isRecording)
        {
            microphoneClip = Microphone.Start(micName, true, recordingDuration, sampleRate);
            isRecording = true;
            InvokeRepeating(nameof(ProcessAudio), recordingDuration, recordingDuration);
        }
    }

    void ProcessAudio()
    {
        if (!isRecording || microphoneClip == null) return;

        int micPosition = Microphone.GetPosition(micName);

        if (micPosition <= 0) return;

        float[] samples = new float[microphoneClip.samples];
        microphoneClip.GetData(samples, 0);

        // Process only the latest segment of audio
        int segmentLength = sampleRate * recordingDuration; // Number of samples in the last recorded second
        float[] latestSamples = samples.Skip(Mathf.Max(0, samples.Length - segmentLength)).ToArray();

        // Recognize the spell
        string detectedSpell = spellRecognition.RecogniseSpell(latestSamples);
        Debug.Log("Recognized Spell: " + detectedSpell);
    }

    private void OnDestroy()
    {
        if (isRecording)
        {
            Microphone.End(micName);
            isRecording = false;
        }
    }
}
