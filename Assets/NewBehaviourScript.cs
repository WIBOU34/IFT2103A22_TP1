using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    //private GameObject sphere;
    //private GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall";
        wall.transform.localScale.Set(0.5f, 3, 3);
        wall.transform.localPosition.Set(0, 0, 0);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.AddComponent<Rigidbody>();
        sphere.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
