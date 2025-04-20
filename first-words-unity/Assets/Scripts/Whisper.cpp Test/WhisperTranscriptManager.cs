using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;

public class WhisperTranscriptManager : MonoBehaviour
{
    
    private WhisperManager whisperManager;
    private MicrophoneRecord microphoneRecord;
    private WhisperStream whisperStream;
    
    [SerializeField] private Text transcriptionTextUI;
    [SerializeField] private ScrollRect TranscriptionWindowScrollUI;

    private string currentSegment;
    private ArrayList activeSpells = new ArrayList();

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
        // whisperStream.OnStreamFinished += OnFinished;
        
        microphoneRecord.StartRecord();
        whisperStream.StartStream();
    }
    
    private void OnResult(string result)
    {
        if(transcriptionTextUI && TranscriptionWindowScrollUI)
        {
            transcriptionTextUI.text = result;
            UiUtils.ScrollDown(TranscriptionWindowScrollUI);
        }
    }

    private void OnSegmentUpdated(WhisperResult segment)
    {
        print($"Segment updated: {segment.Result}");
        // currentSegment = segment.Result;

        // foreach(String spell in activeSpells)
        // {
        //     currentSegment.RemoveConsecutiveCharacters()
        // }

        // if(/* there is a spell in currentSegment */)
        // {
        //     // activate spell
        //     // add spell-word to activeSpells
        // }
        
    }

    private void OnSegmentFinished(WhisperResult segment)
    {
        print($"Segment finished: {segment.Result}");
        currentSegment = "";
    }

}
