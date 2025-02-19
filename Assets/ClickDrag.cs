using UnityEngine;

public class ClickDrag : MonoBehaviour
{

    public float forceAmount = 500;

    private Rigidbody dragObject;
    private Camera camera;

    private float selectionDistance;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //FindFirstObjectByType<Camera>();

        camera = Camera.main ? Camera.main : GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                selectionDistance = (ray.origin - hit.point).magnitude;

                dragObject = hit.rigidbody;
                
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            dragObject = null;
        }

        if (dragObject != null)
        {
            selectionDistance += Input.mouseScrollDelta.y;
        }
    }

    private void FixedUpdate()
    {
        if (dragObject)
        {
            Vector3 cameraObjectDelta = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,selectionDistance));

            Vector3 objectMoveDirection = cameraObjectDelta - dragObject.transform.position;

            dragObject.linearVelocity = objectMoveDirection * forceAmount;
        }
    }
}
