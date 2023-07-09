using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeManager : MonoBehaviour
{
    public static ScreenFadeManager Instance { get; private set; }

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image image;

    private void Awake()
    {
        Instance = this;
    }

    /*
    [SerializeField, Range(0, 1)] private float testSlider = 0;

    private void Update()
    {
        SetFade(testSlider);
    }
    */

    public void SetFade(float fade)
    {
        if (fade > 0)
            canvas.enabled = true;

        image.color = new Color32(62, 59, 70, (byte)(255 * fade));
    }
}
