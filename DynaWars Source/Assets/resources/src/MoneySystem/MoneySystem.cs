using UnityEngine;
using System.Collections;

public class MoneySystem : MonoBehaviour {
		
	public enum OperationType {
		ADD, SUBSTRACT, MULTIPLY, DIVIDE
	}
		
	public void ChangeMoney(ulong qnt, int opType = 0) {
		int op = (int)System.Enum.GetValues(typeof(MoneySystem.OperationType)).GetValue(opType);
		if(op == 0) {
			StartCoroutine(FadeMoney(qnt, 1));
			StartCoroutine(FadeColor(new Color(0, 0.85f, 0.85f), 1));
		} else if(op == 1) {
			StartCoroutine(FadeMoney(0-qnt, 1));
			StartCoroutine(FadeColor(new Color(0.85f, 0, 0), 1));
		} else if(op == 2) {
			PlayerStats.Money *= (ulong)qnt;
		} else if(op == 3) {
			PlayerStats.Money /= (ulong)qnt;
		}
	}
		
	public static long tempMoney;
	public static float pickedTime;
	public static float t;
		
	public IEnumerator FadeColor(Color endColor, float duration, Color? startColor = null) {

		float t = 0;

		if(startColor == null) {
			startColor = GUIStyles.MoneyColor;
		}

		Color tempColor = (Color)startColor;
				
		while(t < duration*2) {
					
			yield return null;
					
			if(t < duration) {
				GUIStyles.MoneyColor = Color.Lerp(tempColor, endColor, t/duration);
			} else if(t > duration && t < duration*2) {
				GUIStyles.MoneyColor = Color.Lerp(endColor, tempColor, t/duration/2);
			}
					
			t += Time.deltaTime;
					
		}

		GUIStyles.MoneyColor = new Color(0, 0.85f, 0);
				
		//GUIStyles.MoneyStyle.normal.textColor = Color.Lerp(Color.white, Color.red, 3);
	}

	public IEnumerator FadeMoney(ulong endMoney, float duration) {
		
		float t = 0;

		ulong tmpMoney = PlayerStats.Money;
		
		while(t < duration) {
			
			yield return null;

			PlayerStats.Money = (ulong)Mathf.Lerp(tmpMoney, endMoney+tmpMoney, t/duration);
			
			t += Time.deltaTime;
			
		}
	
	}

}
