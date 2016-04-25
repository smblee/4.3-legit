using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



public class GameOverUI : MonoBehaviour {
	private AudioManager audioManager;
	void Start() {
		audioManager = AudioManager.instance;
	}
	public void Quit() {
		audioManager.PlaySound ("ButtonClick");
		audioManager.StopSound ("Music");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex-1);
	}
	public void Retry() {
		audioManager.PlaySound ("ButtonClick");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
