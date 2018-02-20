using UnityEngine;
using System.Collections;
using Dev.Krk.MemoryFlow.Game;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Showable))]
public class Finishable : MonoBehaviour
{
	public delegate void FinishedAction();
	public event FinishedAction OnFinished;

	[SerializeField]
	private float finishDuration = 0.5f;

	[SerializeField]
	private Vector3 maxFinishScale;

	private CircleCollider2D circleCollider;

	private Showable showable;

	void Awake()
	{
		circleCollider = GetComponent<CircleCollider2D>();
		showable = GetComponent<Showable> ();
	}

	public void Init(Vector2 position, float showInterval, float hideInterval)
	{
		transform.position = position;
		transform.localScale = Vector3.one;
		circleCollider.enabled = true;

		showable.ShowDelay = (position.x / Field.SIZE + position.y / Field.SIZE) * showInterval;
		showable.HideDelay = (position.x / Field.SIZE + position.y / Field.SIZE) * hideInterval;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		Finish();
	}

	public void Finish()
	{
        OnFinished();
    }

	public void Show()
	{
		showable.Show ();
	}

	public void Hide()
	{
		showable.Hide ();
	}
}
