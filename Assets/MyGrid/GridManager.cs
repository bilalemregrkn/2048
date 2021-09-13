using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyGrid
{
	public enum AxisType
	{
		XY,
		XZ
	}

	public class GridManager : MonoBehaviour
	{
		public static GridManager Instance { get; private set; }

		[SerializeField] private TileController tilePrefabs;
		[SerializeField] private Vector2Int size = new Vector2Int(3, 3);
		[SerializeField, Range(0, 5f)] private float distance = 1.25f;
		[SerializeField] private bool changeDistance;
		[SerializeField] private AxisType axisType;
		public List<TileController> ListGridController => listGridController;
		[SerializeField] private List<TileController> listGridController;

		private void Awake()
		{
			Instance = this;
		}

		[ContextMenu("Create Grid")]
		public void CreateGrid()
		{
			listGridController.Clear();
			Vector3 gridPosition = Vector3.zero;
			GameObject parent = new GameObject();
			parent.transform.name = "Grid";
			for (int i = 0; i < size.y; i++)
			{
				switch (axisType)
				{
					case AxisType.XY:
						gridPosition.y = i * distance;
						break;
					case AxisType.XZ:
						gridPosition.z = i * distance;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}


				for (int j = 0; j < size.x; j++)
				{
					gridPosition.x = j * distance;
					TileController tile = Instantiate(tilePrefabs, gridPosition, Quaternion.identity);
					tile.transform.name = $"Tile [{j},{i}]";
					tile.coordinate = new Vector2(j, i);
					tile.transform.SetParent(parent.transform);
					listGridController.Add(tile);
				}
			}

			SetNeighbor();

			var pivotHelper = parent.AddComponent<SetCenterPosition>();
			pivotHelper.SetCenter();
			parent.transform.position = Vector3.zero;
		}

		private void OnValidate()
		{
			if (changeDistance)
				SetDistance(distance);
		}

		private void SetDistance(float newDistance)
		{
			if (listGridController != null)
			{
				if (listGridController.Count != 0)
				{
					Vector3 gridPosition = Vector3.zero;
					var betweenDistance = newDistance;
					for (int i = 0; i < size.y; i++)
					{
						switch (axisType)
						{
							case AxisType.XY:
								gridPosition.y = i * betweenDistance;
								break;
							case AxisType.XZ:
								gridPosition.z = i * betweenDistance;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}


						for (int j = 0; j < size.x; j++)
						{
							gridPosition.x = j * betweenDistance;
							listGridController[(int)((i * size.y) + j)].transform.position = gridPosition;
						}
					}
				}
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Create Grid as Prefabs")]
		public void CreateGridAsPrefabs()
		{
			GameObject prefabs = Selection.activeObject as GameObject;
			if (prefabs == null) return;
			TileController tileController = prefabs.GetComponent<TileController>();
			if (tileController == null)
			{
				Debug.LogError("Prefabs must contains component TileController");
				return;
			}

			listGridController.Clear();
			Vector3 gridPosition = Vector3.zero;
			GameObject parent = new GameObject();
			parent.transform.name = "Grid";

			for (int i = 0; i < size.y; i++)
			{
				switch (axisType)
				{
					case AxisType.XY:
						gridPosition.y = i * distance;
						break;
					case AxisType.XZ:
						gridPosition.z = i * distance;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				for (int j = 0; j < size.x; j++)
				{
					gridPosition.x = j * distance;

					GameObject tileGameObject = PrefabUtility.InstantiatePrefab(prefabs) as GameObject;
					if (tileGameObject == null)
					{
						Debug.LogError("Selection is Null");
						return;
					}

					TileController tile = tileGameObject.GetComponent<TileController>();
					if (tile == null)
					{
						Debug.LogError("Selection Prefabs must contains component TileController");
						return;
					}

					var tileTransform = tile.transform;
					tileTransform.name = $"Tile [{j},{i}]";
					tileTransform.position = gridPosition;
					tile.coordinate = new Vector2(j, i);
					tile.transform.SetParent(parent.transform);
					listGridController.Add(tile);
				}
			}

			SetNeighbor();

			var pivotHelper = parent.AddComponent<SetCenterPosition>();
			pivotHelper.SetCenter();
			parent.transform.position = Vector3.zero;
		}
#endif

		public TileController GetTile(Vector2 coordinate)
		{
			foreach (var item in listGridController)
			{
				if (item.coordinate == coordinate)
					return item;
			}

			return null;
		}

		public TileController GetRandomEmptyTile()
		{
			var listEmpty = new List<TileController>();
			foreach (var tile in listGridController)
			{
				if (!tile.MyNumberController)
					listEmpty.Add(tile);
			}

			if (listEmpty.Count == 0)
				return null;

			var index = Random.Range(0, listEmpty.Count);
			return listEmpty[index];
		}

		public List<TileController> GetListTileFromDirection(Direction direction)
		{
			var result = new List<TileController>();

			bool isVertical = direction == Direction.Up || direction == Direction.Down;
			bool isAdd = direction == Direction.Left || direction == Direction.Down;

			int currentIndex = 0;
			if (!isAdd) currentIndex = isVertical ? size.y : size.x;
			else currentIndex = 0;

			int amount = isVertical ? size.y : size.x;

			for (int i = 0; i <= amount; i++)
			{
				foreach (var tile in listGridController)
				{
					var current = isVertical ? tile.coordinate.y : tile.coordinate.x;
					if (current == currentIndex)
						result.Add(tile);
				}

				currentIndex += isAdd ? 1 : -1;
			}
			
			return result;
		}

		private void SetNeighbor()
		{
			foreach (var grid in listGridController)
			{
				Vector2 gridCoordinate = grid.coordinate;

				Vector2 gridUpCoordinate = gridCoordinate;
				Vector2 gridDownCoordinate = gridCoordinate;
				Vector2 gridRightCoordinate = gridCoordinate;
				Vector2 gridLeftCoordinate = gridCoordinate;

				gridUpCoordinate.y++;
				gridDownCoordinate.y--;
				gridRightCoordinate.x++;
				gridLeftCoordinate.x--;

				grid.up = GetTile(gridUpCoordinate);
				grid.down = GetTile(gridDownCoordinate);
				grid.right = GetTile(gridRightCoordinate);
				grid.left = GetTile(gridLeftCoordinate);
			}
		}
	}
}