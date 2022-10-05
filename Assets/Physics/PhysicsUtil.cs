using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PhysicsUtil
{
    private static Vector3 gravity = Physics.gravity;

    // Algorithme MRUA
    public static Vector3 CalculatePosition(Vector3 position, Vector3 velocity, float timeElapsed, float mass)
    {
        Vector3 acceleration = CalculateAcceleration(mass);
        return position + velocity * timeElapsed + (timeElapsed * timeElapsed) * 0.5f * acceleration;
    }

    // Variation de position d'un élément en fonction du temps
    public static Vector3 CalculateVelocity(Vector3 position, float timeElapsed, float mass)
    {
        Vector3 acceleration = CalculateAcceleration(mass);
        return acceleration * timeElapsed;
    }

    public static Vector3 CalculateInitialVelocityToHitTarget(Vector3 objectPos, Vector3 targetPos, float airTime)
    {
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

        return new Vector3(xVelocity, yVelocity, zVelocity);
    }

    // calculates acceleration including gravity unless mass is 0
    private static Vector3 CalculateAcceleration(float mass = 1, List<Vector3> forces = null)
    {
        Vector3 acceleration = Vector3.zero;
        if (mass != 0)
            acceleration += gravity;
        else if (mass == 0)
            mass = 1; // évite la division par 0 (on ignore déjà la gravitée si 0 anyway)
        if (forces == null)
        {
            return gravity / mass;
        } else
        {
            foreach (Vector3 elem in forces)
            {
                acceleration += elem;
            }
            return acceleration / mass;
        }
    }


}
