using Accord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollEditScript : MonoBehaviour
{
    public RectTransform listItemPrefab;
    public RectTransform content;
    public ScrollRect scrollView;

    List<ItemContent> items = new List<ItemContent>();

    public void Show(GameObject pie)
    {
        ClearList();

        var pieScript = pie.GetComponent<BigPie>();
        List<WedgeCirkle> wedgeCirkles = pieScript.WedgeObjects;

        foreach (var wedge in wedgeCirkles)
        {
            if (wedge is Wedge)
                CreateListItem(((Wedge)wedge).Name, ((Wedge)wedge).Value.ToString());
        }
    }

    public void ClearList()
    {
        foreach (var item in items)
        {
            Destroy(item.root.gameObject);
        }
        items = new List<ItemContent>();
    }

    public void CreateListItem(string title = "Title", string value = "Enter value")
    {
        if(title != null)
        {
            if (title.ToLower().Equals("id"))
                return;
        }
  

        var item = Instantiate(listItemPrefab);
        item.transform.SetParent(content.transform, false);

        ItemContent newItemContent = new ItemContent(item.transform);
        newItemContent.headerText.text = title;
        newItemContent.inputField.text = value;

        items.Add(newItemContent);
    }

    public List<double> GetInputs()
    {
        List<double> point = new List<double>();
        foreach (var item in items)
        {
            if (double.TryParse(item.inputField.text, out double value))
                point.Add(value);
            else
                point.Add(-1);
        }
        return point;
    }

    public class ItemContent
    {
        public Text headerText;
        public InputField inputField;
        public Transform root;

        public ItemContent(Transform root)
        {
            this.root = root;

            headerText = root.Find("Text").GetComponent<Text>();
            inputField = root.Find("Input").GetComponent<InputField>();
        }
    }

}
