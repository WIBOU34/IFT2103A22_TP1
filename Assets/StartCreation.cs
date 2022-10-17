using UnityEngine;
using UnityEngine.UIElements;

public class StartCreation : MonoBehaviour
{
    private CreationUtil creationUtil;
    private Camera[] cameras;
    // Start is called before the first frame update
    void Start()
    {
        creationUtil = new CreationUtil();
        creationUtil.CreateBounds();
        creationUtil.CreateWall();
        creationUtil.CreateSphere();
        cameras = Camera.allCameras;
        EnableCamera(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnableCamera(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnableCamera(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnableCamera(2);
        }
    }

    private void EnableCamera(int n)
    {
        foreach (Camera cam in cameras)
        {
            cam.enabled = false;
        }
        cameras[n].enabled = true;
    }
}
