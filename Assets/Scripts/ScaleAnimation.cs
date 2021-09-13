using System.Collections;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
	[SerializeField] private AnimationCurve curve;
	[SerializeField] private float time;

	// public IEnumerator Move(Transform current, Vector3 target, float time, AnimationCurve curve = null)
	// {
	// 	float passed = 0;
	// 	var init = current.position;
	//
	// 	while (passed < time)
	// 	{
	// 		passed += Time.deltaTime;
	// 		var normalized = passed / time;
	// 		normalized = curve.Evaluate(normalized);
	// 		current.position = Vector3.LerpUnclamped(init, target, normalized);
	// 		yield return null;
	// 	}
	// }
	

	public IEnumerator Scale(Transform current, Vector3 target)
	{
		float passed = 0;
		var init = current.localScale;
		var time = this.time;
		var curve = this.curve;

		while (passed < time)
		{
			passed += Time.deltaTime;
			var normalized = passed / time;
			normalized = curve.Evaluate(normalized);
			current.localScale = Vector3.LerpUnclamped(init, target, normalized);
			yield return null;
		}
	}
}