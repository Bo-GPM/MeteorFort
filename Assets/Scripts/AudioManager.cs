using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource[] AudioSources;
    [HideInInspector] static public AudioManager audioInstance;

    private void Awake()
    {
        //Singleton
        if (audioInstance == null)
        {
            audioInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    public void PlayAudio(int index)
    {
        //get index and play audio
        AudioSources[index].Play();
    }
    public IEnumerator PlayAudioAfterDelay(int index, float delayTime)
    {
        // 等待几秒
        yield return new WaitForSeconds(delayTime);

        AudioSources[index].Play();

    }
}

