using UnityEngine;

public class RigidBodyCustom : MonoBehaviour
{
    public static Vector3 gravity = Physics.gravity;
    public Vector3 velocity = Vector3.zero;
    private float mass;
    private bool ignoreGravity = false;

    public Vector3 Velocity { get => this.velocity; set => this.velocity = value; }
    public float Mass { get => mass; set => mass = value; }
    public bool IgnoreGravity { get => ignoreGravity; set => ignoreGravity = value; }

    // Algorithme MRUA
    public void CalculatePositionAndVelocity(float timeElapsed)
    {
        Vector3 forces = Vector3.zero;
        if (!ignoreGravity)
        {
            forces = gravity;
        }
        Vector3 acceleration = CalculateAcceleration(forces);
        CalculateVelocity(timeElapsed, acceleration);
        CalculatePosition(timeElapsed, acceleration);
    }

    public void CalculatePosition(float timeElapsed, Vector3 acceleration)
    {
        this.transform.position = this.transform.position + this.velocity * timeElapsed + (timeElapsed * timeElapsed) * 0.5f * acceleration;
    }

    // Variation de position d'un élément en fonction du temps
    public void CalculateVelocity(float timeElapsed, Vector3 acceleration)
    {
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

    // calculates acceleration
    private Vector3 CalculateAcceleration(Vector3 forces)
    {
        return forces / mass;
    }
}
