using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] AudioClip[] MyTalk;
    [SerializeField] AudioClip[] SFX;
    [SerializeField] AudioClip[] BGM;

    Transform pool;
    Queue<AudioSource> soundQue = new Queue<AudioSource>();
    int chennel = 16;

    private void Awake()
    {
        pool = transform.Find("Sound").GetComponent<Transform>();

        for (int index = 0; index < chennel; index++)
        {
            CreateSoundPrefabs();
        }
    }
    void Start()
    {

    }

    private void CreateSoundPrefabs()
    {
        GameObject obj = new GameObject("SFX");
        obj.transform.SetParent(pool, false);
        AudioSource objs = obj.AddComponent<AudioSource>();
        obj.SetActive(false);
        soundQue.Enqueue(objs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"> 0 아빠 / 1 효과음</param>
    /// <param name="clipNumber"></param>
    public void PlaySound(int type, int clipNumber)
    {
        StartCoroutine(playSFX(type, clipNumber));
    }

    IEnumerator playSFX(int type, int clipNumber)
    {
        if (soundQue.Count <= 0)
        {
            CreateSoundPrefabs();
        }
        AudioSource obj = soundQue.Dequeue();

        AudioClip clip = null;
        switch (type)
        {
            case 0:
                clip = MyTalk[clipNumber];
                break;
            case 1:
                clip = SFX[clipNumber];
                break;
        }

        obj.clip = clip;
        obj.gameObject.SetActive(true); 
        obj.Play();
        
        yield return null;
        
        while (obj.isPlaying)
        {
            yield return null;
        }

        obj.clip = null;
        obj.gameObject.SetActive(false);
        soundQue.Enqueue(obj);
       

    }
}
