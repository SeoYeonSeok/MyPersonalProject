using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    private static AudioMgr instance;

    public AudioSource bgmSource;

    public AudioClip[] clips;

    private void Awake()
    {
        // AudioManager �ν��Ͻ��� �����ϰ�, ���� ��ȯ�Ǿ �ı����� �ʵ��� �մϴ�.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // AudioSource ������ ������Ʈ�ϴ� �޼���
    public void UpdateAudioSettings(float volume, AudioClip clip)
    {
        bgmSource.volume = volume;
        bgmSource.clip = clip;
        // �ʿ��� �ٸ� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
    }    
}
