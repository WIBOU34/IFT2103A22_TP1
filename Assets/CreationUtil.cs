using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreationUtil
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;

    private const float X_CENTER = 0;
    private const float Y_CENTER = 0;
    private const float Z_CENTER = 0;
    // Start is called before the first frame update

    public void CreateSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 10f), Random.Range(-10f, 10f));
        sphere.transform.position = position;
        sphere.AddComponent<RigidBodyCustom>().Mass = 1;
        sphere.AddComponent<SphereEntity>();
        //sphere.AddComponent<Rigidbody>();
        //sphere.GetOrAddComponent<SphereCollider>().isTrigger = false;

        //PhysicMaterial material = new PhysicMaterial();
        //material.bounciness = 0.5f;
        //sphere.GetOrAddComponent<SphereCollider>().material = material;

        //Vector3 direction = Vector3.zero - position;
        //direction.Normalize();
        //sphere.GetComponent<Rigidbody>().AddForce(new Vector3(sphere.transform.position.x * -1, 0, sphere.transform.position.z * -1), ForceMode.VelocityChange);
        // TODO: calculer pour la gravitée
        //sphere.GetComponent<Rigidbody>().AddForce(Vector3.Distance(Vector3.zero, position) * direction, ForceMode.VelocityChange);

        //sphere.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;

    }

    public void CreateWall()
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall";
        wall.transform.position = new Vector3(X_CENTER, Y_CENTER, Z_CENTER);
        wall.transform.localScale = new Vector3(0.5f, 3, 3);
        wall.AddComponent<Rigidbody>().useGravity = false;
        wall.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        wall.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void CreateBounds()
    {
        GameObject bounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bounds.name = "Bounds";
        bounds.transform.position = new Vector3(0, -5, 0);
        bounds.transform.localScale = new Vector3(50, 0.1f, 50);
        bounds.GetOrAddComponent<BoxCollider>().isTrigger = true;
        bounds.AddComponent<Rigidbody>().useGravity = false;
        bounds.AddComponent<BoundsScript>();
    }
}
