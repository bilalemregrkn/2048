using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberController : MonoBehaviour
{
	public int Index
	{
		get => _index;
		set
		{
			_index = value;

			//Text 
			int number = (int)Mathf.Pow(2, (_index + 1));
			textNumber.text = number.ToString();

			//Color
			var color = listColor[_index];
			spriteRenderer.color = color;
		}
	}

	private int _index;

	[SerializeField] private TextMeshPro textNumber;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private List<Color> listColor;
}