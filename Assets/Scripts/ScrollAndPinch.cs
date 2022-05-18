/*

Set this on an empty game object positioned at (0,0,0) and attach your active camera.
The script only runs on mobile devices or the remote app.

*/

using UnityEngine;
using TMPro;
class ScrollAndPinch : MonoBehaviour,IStartGameObserver
{
#if UNITY_IOS || UNITY_ANDROID
    public Camera Camera;
    public bool Rotate;
    protected Plane Plane;
    //public TextMeshProUGUI zoomText;
    [SerializeField] Vector2 horizontalCameraBounding;
    [SerializeField] Vector2 verticalCameraBounding;

    private void Awake()
    {
        if (Camera == null)
            Camera = Camera.main;
    }
    private void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
        Globals.cameraMove = false;

    }
    public void StartGame()
    {
        Globals.cameraMove = true;

    }
    private void Update()
    {
        if (Globals.cameraMove)
        {
            cameraAction();
        }
    }
    void cameraAction()
    {
        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount >= 1)
        {
            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (Camera.transform.position.x <= horizontalCameraBounding.x && Delta1.x < 0)
                {
                    Camera.transform.position = new Vector3(horizontalCameraBounding.x, Camera.transform.position.y, Camera.transform.position.z);
                }
                else if (Camera.transform.position.x >= horizontalCameraBounding.y && Delta1.x > 0)
                {
                    Camera.transform.position = new Vector3(horizontalCameraBounding.y, Camera.transform.position.y, Camera.transform.position.z);
                }
                else if (Camera.transform.position.z <= verticalCameraBounding.x && Delta1.z < 0)
                {
                    Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y, verticalCameraBounding.x);
                }
                else if (Camera.transform.position.z >= verticalCameraBounding.y && Delta1.z > 0)
                {
                    Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y, verticalCameraBounding.y);
                }
                else
                {
                    Camera.transform.Translate(Delta1 * 2, Space.World);
                }

            }
        }

        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            //calc zoom
            float zoom;
            zoom = Vector3.Distance(pos1, pos2) /
                       Vector3.Distance(pos1b, pos2b);
            //zoomText.text = zoom.ToString();

            //edge case
            if (zoom == 0 || zoom > 10)
            {
                return;
            }
            if (Camera.transform.position.y <= 10 && zoom > 1)
            {
                zoom = 1;
                pos1 = Vector3.zero;
            }
            else if (Camera.transform.position.y >= 150 && zoom < 1)
            {
                zoom = 1;
                pos1 = Vector3.zero;
            }
            Globals.iconScale =Mathf.Sqrt( Camera.transform.position.y) / 12;
            RenderSettings.fogEndDistance = 270 + Camera.transform.position.y * 0.7f;
            Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);
            //zoomText.text = Camera.transform.position.y + "/" + zoom;
            //Camera.nearClipPlane = Camera.transform.position.y + 50;
            //Move cam amount the mid ray

            if (Rotate && pos2b != pos2)
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
        }
    }
    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
#endif
}