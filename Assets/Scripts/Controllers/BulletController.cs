using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class BulletController : MonoBehaviour
{
    ShipStat _shipstat;

    [SerializeField]
    Vector3 _target;
    [SerializeField]
    AudioClip HitSound;
    [SerializeField]
    GameObject HitEffect;
    [SerializeField]
    float HitEffectDuration;
    [SerializeField]
    int Damage;
    [SerializeField]
    GameObject BulletMovePoint;
    BoxCollider2D[] MovePoint;

    bool TouchEdge;

    WeaponController weaponController;

    void Start()
    {
        TouchEdge = false;
        MovePoint = BulletMovePoint.GetComponentsInChildren<BoxCollider2D>();
        weaponController = transform.parent.GetComponent<WeaponController>();
        GetTarget();
    }

    void Update()
    {

    }

    void GetTarget()
    {
        _target = weaponController.TARGET;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            transform.position = MovePoint[(int)Random.Range(1, 7)].transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = 11;
            TouchEdge = true;

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 direction = (Vector3)(_target - screenPoint);
            direction.Normalize();
            gameObject.GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);

            transform.right = direction;

            Invoke("DestroyBullet", 0.5f);
        }

        if ((collision.gameObject.tag == "EnemyShipTile") && TouchEdge)
        {
            Vector3 dir = (Vector3)(_target - Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 0, 10f));

            if (dir.magnitude < 10f)
            {
                _shipstat = collision.gameObject.transform.parent.GetComponent<ShipStat>();
                _shipstat.Hp -= Damage;
                OnHit();
                Managers.Sound.Play(HitSound, Sound.Effect);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector3 dir = (Vector3)(_target - Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 0, 10f));

        if ((collision.gameObject.tag == "EnemyShipTile") && TouchEdge)
        {
            if (dir.magnitude < 10f)
            {
                _shipstat = collision.gameObject.transform.parent.GetComponent<ShipStat>();
                _shipstat.Hp -= Damage;
                OnHit();
                Managers.Sound.Play(HitSound, Sound.Effect);
                Destroy(gameObject);
            }
        }
    }

    void OnHit()
    {
        GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);
        effect.transform.position = transform.position;
        effect.GetComponent<Animator>().Play("HitAnim");
        Destroy(effect, HitEffectDuration);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
