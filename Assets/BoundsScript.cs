using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsScript : MonoBehaviour
{
    private CreationUtil creationUtil;
    // Start is called before the first frame update
    void Start()
    {
        creationUtil = new CreationUtil();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        creationUtil.CreateSphere();
    }
}
