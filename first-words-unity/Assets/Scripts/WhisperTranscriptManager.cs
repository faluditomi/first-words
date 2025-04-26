using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;

//NOTE 
// for now, this is a singleton, but that might have to change once the game becomes multiplayer
public class WhisperTranscriptManager : MonoBehaviour
{

    public static WhisperTranscriptManager _instance { get; private set; }

    private WhisperManager whisperManager;
    private MicrophoneRecord microphoneRecord;
    private WhisperStream whisperStream;
    
    [SerializeField] private Text transcriptionTextUI;
    [SerializeField] private ScrollRect TranscriptionWindowScrollUI;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

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

    private void OnDestroy()
    {
        whisperStream.OnResultUpdated -= OnResult;
        whisperStream.OnSegmentUpdated -= OnSegmentUpdated;
        whisperStream.OnSegmentFinished -= OnSegmentFinished;
        // whisperStream.OnStreamFinished -= OnFinished;

        microphoneRecord.StopRecord();
        whisperStream.StopStream();
        whisperStream = null;
    }

    private void OnResult(string result)
    {
        if(transcriptionTextUI && TranscriptionWindowScrollUI)
        {
            transcriptionTextUI.text = result;
            UiUtils.ScrollDown(TranscriptionWindowScrollUI);
        }
    }
    
    //NOTE instead of relying only on the segment updates, we could prep the spell (visuals, particals, etc.) when
    //it is recognised in the update, and cast it for real when it appears in the finished segment
    private void OnSegmentUpdated(WhisperResult segment)
    {
        Debug.Log($"Segment updated: {segment.Result}");
        SpellRecognitionManager._instance.ScanSegment(segment.Result);
        
    }

    private void OnSegmentFinished(WhisperResult segment)
    {
        Debug.Log($"Segment finished: {segment.Result}");
        SpellRecognitionManager._instance.ResetSegmentation();
    }

}
