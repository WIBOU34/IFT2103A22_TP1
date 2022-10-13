using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCustom : MonoBehaviour
{
    private static Vector3 gravity = Physics.gravity;
    private Vector3 velocity = Vector3.zero;
    private float mass;

    public Vector3 Velocity { get => this.velocity; set => this.velocity = value; }
    public float Mass { get => mass; set => mass = value; }

    // Algorithme MRUA
    public void CalculatePositionAndVelocity(float timeElapsed)
    {
        CalculateVelocity(timeElapsed);
        CalculatePosition(timeElapsed);

        if (this.gameObject == GameObject.Find("Ball") && this.gameObject.GetComponent<SphereEntity>().collisionWithWall && BallPenetratesWall(timeElapsed))
        {
            float positionY = GameObject.Find("Wall").GetComponent<Renderer>().bounds.max.y + this.gameObject.GetComponent<SphereCollider>().radius;
            this.transform.position = new Vector3(this.transform.position.x, positionY, this.transform.position.z);
        }
    }

    public void CalculatePosition(float timeElapsed)
    {
        Vector3 acceleration = CalculateAcceleration();
        this.transform.position = this.transform.position + this.velocity * timeElapsed + (timeElapsed * timeElapsed) * 0.5f * acceleration;
    }

    // Variation de position d'un élément en fonction du temps
    public void CalculateVelocity(float timeElapsed)
    {
        Vector3 acceleration = CalculateAcceleration();
        velocity += acceleration * timeElapsed;
    }

    // Source des formules https://lambdageeks.com/how-to-find-velocity-with-height-and-distance/
    public void CalculateInitialVelocityToHitTarget(Vector3 targetPos, float airTime)
    {
        Vector3 objectPos = this.transform.position;
        float height = targetPos.y - objectPos.y;
        float lengthX = targetPos.x - objectPos.x;
        float lengthZ = targetPos.z - objectPos.z;

        // lengthX = velocity(x) * t
        float xVelocity = lengthX / airTime;
        // lengthZ = velocity(z) * t
        float zVelocity = lengthZ / airTime;
        // h = velocity(y) * t + 1/2 * gravity * t^2
        // -> velocity(y) = h/t - 1/2 * gravity * t
        float yVelocity = height / airTime - 0.5f * gravity.y * airTime;


        velocity.x = xVelocity;
        velocity.y = yVelocity;
        velocity.z = zVelocity;
    }

    // calculates acceleration including gravity unless mass is 0
    private Vector3 CalculateAcceleration(List<Vector3> forces = null)
    {
        Vector3 acceleration = Vector3.zero;
        if (mass != 0)
            acceleration += gravity;
        else if (mass == 0)
            mass = 1; // évite la division par 0 (on ignore déjà la gravitée si 0 anyway)
        if (forces == null)
        {
            return gravity / mass;
        }
        else
        {
            foreach (Vector3 elem in forces)
            {
                acceleration += elem;
            }
            return acceleration / mass;
        }
    }

    private bool BallPenetratesWall(float timeElapsed)
    {
        bool ballPenetratesWall = false;

        if (this.gameObject == GameObject.Find("Ball"))
        {
            SphereEntity sphereEntity = this.gameObject.GetComponent<SphereEntity>();
            Vector3 ballPositionOnImpact = sphereEntity.BallPositionOnImpact;

            Vector3 translation = this.transform.position - ballPositionOnImpact;
            Vector3 impactPositionOnBall = sphereEntity.impactPosition + translation;

            GameObject wall = GameObject.Find("Wall");
            Bounds wallBounds = wall.gameObject.GetComponent<Renderer>().bounds;
            
            if (impactPositionOnBall.x < wallBounds.max.x && impactPositionOnBall.x > wallBounds.min.x &&
                impactPositionOnBall.y < wallBounds.max.y && impactPositionOnBall.y > wallBounds.min.y &&
                impactPositionOnBall.z < wallBounds.max.z && impactPositionOnBall.z > wallBounds.min.z)
            {
                ballPenetratesWall = true;
            }
        }

        return ballPenetratesWall;
    }
}
