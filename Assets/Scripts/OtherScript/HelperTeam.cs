using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperTeam : MonoBehaviour
{
	[SerializeField] public int equipSpeed;
	[SerializeField] public float troubleSolutionSpeed;

	public int helpNo;
	public bool crash = false;
	Vector3 forceDirection;
	[SerializeField] GameObject[] waterParticle;
[SerializeField]	GameObject _troubleArea;
	Line _currentLine;
	int currentCrashNode;
	Vector3 firstPos;
	Quaternion firstRot;
	bool npcHitActive = false;
	bool reMovingActive = false;
	public bool helpDrawActive { get; set; }

	[SerializeField] GameObject questionMark;

	[SerializeField] GameObject policePrefab, doctorPrefab, engineerPrefab, fireFighterPrefab;
	private void Start()
	{
		questionMark.SetActive(false);
		helpDrawActive = true;
		firstPos = transform.position;
		firstRot = transform.rotation;
	}
	void Update()
	{

		//if (Input.GetMouseButtonDown(0))
		//{
		//	Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
		//	RaycastHit hit;
		//	Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//	//RaycastHit raycastHit;
		//	if (Physics.Raycast(raycast, out hit))
		//	{
		//		if (hit.collider.GetComponent<IHelper>() != null && reMovingActive)
		//		{
		//			reMovingActive = false;
		//			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		//			StartCoroutine(reMove(_currentLine.GetComponent<LineRenderer>().positionCount - 1));
		//		}
		//	}
		//}
	}
	public void helper(Line currentLine, GameObject troubleArea)
	{
		helpDrawActive = false;
		_currentLine = currentLine;
		_troubleArea = troubleArea;
		StartCoroutine(following(currentLine));

		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		//GetComponent<LinesDrawer>().enabled = false;
	}
	IEnumerator following(Line currentLine)
	{
		crash = false;

		npcHitActive = true;
		float posNo = 0f;
		while (currentLine.GetComponent<LineRenderer>().positionCount > posNo)
		{
			if (crash)
			{
				//crash = false;
				crashCar();
				currentCrashNode = (int)posNo;
				break;
			}
            if (_troubleArea == null)
            {
                currentCrashNode = (int)posNo;
                StartCoroutine(reMove(currentCrashNode));

                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo), equipSpeed * Time.deltaTime);
			if (posNo + 1 < currentLine.GetComponent<LineRenderer>().positionCount)
			{
				Vector3 dir = (currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo + 1) - transform.position);
				dir.Normalize();

				Vector3 direction = new Vector3(dir.x, 0f, dir.z);

				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, equipSpeed* 6f * Time.deltaTime);
			}
			else
			{
				Vector3 dir = (currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1) - transform.position);
				dir.Normalize();

				Vector3 direction = new Vector3(dir.x, 0f, dir.z);

				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, equipSpeed * 6f  * Time.deltaTime);
			}
			while (Vector3.Distance(transform.position, currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo)) < 5f && posNo < currentLine.GetComponent<LineRenderer>().positionCount - 1)
			{
				posNo += 1f;
			}
			posNo += Time.deltaTime * equipSpeed/12.5f;
			yield return null;
		}
		while (Vector3.Distance(transform.position, currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1)) > 1f)
		{
			if (crash)
			{
				crash = false;
				//crashCar();
				break;
			}
            if (_troubleArea == null)
            {
		
				break;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1), equipSpeed * Time.deltaTime);
			yield return null;
		}
		if (Vector3.Distance(transform.position, currentLine.GetComponent<LineRenderer>().GetPosition(currentLine.GetComponent<LineRenderer>().positionCount - 1)) <= 1f)
		{
			if (_troubleArea != null)
				StartCoroutine(fireExtinguishing());
		}
		npcHitActive = false;
	}
	IEnumerator coverOpen()
    {
		float counter = 0f;
        while (counter < 2f)
        {
			counter += Time.deltaTime;
			transform.GetChild(4).transform.localRotation = Quaternion.Euler(-60 * counter, transform.GetChild(4).transform.localEulerAngles.y, transform.GetChild(4).transform.localEulerAngles.z);
			yield return null;
        }
		transform.GetChild(4).transform.localRotation = Quaternion.Euler(-120, transform.GetChild(4).transform.localEulerAngles.y, transform.GetChild(4).transform.localEulerAngles.z);

	}
	IEnumerator fireExtinguishing()
	{
		if (_troubleArea.GetComponent<IHelper>().helpNo == helpNo)
		{
			_troubleArea.GetComponent<IHelper>().helpNo = -1;
			Globals.tutorialFireCam = true;
			_troubleArea.GetComponent<troubleArea>().iconSizeDown();
			if (helpNo == 3)
			{
				StartCoroutine(coverOpen());
				GameObject engineer = Instantiate(engineerPrefab, transform.position, Quaternion.identity);
				engineer.GetComponent<engineer>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform.GetChild(0);
				engineer.GetComponent<engineer>().point1 = transform.GetChild(0);
				engineer.GetComponent<engineer>().point2 = transform.GetChild(1);
				engineer.GetComponent<engineer>().point3 = transform.GetChild(2);
				engineer.GetComponent<engineer>().point4 = transform.GetChild(3);
			}
			if (helpNo == 2)
            {
				if (_troubleArea.GetComponent<troubleArea>().fireParticle.transform.childCount > 0)
				{
					GameObject police = Instantiate(policePrefab, transform.position, Quaternion.identity);
					police.GetComponent<police>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform.GetChild(0);
				}
            }
			if (helpNo == 1)
			{
				//GameObject doctor = Instantiate(doctorPrefab, transform.position, Quaternion.identity);
				//doctor.GetComponent<doctor>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform.GetChild(0);
				StartCoroutine(doctorSpawn());


			}
			if (helpNo == 0)
			{
				StartCoroutine(fireFigterSpawn());
				//GameObject fireFighter = Instantiate(fireFighterPrefab, new Vector3(transform.position.x, -76.26f, transform.position.z)+ transform.forward * 2 + new Vector3(transform.right.x,0, transform.right.x) * 4, Quaternion.identity);
				//fireFighter.GetComponent<fireFighter>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform;
				//fireFighter.GetComponent<fireFighter>().firstPos = transform;

				//GameObject fireFighter2 = Instantiate(fireFighterPrefab, new Vector3(transform.position.x, -76.26f, transform.position.z) + transform.forward * 2 - new Vector3(transform.right.x, 0, transform.right.x)* 4, Quaternion.identity);
				//fireFighter2.GetComponent<fireFighter>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform;
				//fireFighter2.GetComponent<fireFighter>().firstPos = transform;

			}

			GetComponent<helpProgress>().progressing(troubleSolutionSpeed);
			for (int i = 0; i < waterParticle.Length; i++)
			{
				waterParticle[i].SetActive(true);
			}
			float counter = 0f;
			while (counter < troubleSolutionSpeed)
			{
				counter += Time.deltaTime;

				Vector3 relativeVector = transform.InverseTransformPoint(_troubleArea.transform.parent.parent.transform.position);
				relativeVector /= relativeVector.magnitude;
				float newSteer = (relativeVector.x / relativeVector.magnitude) * 50;
				transform.Rotate(0, newSteer * Time.deltaTime * 10, 0);
				if (helpNo == 0)
				{
					for (int i = 0; i < _troubleArea.transform.GetChild(0).childCount; i++)
					{
						_troubleArea.transform.GetChild(0).GetChild(i).transform.localScale = new Vector3(1f - (counter / troubleSolutionSpeed), 1f - (counter / troubleSolutionSpeed), 1f - (counter / troubleSolutionSpeed));
						if(counter> troubleSolutionSpeed)
                        {
							_troubleArea.transform.GetChild(0).GetChild(i).transform.localScale = new Vector3(0,0,0);

						}
					}
				}
				yield return null;
			}
			moneyCanvas.Instance.moneySpawn(_troubleArea.transform.parent.position, 6);
			for (int i = 0; i < _troubleArea.transform.GetChild(0).childCount; i++)
			{
				_troubleArea.transform.GetChild(0).GetChild(i).transform.localScale = new Vector3(1, 1, 1);
				_troubleArea.transform.GetChild(0).gameObject.SetActive(false);

			}
			_troubleArea.transform.parent = null;
			TroubleManager.Instance.uiDir.targetList.Remove(_troubleArea);
			Destroy(_troubleArea.GetComponent<troubleArea>().troubleCanvasIcon);
			Destroy(_troubleArea);
			for (int i = 0; i < waterParticle.Length; i++)
			{
				waterParticle[i].SetActive(false);
			}
			Globals.tutorialFireCam = false;
			if (helpNo == 3)
			{
				transform.GetChild(4).transform.localRotation = Quaternion.Euler(0, transform.GetChild(4).transform.localEulerAngles.y, transform.GetChild(4).transform.localEulerAngles.z);

			}
		}
		else
        {
			questionMark.SetActive(true);
			yield return new WaitForSeconds(2f);
			questionMark.SetActive(false);
		}
		StartCoroutine(reMove(_currentLine.GetComponent<LineRenderer>().positionCount - 1));
		yield return null;
		reMovingActive = true;
		//gameObject.layer = LayerMask.NameToLayer("Default");

	}
	IEnumerator doctorSpawn()
	{
		yield return new WaitForSeconds(0.5f);
		GameObject doctor = Instantiate(doctorPrefab, new Vector3(transform.position.x, -76.26f, transform.position.z) + new Vector3(transform.right.x, 0, transform.right.x) * 8, Quaternion.identity);
		doctor.GetComponent<doctor>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform.GetChild(0);
	}
	IEnumerator fireFigterSpawn()
    {
		yield return new WaitForSeconds(0.5f);
		GameObject fireFighter = Instantiate(fireFighterPrefab, new Vector3(transform.position.x, -76.26f, transform.position.z) + new Vector3(transform.right.x, 0, transform.right.x) * 8, Quaternion.identity);
		fireFighter.GetComponent<fireFighter>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform;
		fireFighter.GetComponent<fireFighter>().firstPos = transform;

		GameObject fireFighter2 = Instantiate(fireFighterPrefab, new Vector3(transform.position.x, -76.26f, transform.position.z) - new Vector3(transform.right.x, 0, transform.right.x) * 8, Quaternion.identity);
		fireFighter2.GetComponent<fireFighter>().targetGuilty = _troubleArea.GetComponent<troubleArea>().fireParticle.transform;
		fireFighter2.GetComponent<fireFighter>().firstPos = transform;
	}
	IEnumerator reMove(int startNode)
	{
		float posNo = startNode;
		while (0 < posNo)
		{
			//followObject.transform.position = currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo);

			transform.position = Vector3.MoveTowards(transform.position, _currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo), equipSpeed * Time.deltaTime);
			if (posNo >= 1)
			{
				Vector3 dir = (_currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo - 1) - transform.position);
				dir.Normalize();
				//if(dir.x == 0)
				//            {
				//	dir.x = 0.1f;
				//            }



				Vector3 direction = new Vector3(dir.x, 0f, dir.z);

				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, equipSpeed * 6f * Time.deltaTime);
				//followObject.transform.rotation = Quaternion.Euler(followObject.transform.eulerAngles.x, Mathf.Atan(dir.z/dir.x) *180/Mathf.PI , followObject.transform.eulerAngles.z);
			}
			else
			{
				Vector3 dir = (_currentLine.GetComponent<LineRenderer>().GetPosition(0) - transform.position);
				dir.Normalize();

				Vector3 direction = new Vector3(dir.x, 0f, dir.z);

				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				Quaternion newRot = Quaternion.Euler(0, targetAngle, 0);


				transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, equipSpeed * 6f * Time.deltaTime);
			}
			//followObject.transform.position = currentLine.GetComponent<LineRenderer>().GetPosition(posNo);
			while (Vector3.Distance(transform.position, _currentLine.GetComponent<LineRenderer>().GetPosition((int)posNo)) < 10f && posNo > 1)
			{
				posNo -= 1f;
			}
			posNo -= Time.deltaTime * equipSpeed / 12.5f; 
			yield return null;
		}
		while (Vector3.Distance(transform.position, _currentLine.GetComponent<LineRenderer>().GetPosition(0)) > 1f)
		{

			transform.position = Vector3.MoveTowards(transform.position, _currentLine.GetComponent<LineRenderer>().GetPosition(0), equipSpeed * 6f * Time.deltaTime);
			yield return null;
		}
		while (Vector3.Distance(transform.position, firstPos) > 0.1f)
		{

			transform.position = Vector3.MoveTowards(transform.position, firstPos, equipSpeed * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, firstRot, equipSpeed * 25 * Time.deltaTime);
			yield return null;
		}
		StartCoroutine(lineDestroy());
		crash = false;
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.GetComponent<Vehicle>() != null)
		{
			Debug.Log("hit vehicle");
			crash = true;
			forceDirection = (transform.position - collision.transform.position).normalized;
			collision.transform.GetComponent<Vehicle>().currentSelection = Vehicle.States.stopMove;
			//transform.GetComponent<Rigidbody>().AddForce(transform.up + forceDirection * 500);
			collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * 1500 - forceDirection * 1200);
		}
		if (collision.transform.GetComponent<HelperTeam>() != null)
		{
			crash = true;
			forceDirection = (transform.position - collision.transform.position).normalized;
			//transform.GetComponent<Rigidbody>().AddForce(transform.up + forceDirection * 500);
			collision.transform.GetComponent<Rigidbody>().AddForce(transform.up * 1500 - forceDirection * 1200);
		}
		if (collision.transform.tag == "light")
		{
			if (collision.transform.GetComponent<Rigidbody>() == null)
			{
				collision.gameObject.AddComponent<Rigidbody>();
			}
			forceDirection = (transform.position - collision.transform.position).normalized;

			collision.transform.GetComponent<Rigidbody>().AddForce(collision.transform.up * 15 - forceDirection * 7);
			collision.transform.GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);

		}
		if (collision.transform.tag == "roadbounding")
		{
			Debug.Log("wall");
			crash = true;
			forceDirection = (collision.transform.position - transform.position).normalized;
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<NPC>() != null && npcHitActive)
		{
			other.GetComponent<NPC>().npcDead((other.transform.position - transform.position).normalized);
		}
		if (other.GetComponent<guilty>() != null && npcHitActive)
		{
			other.GetComponent<guilty>().npcDead((other.transform.position - transform.position).normalized);
		}
	}
	IEnumerator lineDestroy()
	{
		while (_currentLine.GetComponent<LineRenderer>().positionCount > 2)
		{

			_currentLine.GetComponent<LineRenderer>().positionCount -= 2;
			yield return null;
		}

		_currentLine.UsePhysics(true);
		Debug.Log("destroy");

		Destroy(_currentLine.gameObject);

		_currentLine = null;
		helpDrawActive = true;
		gameObject.layer = LayerMask.NameToLayer("Default");

	}
	void crashCar()
	{
		transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		transform.GetComponent<Rigidbody>().AddForce(transform.up * 1500 + forceDirection * 700);
		transform.GetComponent<Rigidbody>().AddTorque(new Vector3(forceDirection.z, 0, forceDirection.x) * 10000);
		StartCoroutine(crashDelay());
		Debug.Log(crash);
	}
	IEnumerator crashDelay()
	{
		yield return new WaitForSeconds(3f);
		StartCoroutine(reMove(currentCrashNode));
		transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

	}
}
