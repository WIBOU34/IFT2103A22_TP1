using UnityEngine;

public class BoundsScript : MonoBehaviour
{
    private CreationUtil creationUtil;
    // Start is called before the first frame update
    void Start()
    {
        creationUtil = new CreationUtil();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision avec le sol!");
        creationUtil.CreateSphere();
        Destroy(other.gameObject);
    }
}
