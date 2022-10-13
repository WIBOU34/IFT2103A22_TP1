using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class WallEntity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Event tiggered when collision occured with the wall
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision avec le mur!");
    }
}
