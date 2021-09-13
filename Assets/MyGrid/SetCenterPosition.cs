using System.Linq;
using UnityEngine;

namespace MyGrid
{
	public class SetCenterPosition : MonoBehaviour
	{
		[ContextMenu(nameof(SetCenter))]
		public void SetCenter()
		{
			var children = GetComponentsInChildren<Transform>();
			children.ToList().RemoveAt(0);
			Vector3 totalPos = Vector3.zero;
			foreach (var item in children)
				totalPos += item.position;

			totalPos /= children.Length;

			var parent = transform.parent;
			foreach (var item in children)
				item.SetParent(parent);

			transform.position = totalPos;
			foreach (var item in children)
				item.SetParent(transform);
		}
	}
}