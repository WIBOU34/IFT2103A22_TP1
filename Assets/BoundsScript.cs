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
        Debug.Log("Collision avec le sol");
        Destroy(other.gameObject);
        //creationUtil.CreateSphere();  //Commenter pour le moment ça duplique les sphères pour je ne sais quelle raison en ajoutant sphere.
                                        //AddComponent<SphereCollider>().isTrigger = true; lors de la création de la sphère
    }
}
