using Unity.VisualScripting;
using UnityEngine;

public class CreationUtil
{
    // Définitions inutilisés recommandé par unity pour éviter des erreurs de compilation.
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;

    private const float X_CENTER = 0;
    private const float Y_CENTER = 0;
    private const float Z_CENTER = 0;

    public void CreateSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = "Ball";
        Vector3 position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 8f), Random.Range(-10f, 10f));
        sphere.transform.position = position;
        sphere.AddComponent<RigidBodyCustom>().Mass = 1;
        sphere.AddComponent<SphereEntity>();
        sphere.AddComponent<SphereCollider>().isTrigger = true;
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
        wall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        wall.AddComponent<WallEntity>();
    }

    public void CreateBounds()
    {
        GameObject bounds = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bounds.name = "Bounds";
        bounds.transform.position = new Vector3(X_CENTER, Y_CENTER - 5, Z_CENTER);
        bounds.transform.localScale = new Vector3(50, 0.1f, 50);
        bounds.GetOrAddComponent<BoxCollider>().isTrigger = false;
        bounds.AddComponent<Rigidbody>().useGravity = false;
        bounds.GetComponent<Rigidbody>().isKinematic = false;
        bounds.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        bounds.AddComponent<BoundsScript>();
    }
}
