using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;

public class WhisperVoiceRecognition : MonoBehaviour
{
    private WhisperManager whisperManager;
    private MicrophoneRecord microphoneRecord;
    private WhisperStream whisperStream;
    
    [SerializeField] private Text transcriptionTextUI;
    [SerializeField] private ScrollRect TranscriptionWindowScrollUI;

    private void Awake()
    {
        whisperManager = FindFirstObjectByType<WhisperManager>();
        microphoneRecord = FindFirstObjectByType<MicrophoneRecord>();
    }

    private async void Start()
    {
        whisperStream = await whisperManager.CreateStream(microphoneRecord);
        
        whisperStream.OnResultUpdated += OnResult;
        whisperStream.OnSegmentUpdated += OnSegmentUpdated;
        whisperStream.OnSegmentFinished += OnSegmentFinished;
        whisperStream.OnStreamFinished += OnFinished;
        
        microphoneRecord.StartRecord();
        whisperStream.StartStream();
    }
    
    private void OnResult(string result)
    {
        transcriptionTextUI.text = result;
        UiUtils.ScrollDown(TranscriptionWindowScrollUI);
    }

    private void OnSegmentUpdated(WhisperResult segment)
    {
        // print($"Segment updated: {segment.Result}");
    }

    private void OnSegmentFinished(WhisperResult segment)
    {
        // print($"Segment finished: {segment.Result}");
    }
        
    private void OnFinished(string finalResult)
    {
        print("Stream finished!");
    }
}
