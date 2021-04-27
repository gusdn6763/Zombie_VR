using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rigi;
    private TrailRenderer trailRenderer;

    [HideInInspector] public Transform shotPos;
    public float shotPower;
    public int damage;
    public bool shot = false;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!shot)
        {
            transform.position = shotPos.position;
            transform.rotation = shotPos.rotation;
        }
    }

    public void ArrowShoot(int damage)
    {
        shot = true;
        transform.LookAt(Player.instance.transform);
        rigi.AddForce(transform.forward * shotPower);
        StartCoroutine(BulletDisable());
        
    }

    private void OnDisable()
    {
        shot = false;
        trailRenderer.Clear();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rigi.Sleep();
    }


    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag(Constant.player))
        {
            collision.gameObject.GetComponent<Player>().Damaged(damage);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Shot()
    {
        throw new System.NotImplementedException();
    }
}