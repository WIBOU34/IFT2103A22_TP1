using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEntity : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;

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

    //Event tiggered when sphere enters in collision with another object
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("La sphere entre en collision!");
        Renderer sphereRenderer = this.gameObject.GetComponent<Renderer>();
        sphereRenderer.material.SetColor("_Color", Color.red);
    }
}
