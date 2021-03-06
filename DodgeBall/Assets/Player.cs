using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 200;
	public float maxSpeed = 1200;
	public string horizontalInput;
	public string verticalInput;
	private FixedJoint ballJoint;
	public string catchBallInput;

	void Start() {
		if (horizontalInput == null || verticalInput == null)
			throw new UnityException("One or more inputs aren't specified");
	}
	
	void FixedUpdate () {
		Vector3 movement = new Vector3 (Input.GetAxisRaw (horizontalInput), 0.0f, Input.GetAxisRaw (verticalInput)) * Time.deltaTime * speed;
		rigidbody.MovePosition(rigidbody.position + movement);
		//rigidbody.AddForce (movement);

		if(Input.GetKey(KeyCode.O))
		{
			if (ballJoint)
			{
				var ballBody = ballJoint.connectedBody;
				if(ballBody.gameObject.tag == "ball")
				{
					ballBody.AddForce (Vector3.left * 3000 * Time.deltaTime, ForceMode.Impulse);

					Destroy(ballJoint);
					ballJoint = null;
				}
			}
		}
		
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "ball")
		{
			GameObject ball = col.gameObject;


			if(Input.GetKey(KeyCode.L))
			{
				ballJoint = null;
				ballJoint = gameObject.AddComponent<FixedJoint>();
				ballJoint.connectedBody = col.rigidbody;
				ballJoint.breakForce = 150;
			}
			else if(GameManager.lastCollidedPlayer != this.gameObject)
			{
				ScoreManager.AddScoreToPlayer(name);
				GameObject textP1 = GameObject.Find("Player 1 score");
				TextMesh textMeshP1 = textP1.GetComponent<TextMesh>();
				GameObject textP2 = GameObject.Find("Player 2 score");
				TextMesh textMeshP2 = textP2.GetComponent<TextMesh>();

				textMeshP1.text = "P1 score: " + ScoreManager.Player1Score;
				textMeshP2.text = "P2 score: " + ScoreManager.Player2Score;
			}

			GameManager.lastCollidedPlayer = this.gameObject;
		}

		
	}
}
