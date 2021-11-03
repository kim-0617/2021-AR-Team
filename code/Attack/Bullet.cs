using UnityEngine;
/*타워가 발사하는 총알에 대한 정보와 행동이 담긴 스크립트*/
public class Bullet : MonoBehaviour
{

    private Transform target;

    public float speed = 1f;

    public int damage = 50;

    public float explosionRadius = 0f;
    public GameObject impactEffect;

    public bool slow = false;       // 충돌시 감속 여부
    public float slow_pct = 0f;     // 속도 감소율 %
    public float slow_time = 0f;    // 속도 감소 시간 (초)

    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {

        if (target == null)
        {
            Destroy(gameObject);
            return;
            //target이 없는 경우
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);

    }

    void HitTarget()
    {
        
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
            
        }
        Destroy(gameObject);
        
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.gameObject);
            if (collider.CompareTag("Monster")) // 몬스터일시 폭발
            {
                Debug.Log("Explode!");
                Damage(collider.transform);
                if (slow)
                {
                    Debug.Log("Slow!");
                    Slow(collider.transform);
                }
            }
        }
    }

    void Damage(Transform monster)
    {
        Monster m = monster.GetComponent<Monster>();

        if (m != null)
        {
            m.TakeDamage(damage);
            if (slow)
            {
                Slow(monster);
            }
        }
    }
    void Slow(Transform monster)
    {
        Monster m = monster.GetComponent<Monster>();

        if (m != null)
        {
            m.ChangeSpeedSlow(slow_pct, slow_time);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
