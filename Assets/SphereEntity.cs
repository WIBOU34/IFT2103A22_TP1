using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEntity : MonoBehaviour
{
    private Vector3 velocity = new Vector3(0, 0, 0);
    private Vector3 targetPosition = Vector3.zero;
    private float mass = 1;
    // Start is called before the first frame update
    void Start()
    {
        velocity = PhysicsUtil.CalculateInitialVelocityToHitTarget(this.gameObject.transform.position, targetPosition, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float elapsedTime = Time.fixedDeltaTime;
        velocity += PhysicsUtil.CalculateVelocity(this.gameObject.transform.position, elapsedTime, mass);
        Vector3 position = PhysicsUtil.CalculatePosition(this.gameObject.transform.position, velocity, elapsedTime, mass);
        this.gameObject.transform.position = position;
    }
}
