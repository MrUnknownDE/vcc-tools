using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeatDetectorDisplay : UdonSharpBehaviour
{
    public AudioLinkBeatDetector engine; 
    public TextMeshProUGUI bpmText;
    public Image beatIndicator;
    public Color activeColor = Color.cyan;

    private float flashTimer;

    void Update()
    {
        if (engine == null) return;

        // Wenn die Engine einen Beat erkennt...
        if (engine.isBeat)
        {
            // ...aktualisieren wir SOFORT den Text mit der instantBpm
            if (bpmText != null)
            {
                bpmText.text = engine.instantBpm.ToString("F0"); // "F0" für ganze Zahlen ohne Lag-Gefühl
            }
            
            // Visueller Kick
            flashTimer = 0.1f;
            if (beatIndicator != null) beatIndicator.color = activeColor;
        }

        // Timer für das Abklingen der LED
        if (flashTimer > 0)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0 && beatIndicator != null)
            {
                beatIndicator.color = new Color(0.1f, 0.1f, 0.1f, 1f);
            }
        }
    }
}