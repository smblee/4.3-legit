using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
	public float damage;
	public float cd = 2.0f;
	public float lifetime = 3.0f;
	public bool slow = true;

	public bool delayed = true;

	public float timeBorn ;

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag != "Player" && other.tag != "Bullet" && other.tag!= "Skill"){
			//Destroy (gameObject);
		}
	}
	void Start() {
		if (delayed)
			timeBorn = Time.time;
	}


}
