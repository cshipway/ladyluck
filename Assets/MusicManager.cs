using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject activeWatcher;
    [SerializeField] private Transform cameraTf;
    private AudioSource audioSource;
    private float currentAnim = 1;
    private float velocity;

    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool a = activeWatcher.activeSelf;

        float target = a ? 0: 1;

        currentAnim = Mathf.SmoothDamp(currentAnim, target, ref velocity, 0.25f);

        audioSource.pitch = 0.5f + (currentAnim * 0.5f);
        audioSource.volume = (0.5f + (currentAnim * 0.5f)) * 0.25f;

        cameraTf.transform.position = Vector3.Lerp(new Vector3(0, 2.5f, 0), new Vector3(0, 1.25f, -0.3f), currentAnim);
        cameraTf.transform.localEulerAngles = Vector3.Lerp(new Vector3(90, 0, 0), new Vector3(80, 0, 0), currentAnim);
    }
}
