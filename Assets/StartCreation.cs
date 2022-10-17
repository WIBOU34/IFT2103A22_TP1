using UnityEngine;

public class StartCreation : MonoBehaviour
{
    private CreationUtil creationUtil;
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
