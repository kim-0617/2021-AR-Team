using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*몬스터가 죽고 공격할 때 나는 소리를 관리하기 위한 스크립트*/
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource myAudio;

    public AudioClip sndEnemyAttack; 
    public AudioClip sndEnemyDie; 

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayEnemyAttack() // 공격시 사운드 재생하는 메소드
    {
        myAudio.PlayOneShot(sndEnemyAttack);
    }

    public void PlayEnemyDie() // 적 소멸시 사운드 재생하는 메소드
    {
        myAudio.PlayOneShot(sndEnemyDie);
    }

    void Update()
    {

    }
}
