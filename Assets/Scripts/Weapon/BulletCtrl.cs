using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BulletCtrl : MonoBehaviour 
{
    private Rigidbody rigi;
    private TrailRenderer trailRenderer;

    public Transform barrelLocation;
    public float damage;
    public float shotPower;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }
    private void OnEnable()
    {
        transform.position = barrelLocation.position;
        transform.rotation = barrelLocation.rotation;
        rigi.velocity = barrelLocation.forward * shotPower;
        StartCoroutine(BulletDisable());
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
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.gameObject.CompareTag("Monster"))
        {
            Col.GetComponent<MovingObject>().Damaged(damage);
            gameObject.SetActive(false);
        }
        else if (Col.gameObject.CompareTag("Part"))
        {
            //Col.gameObject<>();
            gameObject.SetActive(false);
        }

    }
}
