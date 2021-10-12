using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FixMath.NET;

[RequireComponent(typeof(RawImage))]
public class TestPerlinnoise : MonoBehaviour
{
    RawImage img;

    [Header("Perlin Noise Settings")]
    public int size = 100;
    [Tooltip("pixel count for 1 unit of noise")]
    public float scale = 10;
    [Header("Current Position")]
    public Vector3 position;

    public Vector3 velocity;
    [Header("Octave Perlin Settings")]
    public int octaves;
    public float persistence;


    public Text performanceText;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<RawImage>();
        if (performanceText == null)
            Debug.LogWarning("Maybe attach performance text");
    }

    // Update is called once per frame
    void Update()
    {
        img = GetComponent<RawImage>();

        Texture2D texture2D = new Texture2D(size, size);

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        stopwatch.Start();
        position += velocity * Time.deltaTime;

        for (int x = 0; x < texture2D.width; x++)
        {
            for (int y = 0; y < texture2D.height; y++)
            {
                float noiseLevel = (float)Fix64.OctavePerlin(
                                                (Fix64)(position.x + x / scale),
                                                (Fix64)(position.y + y / scale),
                                                (Fix64)(position.z), octaves, (Fix64)persistence);
                texture2D.SetPixel(x, y, new Color(noiseLevel, noiseLevel, noiseLevel));
            }
        }
        stopwatch.Stop();
        if (performanceText != null)
        {
            performanceText.text = $"{stopwatch.Elapsed.TotalMilliseconds:0.00}ms";
        }

        texture2D.Apply();
        img.texture = texture2D;
    }
}
