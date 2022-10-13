using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEntity : MonoBehaviour
{
    private Vector3 targetPosition = Vector3.zero;
    public bool collisionWithWall = false;
    private bool collisionOccured = false;
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
        if (!collisionOccured)
        {
            float elapsedTime = Time.fixedDeltaTime;
            this.GetComponent<RigidBodyCustom>().CalculatePositionAndVelocity(elapsedTime);
        }
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
        collisionOccured = true;
        BallPositionOnImpact = this.gameObject.transform.position;
        if (other.gameObject == GameObject.Find("Wall"))
        {
            collisionWithWall = true;
        }

        Renderer sphereRenderer = this.gameObject.GetComponent<Renderer>();
        sphereRenderer.material.SetColor("_Color", Color.red);
        Vector3 velocite = this.gameObject.GetComponent<RigidBodyCustom>().Velocity;

        impactPosition = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        Bounds wallBounds = other.bounds;
        
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        Vector3 p3 = Vector3.zero;

        Vector3 normal = Vector3.zero;

        if (BallHitsACorner(impactPosition, wallBounds))
        {
            normal = (impactPosition - this.gameObject.transform.position).normalized;
        }
        else if (impactPosition.x == wallBounds.max.x) //Face droite
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
        else if (impactPosition.y == wallBounds.max.y) //Top of the wall
        {
            Debug.Log("Dessus");
            p1 = wallBounds.max;
            p2 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
            p3 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.min.z);
            dessusMur = true;
        }
        else if (impactPosition.y == wallBounds.min.y) //Dessous du mur
        {
            Debug.Log("Dessous");
            p1 = wallBounds.min;
            p2 = new(wallBounds.max.x, wallBounds.min.y, wallBounds.min.z);
            p3 = new(wallBounds.min.x, wallBounds.min.y, wallBounds.max.z);
        }

        if (normal == Vector3.zero)
        {
            var dir = Vector3.Cross(p2 - p1, p3 - p1);
            normal = Vector3.Normalize(dir).normalized;
        }

        var dot = Vector3.Dot(normal, velocite);

        normal = new(2 * dot * normal.x, 2 * dot * normal.y, 2 * dot * normal.z);
        velocite -= normal;
        velocite = new(velocite.x, velocite.y / 2, velocite.z);

        this.gameObject.GetComponent<RigidBodyCustom>().Velocity = velocite;
        collisionOccured = false;
    }

    private bool BallHitsACorner(Vector3 impactPosition, Bounds wallBounds)
    {
        return impactPosition.x == wallBounds.min.x && impactPosition.y == wallBounds.max.y && impactPosition.z == wallBounds.min.z || //Sommet Sup Avant Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.y == wallBounds.max.y && impactPosition.z == wallBounds.min.z || //Sommet Sup Avant Droit
               impactPosition.x == wallBounds.min.x && impactPosition.y == wallBounds.max.y && impactPosition.z == wallBounds.max.z || //Sommet Sup Arrière Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.y == wallBounds.max.y && impactPosition.z == wallBounds.max.z || //Sommet Sup Arrière Droit
               impactPosition.x == wallBounds.min.x && impactPosition.y == wallBounds.min.y && impactPosition.z == wallBounds.min.z || //Sommet Inf Avant Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.y == wallBounds.min.y && impactPosition.z == wallBounds.min.z || //Sommet Inf Avant Droit
               impactPosition.x == wallBounds.min.x && impactPosition.y == wallBounds.min.y && impactPosition.z == wallBounds.max.z || //Sommet Inf Arrière Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.y == wallBounds.min.y && impactPosition.z == wallBounds.max.z || //Sommet Inf Arrière Droit
               impactPosition.x == wallBounds.min.x && impactPosition.y == wallBounds.max.y || //Coin Supérieur Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.y == wallBounds.max.y || //Coin Supérieur Droit
               impactPosition.z == wallBounds.min.z && impactPosition.y == wallBounds.max.y || //Coin Supérieur Avant
               impactPosition.z == wallBounds.max.z && impactPosition.y == wallBounds.max.y || //Coin Supérieur Arrière
               impactPosition.x == wallBounds.min.x && impactPosition.z == wallBounds.min.z || //Coin Latéral Avant Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.z == wallBounds.min.z || //Coin Latéral Avant Droit
               impactPosition.x == wallBounds.min.x && impactPosition.z == wallBounds.max.z || //Coin Latéral Arrière Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.z == wallBounds.max.z || //Coin Latéral Arrière droit
               impactPosition.x == wallBounds.min.x && impactPosition.y == wallBounds.min.y || //Coin Inf Gauche
               impactPosition.x == wallBounds.max.x && impactPosition.y == wallBounds.min.y || //Coin Inf Droit
               impactPosition.z == wallBounds.min.z && impactPosition.y == wallBounds.min.y || //Coin Inf Avant
               impactPosition.z == wallBounds.max.z && impactPosition.y == wallBounds.min.y; //Coin Inf Arrière
    }
}
