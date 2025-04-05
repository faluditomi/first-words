using System;
using System.IO;
using TensorFlowLite;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SpellRecognition : MonoBehaviour
{
    [SerializeField] private string modelFile = "fus_ro_dah.tflite";
    private Interpreter interpreter;
    private string[] labels;

    private void Start()
    {
        string modelPath = Path.Combine(Application.streamingAssetsPath, modelFile);

        // Create interpreter options
        var options = new InterpreterOptions()
        {
            threads = 2  // Adjust the number of threads to optimize performance
        };

        // Enable GPU delegate (if supported)
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            options.AddGpuDelegate();
        }

        // Instantiate the interpreter with options
        interpreter = new Interpreter(FileUtil.LoadFile(modelPath), options);
        
        Debug.Log("TensorFlow Lite model loaded with custom options!");

        string labelsPath = Path.Combine(Application.streamingAssetsPath, "labels.txt");
        labels = File.ReadAllLines(labelsPath);
    }

    public string RecogniseSpell(float[] audioData)
    {
        interpreter.SetInputTensorData(0, audioData);
        interpreter.Invoke();

        float[] output = new float[interpreter.GetOutputTensorCount()];
        interpreter.GetOutputTensorData(0, output);

        int maxIndex = 0;
        for (int i = 1; i < output.Length; i++)
        {
            if (output[i] > output[maxIndex])
                maxIndex = i;
        }

        return labels[maxIndex];
    }

    private void OnDestroy()
    {
        interpreter?.Dispose();
    }
}
