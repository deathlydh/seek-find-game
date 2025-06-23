using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoloSimulator : MonoBehaviour
{
    // —писок животных, которых может распознавать наш YOLO
    private string[] wildlifeClasses = new string[]
    {
        "elk", "wolf", "bear", "fox", "boar", "moose", "deer", "rabbit", "lynx", "wildcat"
    };

    // »нтервал между симул€ци€ми детекции (в секундах)
    public float detectionInterval = 1.5f;

    private void Start()
    {
        Debug.Log("[INFO] YOLO Wildlife Detector initialized.");
        Debug.Log("[INFO] Loading weights from models/yolov8-wildlife.pt...");
        InvokeRepeating(nameof(SimulateDetection), 0.5f, detectionInterval);
    }

    private void SimulateDetection()
    {
        List<DetectedObject> detections = GenerateRandomDetections();

        if (detections.Count == 0)
        {
            Debug.Log("[INFO] No animals detected in this frame.");
            return;
        }

        foreach (var obj in detections)
        {
            Debug.Log($"[INFO] Detected '{obj.Class}' with confidence {obj.Confidence:F2} | BBox: [{obj.BoundingBox.xMin}, {obj.BoundingBox.yMin}, {obj.BoundingBox.xMax}, {obj.BoundingBox.yMax}]");
        }
    }

    private List<DetectedObject> GenerateRandomDetections()
    {
        int detectionCount = UnityEngine.Random.Range(1, 5);
        List<DetectedObject> detections = new List<DetectedObject>();

        for (int i = 0; i < detectionCount; i++)
        {
            string animal = wildlifeClasses[UnityEngine.Random.Range(0, wildlifeClasses.Length)];
            float confidence = UnityEngine.Random.Range(0.65f, 0.99f);

            // √енерируем случайные координаты bounding box
            int xMin = UnityEngine.Random.Range(0, 640);
            int yMin = UnityEngine.Random.Range(0, 480);
            int width = UnityEngine.Random.Range(80, 300);
            int height = UnityEngine.Random.Range(80, 300);
            int xMax = xMin + width;
            int yMax = yMin + height;

            detections.Add(new DetectedObject
            {
                Class = animal,
                Confidence = confidence,
                BoundingBox = new RectInt(xMin, yMin, width, height)
            });
        }

        return detections;
    }

    [Serializable]
    private class DetectedObject
    {
        public string Class;
        public float Confidence;
        public RectInt BoundingBox;
    }
}
