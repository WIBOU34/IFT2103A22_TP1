using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class StartCreation : MonoBehaviour
{
    private CreationUtil creationUtil;
    //private GameObject sphere;
    //private GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        creationUtil = new CreationUtil();
        creationUtil.CreateBounds();
        creationUtil.CreateWall();
        creationUtil.CreateSphere();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
