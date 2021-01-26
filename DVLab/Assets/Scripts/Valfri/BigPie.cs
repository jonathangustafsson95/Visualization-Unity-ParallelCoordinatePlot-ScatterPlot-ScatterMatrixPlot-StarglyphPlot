using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BigPie : MonoBehaviour
{

	public Image wedge;
	private Vector2 startSize;
	public List<Image> wedges;
	private List<Vector2> wedgeStartSizes = new List<Vector2>();

	public List<WedgeCirkle> WedgeObjects = new List<WedgeCirkle>();
	private List<float> StartSizes = new List<float>();

	public void CreateBigPie(BaseEventData eventData)
	{
		foreach (var wedge in WedgeObjects)
		{
			float StartSize = wedge.Size;
			StartSizes.Add(StartSize);
			wedge.Size = StartSize * 5;
		}
	}
	public void CreateBigPie()
	{
		foreach (var wedge in WedgeObjects)
		{
			float StartSize = wedge.Size;
			StartSizes.Add(StartSize);
			wedge.Size = StartSize * 5;
		}
	}
	public void SmallPie(BaseEventData eventData)
	{
		foreach (var wedge in WedgeObjects)
		{

			var size = StartSizes[0];
			wedge.Size = size;
			StartSizes.Remove(size);
			
		}
	}

	public void ShowClickedPie(BaseEventData eventData)
	{
		var menu = GameObject.Find("PieStats");
		var canvas = GameObject.Find("Canvas");
		var statsMenuScript = canvas.GetComponent<PieStatsMenu>();

		statsMenuScript.OpenMenu();
		statsMenuScript.ShowPie(gameObject);
	}

}
