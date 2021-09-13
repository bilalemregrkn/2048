using System.Collections;
using UnityEngine;


public class MoveAnimation : MonoBehaviour
{
	[SerializeField] private AnimationCurve moveCurve;
	[SerializeField] private float time;

	public IEnumerator Move(Transform current, Vector3 target, float time, AnimationCurve curve = null)
	{
		float passed = 0;
		var init = current.position;

		while (passed < time)
		{
			passed += Time.deltaTime;
			var normalized = passed / time;
			normalized = curve.Evaluate(normalized);
			current.position = Vector3.LerpUnclamped(init, target, normalized);
			yield return null;
		}
	}

	public IEnumerator Move(Transform current, Vector3 target)
	{
		float passed = 0;
		var init = current.position;
		var time = this.time;
		var curve = moveCurve;

		while (passed < time)
		{
			passed += Time.deltaTime;
			var normalized = passed / time;
			normalized = curve.Evaluate(normalized);
			current.position = Vector3.LerpUnclamped(init, target, normalized);
			yield return null;
		}
	}
	
}