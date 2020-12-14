///-----------------------------------------------------------------
/// Author : #DEVELOPER_NAME#
/// Date : #DATE#
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {
	[Header("Text")]
	[SerializeField] protected Text leftText = default;
	[SerializeField] protected Text rightText = default;
	[SerializeField] protected Text drawText = default;
	[Space]
	[SerializeField] protected string winString = default;
	[SerializeField] protected string loseString = default;
	[SerializeField] protected string drawString = default;
	[Space]
	[SerializeField] protected Color colorLose = default;
	[SerializeField] protected Color colorWin = default;
	[SerializeField] protected Color colorDraw = default;

	[Header("Reset")]
	[SerializeField] protected Button resetButton = default;

	public enum StateWin {
		LEFT_WIN,
		RIGHT_WIN,
		DRAW
	}

	protected void Start() {
		drawText.gameObject.SetActive(false);
		leftText.gameObject.SetActive(true);
		rightText.gameObject.SetActive(true);

		resetButton.onClick.AddListener(ResetButton_OnClick);
	}

	protected void ResetButton_OnClick() {
		SceneManager.LoadSceneAsync(0);
		resetButton.onClick.RemoveListener(ResetButton_OnClick);
	}

	public void Init(StateWin whoIsWinner) {
		gameObject.SetActive(true);

		if (whoIsWinner == StateWin.LEFT_WIN) {
			SetText(leftText, rightText);
		}
		else if (whoIsWinner == StateWin.RIGHT_WIN) {
			SetText(rightText, leftText);
		}
		else {
			leftText.gameObject.SetActive(false);
			rightText.gameObject.SetActive(false);
			drawText.gameObject.SetActive(true);

			drawText.text = drawString;
			drawText.color = colorDraw; 
		}
	}

	protected void SetText(Text winText, Text loseText) {
		winText.text = winString;
		loseText.text = loseString;

		winText.color = colorWin;
		loseText.color = colorLose;
	}
}