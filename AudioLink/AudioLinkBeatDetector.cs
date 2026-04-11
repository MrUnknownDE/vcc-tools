using UdonSharp;
using UnityEngine;
using AudioLink;

public class AudioLinkBeatDetector : UdonSharpBehaviour
{
    public AudioLink.AudioLink audioLinkInstance;
    
    [Header("Settings")]
    [Range(0, 1)] public float threshold = 0.5f;
    public float minBpm = 70f;
    public float maxBpm = 210f; // Alles darüber wird als Fehler ignoriert
    
    [Header("Output")]
    public float bpm = 128f;
    public float instantBpm = 128f;
    public bool isBeat;
    
    private float lastBeatTime;

    void Update()
    {
        if (audioLinkInstance == null || !audioLinkInstance.AudioDataIsAvailable()) return;

        Vector2 bassPos = AudioLink.AudioLink.GetALPassAudioBass();
        Vector4 data = audioLinkInstance.GetDataAtPixel((int)bassPos.x, (int)bassPos.y);
        float currentLevel = data.x;

        // Prüfen auf Threshold
        if (currentLevel > threshold)
        {
            float currentTime = Time.time;
            float timeDiff = currentTime - lastBeatTime;

            // Der "Debounce" Check: 
            // 0.27s entspricht ca. 222 BPM. Alles was schneller kommt, ist Rauschen.
            if (timeDiff > 0.27f) 
            {
                if (lastBeatTime > 0)
                {
                    float detected = 60f / timeDiff;
                    
                    // Nur plausible Werte übernehmen
                    if (detected >= minBpm && detected <= maxBpm)
                    {
                        instantBpm = detected;
                        // Glättung: 0.5 sorgt für schnelles Folgen, aber filtert Ausreißer
                        bpm = Mathf.Lerp(bpm, instantBpm, 0.5f);
                    }
                }
                lastBeatTime = currentTime;
                isBeat = true;
            }
            else
            {
                isBeat = false;
            }
        }
        else
        {
            isBeat = false;
        }
    }
}