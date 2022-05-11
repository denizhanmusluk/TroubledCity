﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
	public GameObject linePrefab;
	public LayerMask cantDrawOverLayer;
	int cantDrawOverLayerIndex;

	[Space(300f)]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;

	[SerializeField] Line currentLine;

	Camera cam;
	public bool drawing = false;
	GameObject followObject;
	int helpNo;
	void Start()
	{
		cam = Camera.main;
		cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
	}
	[SerializeField] GameObject helperTeamTemp;
	[SerializeField] Material[] lineMaterial;
	void Update()
	{
		clickAction();
	}
	void clickAction()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
			//RaycastHit raycastHit;
			if (Physics.Raycast(raycast, out hit))
			{

				if (hit.collider.transform.name == transform.name)
				{
					helperTeamTemp = hit.collider.gameObject;
					Globals.cameraMove = false;
					Debug.Log("hospital");

					BeginDraw();
					drawing = true;
					followObject = hit.collider.gameObject;
					helpNo = hit.collider.GetComponent<HelperTeam>().helpNo;
					currentLine.GetComponent<LineRenderer>().material = lineMaterial[hit.collider.GetComponent<HelperTeam>().helpNo];

				}
			}

		}


		if (currentLine != null && drawing)
			Draw();

		if (Input.GetMouseButtonUp(0))
		{
			Globals.cameraMove = true;
			Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
			if (Physics.Raycast(raycast, out hit))
			{
				if (GetComponent<HelperTeam>().helpDrawActive && drawing)
				{
					if (hit.collider.GetComponent<IHelper>() != null && hit.collider.tag == "troublearea")
					{
						EndDraw();
						//StartCoroutine(following());
						this.GetComponent<HelperTeam>().helper(currentLine, hit.collider.gameObject);
						Debug.Log("deneme");
					}
					else
					{
						if (currentLine != null && drawing && GetComponent<HelperTeam>().helpDrawActive)
						{
							Debug.Log("DESTROY");
							drawing = false;
							StartCoroutine(lineDestroy());
						}
					}
				}
			}
			drawing = false;

		}
	}
	// Begin Draw ----------------------------------------------
	void BeginDraw()
	{
		currentLine = Instantiate(linePrefab).GetComponent<Line>();

		//Set line properties
		currentLine.UsePhysics(false);
		currentLine.SetLineColor(lineColor);
		currentLine.SetPointsMinDistance(linePointsMinDistance);
		currentLine.SetLineWidth(lineWidth);

	}
	// Draw ----------------------------------------------------
	void Draw()
	{

		Ray raycast = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
		//RaycastHit raycastHit;
		if (Physics.Raycast(raycast, out hit))
		{
			if (hit.collider.GetComponent<IRoad>() != null)
			{
				currentLine.AddPoint(hit.point);
			}
		}
	}
	// End Draw ------------------------------------------------
	void EndDraw()
	{
		if (currentLine != null)
		{
			if (currentLine.pointsCount < 2)
			{
				Destroy(currentLine.gameObject);
			}
			else
			{

				currentLine.UsePhysics(true);
			}
		}
	}
	IEnumerator lineDestroy()
	{
		while (currentLine.GetComponent<LineRenderer>().positionCount > 2)
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
		while (currentLine.GetComponent<LineRenderer>().positionCount > posNo)
		{
			followObject.transform.position = Vector3.MoveTowards(followObject.transform.position, currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo), 50 * Time.deltaTime);
			if (posNo + 1 < currentLine.GetComponent<LineRenderer>().positionCount)
			{
				Vector3 dir = (currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo + 1) - followObject.transform.position);
				dir.Normalize();


				Vector3 direction = new Vector3(dir.x, 0f, dir.z);

				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


				followObject.transform.rotation = Quaternion.RotateTowards(followObject.transform.rotation, newRot, 500 * Time.deltaTime);
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
			if (Vector3.Distance(followObject.transform.position, currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo)) < 5f)
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
}
