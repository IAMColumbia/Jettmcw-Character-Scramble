using UnityEngine;

public class PlayerAnchors : MonoBehaviour
{
	[Header("Selection Area")]
	public Transform LeftChoice;
	public Transform Selection, RightChoice, Preview;

	[Header("Finished Area")]
	public Transform[] Finished;

	[Header("Animation Points")]
	public Transform LeftUpwards;
	public Transform RightUpwards, LeftDownwards, RightDownwards;
}
