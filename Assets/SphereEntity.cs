using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEntity : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;
    public bool collisionWithGround = false;
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

            //Top of the wall 
            if (impactPosition.y == wallBounds.max.y && impactPosition.x != wallBounds.max.x && impactPosition.x != wallBounds.min.x && impactPosition.z != wallBounds.max.z && impactPosition.z != wallBounds.min.z)
            {
                Vector3 p1 = wallBounds.max;
                Vector3 p2 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
                Vector3 p3 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.min.z);

                var dir = Vector3.Cross(p2 - p1, p3 - p1);
                normal = Vector3.Normalize(dir).normalized;

            }

            var dot = Vector3.Dot(normal, velocite);

            normal = new(2 * dot * normal.x, 2 * dot * normal.y, 2 * dot * normal.z);
            velocite -= normal;
            velocite = new(velocite.x / 4, velocite.y / 4, velocite.z / 4);

            this.gameObject.GetComponent<RigidBodyCustom>().Velocity = velocite;
        }        
    }
}
