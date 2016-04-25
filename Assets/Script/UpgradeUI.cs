using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class UpgradeUI : MonoBehaviour {
	public Text PowerText;
	public Text HealthText;
	public Text SpeedText;
	public Text NotEnoughText;
	public Upgrade[] powerUpgrades = {new Upgrade(10,60),new Upgrade(15,80),new Upgrade(20,150),new Upgrade(30,200)};

	public Upgrade[] healthUpgrades = {new Upgrade(100,50),new Upgrade(200,80),new Upgrade(300,120),new Upgrade(400,150),new Upgrade(600,200)};

	public Upgrade[] speedUpgrades = {new Upgrade(1.0f,30),new Upgrade(1.2f,50),new Upgrade(1.5f,70),new Upgrade(2f,150),new Upgrade(2.5f,200)};

	private int[] curUpgrades = { 0, 0, 0 };
	private AudioManager audioManager;
	public PlayerController player;
	public Weapon playerWeapon;

	public class Upgrade {
		public float amt;
		public int price;
		public Upgrade (float a,int p) {
			amt = a;
			price = p;
		}
	}

	void Start() {
		audioManager = AudioManager.instance;
		player = GameObject.Find ("nerdyguy").gameObject.GetComponent<PlayerController>();
		playerWeapon = GameObject.Find ("nerdyguy").gameObject.GetComponentInChildren<Weapon>();
	}
	void Awake() {

	}
	public void Power() {
		audioManager.PlaySound ("ButtonClick");
		int upgradeResult = UpgradeStats (0);
		switch (upgradeResult) {
		case -1:
			NotEnoughText.text = "Already at max Hack Power!";
			break;
		case 0:
			NotEnoughText.text = "Not enough H-Points: Need " + powerUpgrades [curUpgrades[0]].price;
			break;
		case 1:
			NotEnoughText.text = "";
			PowerText.text = "int Hack Power\t= " + powerUpgrades[curUpgrades[0]-1].amt;
			break;
		}

		//GameMaster.gm.upgradePlayer (0,5);
	}
	public void Health() {
		audioManager.PlaySound ("ButtonClick");
		int upgradeResult = UpgradeStats (1);
		switch (upgradeResult) {
		case -1:
			NotEnoughText.text = "Already at max Health!";
			break;
		case 0:
			NotEnoughText.text = "Not enough H-Points: Need " + healthUpgrades [curUpgrades[1]].price;
			break;
		case 1:
			NotEnoughText.text = "";
			HealthText.text = "int Health\t\t= " + healthUpgrades[curUpgrades[1]-1].amt;
			break;
		}
	}
	public void Speed() {
		audioManager.PlaySound ("ButtonClick");
		int upgradeResult = UpgradeStats (2);
		switch (upgradeResult) {
		case -1:
			NotEnoughText.text = "Already at max Speed!";
			break;
		case 0:
			NotEnoughText.text = "Not enough H-Points: Need " + speedUpgrades [curUpgrades[2]].price;
			break;
		case 1:
			NotEnoughText.text = "";
			SpeedText.text = "float Speed\t\t= " + speedUpgrades[curUpgrades[2]-1].amt;
			break;
		}
	}

	public int UpgradeStats (int i) {
		
		switch (i) {
		// power
		case 0:
			// check if at final upgrade
			if (curUpgrades [0] > powerUpgrades.Length - 1) {
				return -1;
			}
			// check if not enough money
			else if (powerUpgrades [curUpgrades [0]].price > GameMaster.gm.getMoney ()) {
				return 0;
			}
			GameMaster.gm.increaseMoney (powerUpgrades [curUpgrades [0]].price * -1);
			playerWeapon.SwitchWeapon (curUpgrades [0]);
			curUpgrades [0]++;
			if (curUpgrades [0] > powerUpgrades.Length - 1)
				return -1;
			break;

		// health
		case 1:
			// check if at final upgrade
			if (curUpgrades [1] > healthUpgrades.Length - 1) {
				return -1;
			}
			// check if not enough money
			else if (healthUpgrades [curUpgrades [1]].price > GameMaster.gm.getMoney ()) {
				return 0;
			}

			GameMaster.gm.increaseMoney (healthUpgrades [curUpgrades [1]].price * -1);
			player.UpgradeHealth ((int) healthUpgrades [curUpgrades [1]].amt);
			curUpgrades[1]++;
			break;
		// speed
		case 2:
			// check if at final upgrade
			if (curUpgrades[2] > speedUpgrades.Length - 1) {
				return -1;
			}
			// check if not enough money
			else if (speedUpgrades [curUpgrades[2]].price > GameMaster.gm.getMoney()) {
				return 0;
			}

			GameMaster.gm.increaseMoney (speedUpgrades [curUpgrades[2]].price * -1);
			player.speed = speedUpgrades [curUpgrades[2]].amt;
			curUpgrades[2]++;
			break;
		default:
			break;
		}
		return 1;
	}
}
