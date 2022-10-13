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
    private int stayTriggerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<RigidBodyCustom>().CalculateInitialVelocityToHitTarget(targetPosition, 1.7f);
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
        this.GetComponent<RigidBodyCustom>().IgnoreGravity = false;
        stayTriggerCount = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        stayTriggerCount++;
        this.GetComponent<RigidBodyCustom>().IgnoreGravity = true;
        Vector3 velocity = this.GetComponent<RigidBodyCustom>().Velocity;
        if (Mathf.Abs(velocity.x) < 0.0001f && Mathf.Abs(velocity.z) < 0.0001f || stayTriggerCount > 20)
        {
            Debug.Log("Ball was stuck on top of or in the wall!");
            Vector3 target = this.gameObject.transform.position;
            this.GetComponent<RigidBodyCustom>().CalculateInitialVelocityToHitTarget(new(target.x + 1, target.y, target.z), 0.5f);
        }
    }

    //Event tiggered when sphere enters in collision with another object
    private void OnTriggerEnter(Collider other)
    {
        collisionOccured = true;
        BallPositionOnImpact = this.gameObject.transform.position;
        if (other.gameObject == GameObject.Find("Wall"))
        {
            collisionWithWall = true;
        } else
        {
            return; // on ne veut pas gérer les collisions avec les autres objets que le mur
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
        else if (impactPosition.y == wallBounds.max.y) //Top of the wall
        {
            p1 = wallBounds.max;
            p2 = new(wallBounds.max.x, wallBounds.max.y, wallBounds.min.z);
            p3 = new(wallBounds.min.x, wallBounds.max.y, wallBounds.min.z);
            dessusMur = true;
        }
        else if (impactPosition.y == wallBounds.min.y) //Dessous du mur
        {
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
        float velociteY = velocite.y;
        if (Mathf.Abs(velociteY) < 1)
        {
            velocite = new(velocite.x / 1.20f, velocite.y, velocite.z / 1.20f);
        } else
        {
            velocite = new(velocite.x / 1.20f, velocite.y / 1.8f, velocite.z / 1.20f);
        }

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
