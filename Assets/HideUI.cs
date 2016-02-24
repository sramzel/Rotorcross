using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HideUI : MonoBehaviour {
	public bool Hidden {
		get {
			return hidden;
		}
	}

	bool hidden = false;
	public enum Direction {
		left,
		bottom,
		top
	}
	public RectTransform panel;
	public Direction direction;
	public RectTransform button;
	public RectTransform canvas;
	public Text buttonLabel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnHide(){
		switch (direction) {
		case Direction.bottom:
			if (hidden) {
				panel.anchoredPosition = new Vector2 (panel.anchoredPosition.x, -panel.rect.yMin);
				buttonLabel.text = "▼";
			} else {
				panel.anchoredPosition = new Vector2 (panel.anchoredPosition.x, panel.rect.yMin + button.rect.height);
				buttonLabel.text = "▲";
			}
			break;
		case Direction.left:
			if (hidden) {
				panel.anchoredPosition = new Vector2 (-panel.rect.xMin, panel.anchoredPosition.y);
				buttonLabel.text = "◀";
			} else {
				panel.anchoredPosition = new Vector2 (-panel.rect.xMax + button.rect.width, panel.anchoredPosition.y);
				buttonLabel.text = "▶";
			}
			break;
		case Direction.top:
			if (hidden) {
				panel.anchoredPosition = new Vector2 (panel.anchoredPosition.x, -panel.rect.yMax);
				buttonLabel.text = "▲";
			} else {
				panel.anchoredPosition = new Vector2 (panel.anchoredPosition.x, panel.rect.yMax - button.rect.height);
				buttonLabel.text = "▼";
			}
			break;
		}
		hidden = !hidden;
	}
}