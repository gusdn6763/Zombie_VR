using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyRotation : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private Light keyLight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, Time.realtimeSinceStartup * speed, 0);
    }

    private void OnDestroy()
    {
        Destroy(keyLight);
    }
}
