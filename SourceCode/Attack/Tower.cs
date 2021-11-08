using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*타워의 조준과 공격행동에 대한 정의가 담긴 스크립트*/
public class Tower : MonoBehaviour
{

    private Transform target;
    private Monster targetMonster;

    [Header("General")]

    public float range = 1f;
    //0.15f

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]
    public string monsterTag = "Monster";

    public Transform partToRotate;
    public float turnSpeed = 5f;

    public Transform firePoint;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
        //(String methodName, float time, float repeatRate)
    }

    void UpdateTarget()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag(monsterTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestMonster = null;
        foreach (GameObject monster in monsters)
        {
            float distanceToMonster = Vector3.Distance(transform.position, monster.transform.position);
            if (distanceToMonster < shortestDistance)
            {
                shortestDistance = distanceToMonster;
                nearestMonster = monster;
            }
        }

        if (nearestMonster != null && shortestDistance <= range)
        {
            target = nearestMonster.transform;
            targetMonster = nearestMonster.GetComponent<Monster>();
        }
        else
        {
            target = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }

        LockOnTarget();

        
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
        

    }

    void LockOnTarget() // 몬스터 조준
    {
        if (turnSpeed == 0)
        {
            return;
        }
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
   
    
    void Shoot() // 총알 발사
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
            bullet.Seek(target);
  
    }
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
