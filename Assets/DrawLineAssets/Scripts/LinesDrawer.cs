using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LinesDrawer : MonoBehaviour {

	public GameObject linePrefab;
	public LayerMask cantDrawOverLayer;
	int cantDrawOverLayerIndex;

	[Space ( 300f )]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;

[SerializeField]	Line currentLine;

	Camera cam;
	bool drawing = false;
	GameObject followObject;
	int helpNo;
	void Start ( ) {
		cam = Camera.main;
		cantDrawOverLayerIndex = LayerMask.NameToLayer ( "CantDrawOver" );
	}
	GameObject helperTeamTemp;
	[SerializeField] Material[] lineMaterial;
	void Update ( ) {
        //if (Input.GetMouseButtonDown(0))
        //{
        //	Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        //	RaycastHit hit;
        //	//RaycastHit raycastHit;
        //	if (Physics.Raycast(raycast, out hit))
        //	{
        //		Debug.Log("road1");
        //		if (hit.collider.name == "road")
        //		{
        //			Debug.Log("road");

        //		}
        //	}
        //}
        if (Input.GetMouseButtonDown(0))
        {
			Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
			//RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out hit))
			{

				if (hit.collider.GetComponent<IHelper>() != null)
				{
					helperTeamTemp = hit.collider.gameObject;
					Globals.cameraMove = false;
					Debug.Log("hospital");

					BeginDraw();
					drawing = true;
					followObject = hit.collider.gameObject;
					helpNo = hit.collider.GetComponent<IHelper>().helpNo;
					currentLine.GetComponent<LineRenderer>().material = lineMaterial[hit.collider.GetComponent<IHelper>().helpNo];

				}
			}

		}

		//if (Input.GetMouseButton(0))

		if (currentLine != null && drawing)
            Draw();

		if (Input.GetMouseButtonUp(0))
		{
			Globals.cameraMove = true;
			Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
			//RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out hit))
			{
				if (hit.collider.GetComponent<IHelper>() != null)
				{
					EndDraw();
					//StartCoroutine(following());
					helperTeamTemp.GetComponent<IHelper>().helper(currentLine, hit.collider.gameObject);
				}
                else
                {
					if (currentLine != null && drawing)
					{
						drawing = false;
						StartCoroutine(lineDestroy());
					}
                }
			}
			drawing = false;

		}
	}

	// Begin Draw ----------------------------------------------
	void BeginDraw ( ) {
		currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );

		//Set line properties
		currentLine.UsePhysics ( false );
		currentLine.SetLineColor ( lineColor );
		currentLine.SetPointsMinDistance ( linePointsMinDistance );
		currentLine.SetLineWidth ( lineWidth );

	}
	// Draw ----------------------------------------------------
	void Draw ( ) {

		Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
		//RaycastHit raycastHit;
		if (Physics.Raycast(raycast, out hit))
        {
			Debug.Log("draw");

			if (hit.collider.GetComponent<IRoad>() != null)
			{
				Debug.Log("road");


				currentLine.AddPoint(hit.point);
                //Debug.Log("road" + hit.point);
                //if (Physics.SphereCast(mousePosition, lineWidth / 3f, Vector3.zero, out hit, 10))
                //{
                //    EndDraw();
                //}
                //else
                //    currentLine.AddPoint(hit.point);
            }
		}

		//if (Physics.SphereCast(mousePosition, lineWidth / 3f, Vector3.zero, out hit, 1))
		//{
		//	EndDraw();
		//}
		//else
		//	currentLine.AddPoint(mousePosition);

		//Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
		//RaycastHit2D hit = Physics2D.CircleCast ( mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer );

	}
	// End Draw ------------------------------------------------
	void EndDraw ( ) {
		if ( currentLine != null ) {
			if ( currentLine.pointsCount < 2 ) {
				//If line has one point
				Destroy ( currentLine.gameObject );
			} else {
				//Add the line to "CantDrawOver" layer
				//currentLine.gameObject.layer = cantDrawOverLayerIndex;

				//Activate Physics on the line
				currentLine.UsePhysics ( true );

				//currentLine = null;
			}
		}
	}
	IEnumerator lineDestroy()
    {
		while(currentLine.GetComponent<LineRenderer>().positionCount > 2)
        {

			currentLine.GetComponent<LineRenderer>().positionCount -= 2;
			yield return null;
        }
		currentLine.UsePhysics(true);
		Destroy(currentLine.gameObject);

		currentLine = null;

	}
	IEnumerator following()
    {
		float posNo = 0f;
		while(currentLine.GetComponent<LineRenderer>().positionCount > posNo)
        {
			//followObject.transform.position = currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo);
			followObject.transform.position = Vector3.MoveTowards(followObject.transform.position, currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo), 50 * Time.deltaTime);
			if (posNo + 1 < currentLine.GetComponent<LineRenderer>().positionCount)
			{
				Vector3 dir = (currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo + 1) - followObject.transform.position);
				dir.Normalize();
				//if(dir.x == 0)
    //            {
				//	dir.x = 0.1f;
    //            }
	


				Vector3 direction = new Vector3(dir.x, 0f, dir.z);

				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


				followObject.transform.rotation = Quaternion.RotateTowards(followObject.transform.rotation, newRot, 500 * Time.deltaTime);
                //followObject.transform.rotation = Quaternion.Euler(followObject.transform.eulerAngles.x, Mathf.Atan(dir.z/dir.x) *180/Mathf.PI , followObject.transform.eulerAngles.z);
            }
            else
            {
				Vector3 dir = (currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1) - followObject.transform.position);
				dir.Normalize();

				Vector3 direction = new Vector3(dir.x, 0f, dir.z);

				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


				followObject.transform.rotation = Quaternion.RotateTowards(followObject.transform.rotation, newRot, 800 * Time.deltaTime);
			}
			//followObject.transform.position = currentLine.GetComponent<LineRenderer>().GetPosition(posNo);
			if(Vector3.Distance( followObject.transform.position, currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo)) < 5f)
            {
				posNo += 1f;
            }
			posNo += Time.deltaTime * 30;
			yield return null;
        }
		while (Vector3.Distance(followObject.transform.position, currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1)) > 1f)
        {
			followObject.transform.position = Vector3.MoveTowards(followObject.transform.position, currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1), 50 * Time.deltaTime);
			yield return null;
		}

	}
	private void ApplySteer(GameObject followObj, Vector3 lookPos)
	{
		Vector3 relativeVector = transform.InverseTransformPoint(lookPos);
		relativeVector /= relativeVector.magnitude;
		float newSteer = (relativeVector.x / relativeVector.magnitude) * 50;
		followObj.transform.Rotate(0, newSteer * Time.deltaTime * 10, 0);
	}
}
