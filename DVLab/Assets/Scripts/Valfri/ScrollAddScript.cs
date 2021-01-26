using Accord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollAddScript : MonoBehaviour
{
    public RectTransform listItemPrefab;
    public RectTransform content;
    public ScrollRect scrollView;

    List<ItemContent> items = new List<ItemContent>();

    void Start()
    {
        CreateAllListItems();
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

    public void CreateListItem(string title = "Title")
    {
        if (title.ToLower().Equals("id") || title.ToLower().Equals("") || title.ToLower().Replace("\"", "").Equals("id"))
        {
            return;
        }

        var item = Instantiate(listItemPrefab);
        item.transform.SetParent(content.transform, false);

        ItemContent newItemContent = new ItemContent(item.transform);
        newItemContent.headerText.text = title;

        items.Add(newItemContent);
    }

    private void CreateAllListItems()
    {
        foreach (var item in Dataset.ListOfPoints[0].Keys)
        {
            CreateListItem(item);
        }
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
}
