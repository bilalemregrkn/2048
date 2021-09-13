using System.Collections;
using UnityEngine;

namespace MyGrid
{
	public class TileController : MonoBehaviour
	{
		[SerializeField] private MoveAnimation moveAnimation;
		public NumberController MyNumberController { get; set; }


		public TileController up;
		public TileController down;
		public TileController right;
		public TileController left;

		public Vector2 coordinate;

		public TileController GetNeighbour(Direction direction)
		{
			return direction switch
			{
				Direction.Up => up,
				Direction.Down => down,
				Direction.Left => left,
				Direction.Right => right,
				Direction.UpRight => up != null ? up.right : null,
				Direction.UpLeft => up != null ? up.left : null,
				Direction.DownRight => down != null ? down.right : null,
				Direction.DownLeft => down != null ? down.left : null,
				_ => null
			};
		}

		public IEnumerator Move(Direction direction)
		{
			if (!MyNumberController) yield break;

			var next = GetNeighbour(direction);
			if (!next) yield break;

			if (!next.MyNumberController)
			{
				
				yield return moveAnimation.Move(MyNumberController.transform, next.transform.position);

				next.MyNumberController = MyNumberController;
				// MyNumberController.transform.position = next.transform.position;
				MyNumberController = null;

				var move = next.Move(direction);
				yield return move;
			}
			else if (next.MyNumberController.Index == MyNumberController.Index)
			{
				//Merge
				Destroy(MyNumberController.gameObject);
				MyNumberController = null;

				// yield return new WaitForSeconds(.1f);
				// next.MyNumberController.Index++;
				GameManager.Instance.willUp.Add(next.MyNumberController);
			}
		}
	}
}