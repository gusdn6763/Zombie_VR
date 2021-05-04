using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BulletCtrl : MonoBehaviour
{
    private Rigidbody rigi;
    private TrailRenderer trailRenderer;

    public Transform barrelLocation;
    public float shotPower;
    public int damage;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    private void OnEnable()
    {
        if (barrelLocation != null)
        {
            transform.position = barrelLocation.position;
            transform.rotation = barrelLocation.rotation;
            rigi.AddForce(barrelLocation.forward * shotPower);
            
            StartCoroutine(BulletDisable());
        }
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rigi.Sleep();
    }


    IEnumerator BulletDisable()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.zombiePart))
        {
            other.GetComponent<Part>().Damaged(damage, transform.position);
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constant.monster))
        {
            collision.gameObject.GetComponent<Mob>().Damaged(damage, transform.position);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag(Constant.zombiePart))
        {
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
