using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Ingredient : MonoBehaviour
{
    public string id;
    public AudioClip[] sounds = new AudioClip[0];

    public Vector3 DefaultScale { get; private set; }

    private AudioSource _audioSource;
    private bool _isCollected;
    private bool _isHeld;
    private float _releaseTime;

    public void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        var audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        var audioMixGroup = audioMixer.FindMatchingGroups("SFX");
        _audioSource.outputAudioMixerGroup = audioMixGroup[0];
        DefaultScale = transform.localScale;
    }

    public bool CanHold()
    {
        if (_isCollected) return false;
        if (_isHeld) return false;
        return true;
    }

    public void Hold()
    {
        _isHeld = true;
        gameObject.SetLayerRecursively("HeldItem");
    }

    public void Release()
    {
        _isHeld = false;
        _releaseTime = Time.time;
        gameObject.SetLayerRecursively("Default");
    }

    public bool CanCollect()
    {
        if (_isCollected) return false;
        if (_isHeld) return false;
        return Time.time - _releaseTime < 1f;
    }

    public void Collect()
    {
        _isCollected = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (sounds.Length > 0)
        {
            _audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            _audioSource.Play();
        }
    }
}
