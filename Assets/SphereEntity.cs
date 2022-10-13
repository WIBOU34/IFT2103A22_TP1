using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEntity : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;
    public bool collisionWithWall = false;

    public bool dessusMur = false;
    public Vector3 impactPosition = Vector3.zero;
    public Vector3 BallPositionOnImpact = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<RigidBodyCustom>().CalculateInitialVelocityToHitTarget(targetPosition, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float elapsedTime = Time.fixedDeltaTime;
        this.GetComponent<RigidBodyCustom>().CalculatePositionAndVelocity(elapsedTime);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Stay");
    }

    //Event tiggered when sphere enters in collision with another object
    private void OnTriggerEnter(Collider other)
    {
        BallPositionOnImpact = this.gameObject.transform.position;
        if (other.gameObject == GameObject.Find("Wall"))
        {
            collisionWithWall = true;
        }

        Renderer sphereRenderer = this.gameObject.GetComponent<Renderer>();
        sphereRenderer.material.SetColor("_Color", Color.red);
        Vector3 velocite = this.gameObject.GetComponent<RigidBodyCustom>().Velocity;

        impactPosition = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        Bounds wallBounds = other.gameObject.GetComponent<Renderer>().bounds;

        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        Vector3 p3 = Vector3.zero;

        if (impactPosition.x == wallBounds.max.x) //Face droite
        {
            Debug.Log("Droite");
            p1 = wallBounds.max;
            p2 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
            p3 = new(wallBounds.max.x, wallBounds.min.y, wallBounds.min.z);
        }
        else if (impactPosition.x == wallBounds.min.x) //Face gauche
        {
            Debug.Log("Gauche");
            p1 = wallBounds.min;
            p2 = new(wallBounds.min.x, wallBounds.min.y, wallBounds.max.z);
            p3 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.max.z);
        }
        else if (impactPosition.z == wallBounds.max.z) //Face cachée
        {
            Debug.Log("Arrière");
            p1 = wallBounds.max;
            p2 = new(wallBounds.max.x, wallBounds.min.y, wallBounds.max.z);
            p3 = new(wallBounds.min.x, wallBounds.min.y, wallBounds.max.z);
        }
        else if (impactPosition.z == wallBounds.min.z) //face devant
        {
            Debug.Log("Devant");
            p1 = wallBounds.min;
            p2 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.min.z);
            p3 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
        }
        else if (impactPosition.y == wallBounds.max.y) //Top of the wall // && impactPosition.x != wallBounds.max.x && impactPosition.x != wallBounds.min.x && impactPosition.z != wallBounds.max.z && impactPosition.z != wallBounds.min.z
        {
            Debug.Log("Dessus");
            p1 = wallBounds.max;
            p2 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
            p3 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.min.z);
            dessusMur = true;
        }

        var dir = Vector3.Cross(p2 - p1, p3 - p1);
        Vector3 normal = Vector3.Normalize(dir).normalized;

        var dot = Vector3.Dot(normal, velocite);

        normal = new(2 * dot * normal.x, 2 * dot * normal.y, 2 * dot * normal.z);
        velocite -= normal;
        velocite = new(velocite.x, velocite.y / 2, velocite.z);

        this.gameObject.GetComponent<RigidBodyCustom>().Velocity = velocite;
    }
}
