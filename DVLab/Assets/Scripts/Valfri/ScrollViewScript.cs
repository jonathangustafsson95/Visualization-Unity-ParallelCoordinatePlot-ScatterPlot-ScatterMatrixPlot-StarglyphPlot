using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewScript : MonoBehaviour
{
    public RectTransform listItemPrefab;
    public RectTransform content;

    public ScrollRect scrollView;

    List<ItemContent> items = new List<ItemContent>();

    public void ClearList()
    {
        foreach (var item in items)
        {
            Destroy(item.root.gameObject);
        }
        items = new List<ItemContent>();
    }

    public void CreateListItem(Wedge wedge)
    {
        var item = Instantiate(listItemPrefab);
        item.transform.SetParent(content.transform, false);

        ItemContent newItemContent = new ItemContent(item.transform);

        newItemContent.ContentImage = Instantiate(wedge.Image);

        newItemContent.contentText.text = wedge.Value.ToString();
        newItemContent.headerText.text = wedge.Name;

        items.Add(newItemContent);
    }

    public class ItemContent
    {
        public RectTransform header;
        public RectTransform itemContent;
        public Text headerText;
        public Text contentText;
        public RectTransform imageContainer;

        private Image contentImage;
        public Image ContentImage 
        {
            get { return contentImage; }
            set
            {
                contentImage = value;
                contentImage.transform.SetParent(imageContainer);
                contentImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            }
        }

        public Transform root;
        public ItemContent(Transform root)
        {
            this.root = root;
            header = root.Find("Header").GetComponent<RectTransform>();
            headerText = header.Find("Text").GetComponent<Text>();
            itemContent = root.Find("Content").GetComponent<RectTransform>();
            imageContainer = itemContent.Find("ImageContainer").GetComponent<RectTransform>();
            contentText = itemContent.Find("Text").GetComponent<Text>();
            ContentImage = imageContainer.Find("Image").GetComponent<Image>();
        }
    }


}
