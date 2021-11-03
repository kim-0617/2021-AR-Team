using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
/*몬스터에 대한 정보와 행동이 정의된 스크립트*/
public class Monster : MonoBehaviour
{
    

    [HideInInspector]
    private float startSpeed = 0.1f;
    //private float speed = 0.1f;

    public float startHealth = 100;
    private float health;

    public int worth = 50;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

    private bool isDead = false;
    public Unit movement;

    void Start()
    {
        if (movement != null)
        {
            startSpeed = movement.speed;
            //speed = startSpeed;
        }
        health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead) // 체력이 더 이상 없다면
        {
            Die();
            SoundManager.instance.PlayEnemyDie(); // 정의된 몬스터죽는 소리 재생
        }
    }

    public void ChangeSpeedSlow(float pct, float time)
    {
        movement.speed = startSpeed * (1f - (pct/100));

        Invoke("ResetSpeed", time); // time 초만큼 대기 후 Speed 원상복귀
    }

    public void ResetSpeed()
    {
        movement.speed = startSpeed;
    }

    void Die()
    {
        isDead = true;

        PlayerStats.Money += worth; // 골드 획득

        if (deathEffect != null)
        {
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }
        RoundSpawner.MonstersAlive--;

        Destroy(gameObject);
    }
}
