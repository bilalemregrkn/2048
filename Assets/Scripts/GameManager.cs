using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyGrid;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; set; }
	
	[SerializeField] private NumberController prefabs;
	[SerializeField] private ScaleAnimation scaleAnimation;
	[SerializeField] private float moveAnimTime;

	private bool _isYourTurn = true;

	[ContextMenu(nameof(Spawn))]
	private IEnumerator Spawn()
	{
		var number = Instantiate(prefabs);
		number.Index = Random.value < .6 ? 0 : 1;

		var tile = GridManager.Instance.GetRandomEmptyTile();
		if (!tile) yield break;

		number.transform.position = tile.transform.position;
		tile.MyNumberController = number;

		number.transform.localScale = Vector3.zero;
		yield return scaleAnimation.Scale(number.transform, Vector3.one);
	}

	private bool IsPressUp => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
	private bool IsPressLeft => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
	private bool IsPressRight => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
	private bool IsPressDown => Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

	public List<NumberController> willUp;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		StartCoroutine(Spawn());
	}

	private void Update()
	{
		if (IsPressUp)
			Move(Direction.Up);

		if (IsPressLeft)
			Move(Direction.Left);

		if (IsPressRight)
			Move(Direction.Right);

		if (IsPressDown)
			Move(Direction.Down);

		if (Input.GetKeyDown(KeyCode.Space))
			StartCoroutine(Spawn());
	}

	private void Move(Direction direction)
	{
		if (!_isYourTurn)
			return;

		_isYourTurn = false;
		var action = MoveAction(direction);
		StartCoroutine(action);
	}

	private IEnumerator MoveAction(Direction direction)
	{
		var list = GridManager.Instance.GetListTileFromDirection(direction);

		willUp.Clear();
		foreach (var tile in list)
			yield return tile.Move(direction);
		// StartCoroutine(tile.Move(direction));

		// yield return new WaitForSeconds(moveAnimTime * 4);
		foreach (var item in willUp)
			item.Index++;

		yield return Spawn();

		_isYourTurn = true;
	}
}