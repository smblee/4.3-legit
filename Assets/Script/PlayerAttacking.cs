using UnityEngine;
using System.Collections;

public class PlayerAttacking : MonoBehaviour {
	private float cd;
	public Weapon weapon;
	public Skill skill;
	private float[] cd_skill={-1.0f,-1.0f,-1.0f,-1.0f};
	private bool[] skill_owned ={false,false,false,false};

	private GameObject skillPf_heal;
	private GameObject skillPf;
	[SerializeField]
	private SkillCooldownUI cdUI; 

	private Vector3 lastPosition = new Vector3(0,1,0);
	private AudioManager audioManager;
	private PlayerController pc;


	void Start () {
		audioManager = AudioManager.instance;
		weapon = transform.FindChild ("Weapon").GetComponent<Weapon>();
		skill = transform.FindChild ("Skill").GetComponent<Skill>();
		pc = transform.GetComponent<PlayerController> ();
	}

	// Update is called once per frame
	void Update () {
		Vector3 movement_vector = new Vector3 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"), 0f);
		if (movement_vector != Vector3.zero) lastPosition = movement_vector;
		if (Time.time >= cd) {
			if (Input.GetButton("Fire1")) {
				Fire ();
			}
		}

		for (int i = 1; i < skill_owned.Length+1; i++){
			if (skill_owned[i-1] && cd_skill[i-1] > -1){
				if (Time.time >= cd_skill[i-1]){
					if (Input.GetButton("Skill"+i)){
						Skill(i);
					}
				}
			}
		}

		if (skillPf_heal != null)
			skillPf_heal.transform.position = transform.position;
	}

	void Fire() {
		GameObject bulletPf = Instantiate(weapon.getCurrentWeapon(), transform.position, transform.rotation) as GameObject;
	//	GameObject bulletPf2 = Instantiate(weapon.bulletPrefab, transform.position, transform.rotation) as GameObject;
		//GameObject bulletPf3 = Instantiate(weapon.bulletPrefab, transform.position, transform.rotation) as GameObject;
		audioManager.PlaySound (weapon.sound);

		// rotation of bullet handler
		float rotateDegree = 
			lastPosition.x == 0 && lastPosition.y != 0 ? 90 :
			lastPosition.x > 0 && lastPosition.y > 0 || lastPosition.x < 0 && lastPosition.y < 0 ? 45 :
			lastPosition.x > 0 && lastPosition.y < 0 || lastPosition.x < 0 && lastPosition.y > 0 ? 135 : 0; 
		
		// rotate according to player direction
		bulletPf.transform.Rotate (new Vector3 (0, 0, rotateDegree));
	//	bulletPf2.transform.Rotate (new Vector3 (0, 0, rotateDegree+10));
		//bulletPf3.transform.Rotate (new Vector3 (0, 0, rotateDegree-10));

		// fire to the direction
		bulletPf.GetComponent<Rigidbody2D> ().AddForce (lastPosition * weapon.bulletSpeed);
	//	bulletPf2.GetComponent<Rigidbody2D> ().AddForce ((new Vector3(lastPosition.x+0.2f,lastPosition.y+0.2f,lastPosition.z+0.2f)) * weapon.bulletSpeed);
		//bulletPf3.GetComponent<Rigidbody2D> ().AddForce (lastPosition * weapon.bulletSpeed);
		cd = Time.time + weapon.attackSpeed;
	}

	void Skill(int index){
		float _cd, _lifetime;
		Vector2 pos;
		//If the skill is healing
		switch (index){
			case 2:
				skillPf_heal = Instantiate(skill.getCurrentSkill(index), transform.position, transform.rotation) as GameObject;
				_cd = skillPf_heal.gameObject.GetComponent<SkillScript> ().cd;
				cdUI.showCD (index - 1, _cd);
				pc.HealPlayer();
				_lifetime = skillPf_heal.gameObject.GetComponent<SkillScript> ().lifetime;
				audioManager.PlaySound ("Skill" + index, _lifetime);
				cd_skill[index-1]= Time.time + _cd;
				Destroy(skillPf_heal, _lifetime);
				break;
			case 4:
				pos = new Vector2(0, 0);
				skillPf = Instantiate(skill.getCurrentSkill(index), pos, transform.rotation) as GameObject;	
				_lifetime = skillPf.gameObject.GetComponent<SkillScript> ().lifetime;
				_cd = skillPf.gameObject.GetComponent<SkillScript> ().cd;
				cdUI.showCD (index - 1, _cd);
				audioManager.PlaySound ("Skill" + index, _lifetime);
				cd_skill[index-1]= Time.time + _cd;
				Destroy(skillPf, _lifetime);
				break;		
			case 3:
				
				//upwards
				if (lastPosition.x == 0 && lastPosition.y > 0)
					pos = new Vector2(transform.position.x, transform.position.y+2);
				else if (lastPosition.x == 0 && lastPosition.y < 0) //downwards
					pos = new Vector2(transform.position.x, transform.position.y-2);
				else if (lastPosition.x > 0 && lastPosition.y > 0 ) //right up
					pos = new Vector2(transform.position.x+2, transform.position.y+2);
				else if (lastPosition.x > 0 && lastPosition.y < 0 ) //right down
					pos = new Vector2(transform.position.x+2, transform.position.y-2);
				else if (lastPosition.x < 0 && lastPosition.y > 0 ) //left up
					pos = new Vector2(transform.position.x-2, transform.position.y+2);
				else if (lastPosition.x < 0 && lastPosition.y < 0 ) //left down
					pos = new Vector2(transform.position.x-2, transform.position.y-2);
				else if (lastPosition.x > 0 && lastPosition.y == 0 ) //right
					pos = new Vector2(transform.position.x+2, transform.position.y);
				else if (lastPosition.x < 0 && lastPosition.y == 0 ) //left
					pos = new Vector2(transform.position.x-2, transform.position.y);
				else 
					pos = new Vector2(0 ,0);

				if (pos.x > 4.6f)
					pos.x = 4.6f;
				if (pos.x < -4.6f)
					pos.x = -4.6f;
				if (pos.y > 3.56f)
					pos.y = 3.56f;
				if (pos.y < -3.56f)
					pos.y = -3.56f;

				skillPf = Instantiate(skill.getCurrentSkill(index), pos, transform.rotation) as GameObject;	
				_lifetime = skillPf.gameObject.GetComponent<SkillScript> ().lifetime;
				_cd = skillPf.gameObject.GetComponent<SkillScript> ().cd;
				cdUI.showCD (index - 1, _cd);
				audioManager.PlaySound ("Skill" + index);
				cd_skill[index-1]= Time.time + _cd;
				Destroy(skillPf, _lifetime);
				break;		
			default:
				skillPf = Instantiate(skill.getCurrentSkill(index), transform.position, transform.rotation) as GameObject;			
				_lifetime = skillPf.gameObject.GetComponent<SkillScript> ().lifetime;
				_cd = skillPf.gameObject.GetComponent<SkillScript> ().cd;
				cdUI.showCD (index - 1, _cd);
				audioManager.PlaySound ("Skill" + index, _lifetime);
				cd_skill[index-1]= Time.time + _cd;
				Destroy(skillPf, _lifetime);
				break;
		}
	}

	public void addSkill(int index){
		skill_owned[index-1] = true;
		cd_skill[index-1] = 0;
		cdUI.addSkillUI (index-1);
	}

}
