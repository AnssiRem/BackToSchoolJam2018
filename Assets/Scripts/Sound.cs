using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip Die;
    public AudioClip GameOver;
    public AudioClip Loot;
    public AudioClip Piano;
    public AudioClip Relax;
    public AudioClip Root;

    private AudioSource musicSource;
    private AudioSource rootSource;
    private AudioSource source;

    private void Start()
    {
        int music = Random.Range(0, 2);
        musicSource = GameObject.Find("/Main Camera/Music Source").GetComponent<AudioSource>();
        rootSource = GameObject.Find("/Main Camera/Root Source").GetComponent<AudioSource>();
        source = GameObject.Find("/Main Camera").GetComponent<AudioSource>();


        if (music == 0)
        {
            musicSource.clip = Relax;
        }
        else if (music == 1)
        {
            musicSource.clip = Piano;
        }

        musicSource.Play();
    }

    public void PlayDie()
    {
        source.PlayOneShot(Die, 1.5f);
    }

    public void PlayGameOver()
    {
        source.PlayOneShot(GameOver, 1.5f);
    }

    public void PlayLoot()
    {
        source.PlayOneShot(Loot);
    }

    public void PlayRoot()
    {
        rootSource.pitch = Random.Range(0.3f, 0.5f);
        rootSource.PlayOneShot(Root, 0.25f);
    }
}
