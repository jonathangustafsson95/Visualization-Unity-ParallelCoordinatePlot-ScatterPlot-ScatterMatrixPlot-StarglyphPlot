using Accord.Math.Optimization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wedge : WedgeCirkle
{
	#region Properties
	public double Value { get; set; }
	public double NormalizedValue { get; set; }
	public string Name { get; set; }
	#endregion

	#region Constructors
	public Wedge(Image image) : base(image)
	{
		Image.fillAmount = (float)1 / (float)Dataset.NumberOfFeatures;
	}
	public Wedge(Image image, double value, double normalizedValue, double kpcaValue) :base(image)
	{
		Value = value;
		NormalizedValue = normalizedValue;
		//KpcaValue = kpcaValue;

		Size = (float)normalizedValue;
	}
	#endregion
}

public class WedgeCirkle
{
	#region Properties
	public Image Image { get; set; }
	public int SizeMultiplier { get; set; } = 30;
	protected Vector2 sizeVector { get; set; }
	public float Size 
	{
		get { return Image.rectTransform.sizeDelta.x / SizeMultiplier; }
		set
		{
			float sizeMultiplied = value * SizeMultiplier;

			if (Image.rectTransform.sizeDelta.x != sizeMultiplied)
			{
				sizeVector = new Vector2(sizeMultiplied, sizeMultiplied);
				Image.rectTransform.sizeDelta = sizeVector;
			}
		} 
	}
	public float Rotation 
	{
		get { return Image.transform.rotation.z; }
		set
		{
			if(value != Image.transform.rotation.z)
			{
				Image.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, value));
			}
		}
	}
	#endregion

	#region Constructors
	public WedgeCirkle(Image image)
	{
		Image = image;
		Init();
		SetPosition();
	}
	#endregion

	#region Methods
	private void Init()
	{
		Image.color = Color.white;
		Image.rectTransform.sizeDelta = new Vector2(30f, 30f);
		Image.fillAmount = 1f;
	}
	private void SetPosition(float x = 0, float y = 0, float z = 0)
	{
		Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, z);
	}
	public void SetParant(GameObject parent)
	{
		Image.transform.SetParent(parent.transform);
		Image.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
	}
	public void SetColor(Color color)
	{
		Image.color = color;
	}
	#endregion
}

public class WedgeEventLayer : WedgeCirkle
{
	#region Properties
	new public float Size
	{
		get { return Image.rectTransform.sizeDelta.x; }
		set
		{
			if (Image.rectTransform.sizeDelta.x != value)
			{
				sizeVector = new Vector2(value, value);
				Image.rectTransform.sizeDelta = sizeVector;
			}
		}
	}
	#endregion

	#region Constructors
	public WedgeEventLayer(Image image) : base(image)
	{
		Image.color = new Color(1.0f, 1.0f, 1.0f, 0f);
	}
	#endregion

	#region Methods
	public void AddEvent(EventTrigger trigger, EventTriggerType triggerType, System.Action<BaseEventData> callback)
	{
		EventTrigger.Entry triggerEnter = new EventTrigger.Entry();
		triggerEnter.eventID = triggerType;
		triggerEnter.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
		trigger.triggers.Add(triggerEnter);
	}
	#endregion
}