using UnityEngine;

public class CompassMarkerScript : MonoBehaviour
{
    [Header("Camera")]
    //public Camera camera;

    public GameObject associatedTargetObj;
    public int associatedTargetObjId;

    private int xUpperBound = 200;
    private int xLowerBound = -200;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Update the x coordinate of the obj
        // @source: https://www.youtube.com/watch?v=XcpTC1VYVNE
        Vector3 cameraPos = Camera.main.transform.position;
         Vector3 directionToTarget = associatedTargetObj.transform.position - cameraPos;
        float signedAngle = Vector3.SignedAngle(new Vector3( Camera.main.transform.forward.x, 0,  Camera.main.transform.forward.z), new Vector3(directionToTarget.x, 0, directionToTarget.z), Vector3.up);
        float compassPosX = Mathf.Clamp(2 * signedAngle / Camera.main.fieldOfView, -0.5f, 0.5f);
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(400 * compassPosX, 0);

    }

    public void setAssociatedTargetId(GameObject newTarget, int newTargetId)
    {
        associatedTargetObj = newTarget;
        associatedTargetObjId = newTargetId;
    }
}
