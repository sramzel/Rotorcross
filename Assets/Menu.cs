using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	
	public Image image;
	int level;
	public string[] scenes;
	public Sprite[] screenShots;
    public string[] titles;
    public Text title;

	// Use this for initialization
	void Start () {
		image.sprite = screenShots [level];
        title.text = titles[level];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Exit(){
		Application.Quit ();
	}

	public void Load (){
        gameObject.SetActive(false);
		SceneManager.LoadScene (scenes [level]);
	}

	public void Next(){
		level++;
		level %= scenes.Length;
		image.sprite = screenShots [level];
        title.text = titles[level];
	}

	public void Prev(){
		level--;
		if (level < 0) level += scenes.Length ;
		image.sprite = screenShots [level];
        title.text = titles[level];
    }
}
