using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEntity : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;
    private bool collisionWithWall = false;

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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision ball");
    }

    //Event tiggered when sphere enters in collision with another object
    private void OnTriggerEnter(Collider other)
    {
        if (!collisionWithWall)
        {
            collisionWithWall = true;
            Debug.Log("La sphere entre en collision!");
            Renderer sphereRenderer = this.gameObject.GetComponent<Renderer>();
            sphereRenderer.material.SetColor("_Color", Color.red);
            Vector3 position = this.gameObject.transform.position;
            Vector3 velocite = this.gameObject.GetComponent<RigidBodyCustom>().Velocity;

            Vector3 impactPosition = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            Bounds wallBounds = other.gameObject.GetComponent<Renderer>().bounds;

            Vector3 normal = new(1, 1, 1);

            Vector3 p1 = Vector3.zero;
            Vector3 p2 = Vector3.zero;
            Vector3 p3 = Vector3.zero;

            //Top of the wall 
            if (impactPosition.y == wallBounds.max.y) // && impactPosition.x != wallBounds.max.x && impactPosition.x != wallBounds.min.x && impactPosition.z != wallBounds.max.z && impactPosition.z != wallBounds.min.z
            {
                collisionWithWall = false; //Pour ne pas que la balle traverse à travers le mur...
                p1 = wallBounds.max;
                p2 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
                p3 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.min.z);
            }
            else if (impactPosition.x == wallBounds.max.x) //Face droite
            {
                p1 = wallBounds.max;
                p2 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
                p3 = new(wallBounds.max.x, wallBounds.min.y, wallBounds.min.z);
            }
            else if (impactPosition.x == wallBounds.min.x) //Face gauche
            {
                p1 = wallBounds.min;
                p2 = new(wallBounds.min.x, wallBounds.min.y, wallBounds.max.z);
                p3 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.max.z);
            }
            else if (impactPosition.z == wallBounds.max.z) //Face cachée
            {
                p1 = wallBounds.max;
                p2 = new(wallBounds.max.x, wallBounds.min.y, wallBounds.max.z);
                p3 = new(wallBounds.min.x, wallBounds.min.y, wallBounds.max.z);
            }
            else if (impactPosition.z == wallBounds.min.z) //face devant
            {
                p1 = wallBounds.min;
                p2 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.min.z);
                p3 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
            }

            var dir = Vector3.Cross(p2 - p1, p3 - p1);
            normal = Vector3.Normalize(dir).normalized;

            var dot = Vector3.Dot(normal, velocite);

            normal = new(2 * dot * normal.x, 2 * dot * normal.y, 2 * dot * normal.z);
            velocite -= normal;
            velocite = new(velocite.x / 2, velocite.y / 2, velocite.z / 2);

            this.gameObject.GetComponent<RigidBodyCustom>().Velocity = velocite;
        }        
    }
}
