using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SkillCooldownUI : MonoBehaviour {
	public List<SkillInfo> skills;
	[System.Serializable]
	public class SkillInfo {
		public float cd;
		public Image skillIcon;
		[HideInInspector]
		public float currentcd;
		public SkillInfo(float cd) {
			
		}
	}
	void FixedUpdate() {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (SkillInfo s in skills) {
			if (s.currentcd < s.cd) {
				s.currentcd += Time.deltaTime;
				s.skillIcon.fillAmount = s.currentcd / s.cd;

			}
		}
	}

	void showSkillIcon(int skillIndex) {
	
	}

	public void showCD(int skillIndex) {
		if (skills.ElementAtOrDefault (skillIndex) == null) {
		}
		skills [skillIndex].currentcd = 0;
	}

	public void addSkillUI (int skillIndex) {
		skills [skillIndex].skillIcon.enabled = false;
	}


}
