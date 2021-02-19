﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MultiplayerClient.Canvas
{ 
    public class CanvasButton
    {
        private GameObject buttonObj;
        private GameObject textObj;
        private Button button;
        private UnityAction<string> clicked;
        private string buttonName;
        private CanvasPanel _canvas;

        public bool active;

        public CanvasButton(GameObject parent, string name, Texture2D tex, Vector2 pos, Vector2 size, Rect bgSubSection, CanvasPanel panel, Font font = null, string text = null, int fontSize = 13)
        {
            if (size.x == 0 || size.y == 0)
            {
                size = new Vector2(bgSubSection.width, bgSubSection.height);
            }

            _canvas = panel;

            buttonName = name;

            buttonObj = new GameObject("Canvas Button - " + name);
            buttonObj.AddComponent<CanvasRenderer>();
            RectTransform buttonTransform = buttonObj.AddComponent<RectTransform>();
            buttonTransform.sizeDelta = new Vector2(bgSubSection.width, bgSubSection.height);
            buttonObj.AddComponent<Image>().sprite = Sprite.Create(tex, new Rect(bgSubSection.x, tex.height - bgSubSection.height, bgSubSection.width, bgSubSection.height), Vector2.zero);
            button = buttonObj.AddComponent<Button>();

            buttonObj.transform.SetParent(parent.transform, false);

            buttonTransform.SetScaleX(size.x / bgSubSection.width);
            buttonTransform.SetScaleY(1.0f);//size.y / bgSubSection.height);

            Vector2 position = new Vector2((pos.x + ((size.x / bgSubSection.width) * bgSubSection.width) / 2f) / Screen.width, (Screen.height - (pos.y + ((size.y / bgSubSection.height) * bgSubSection.height) / 2f)) / Screen.height);
            buttonTransform.anchorMin = position;
            buttonTransform.anchorMax = position;

            Object.DontDestroyOnLoad(buttonObj);

            if (font != null && text != null)
            {
                textObj = new GameObject("Canvas Button Text - " + name);
                textObj.AddComponent<RectTransform>().sizeDelta = new Vector2(bgSubSection.width, bgSubSection.height);
                Text t = textObj.AddComponent<Text>();
                t.text = text;
                t.font = font;
                t.fontSize = fontSize;
                t.alignment = TextAnchor.MiddleCenter;
                Outline outline = textObj.AddComponent<Outline>();
                outline.effectColor = Color.black;
                textObj.transform.SetParent(buttonObj.transform, false);

                Object.DontDestroyOnLoad(textObj);
            }

            active = true;
        }

        public void UpdateSprite(Texture2D tex, Rect bgSubSection)
        {
            if (buttonObj != null)
            {
                buttonObj.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(bgSubSection.x, tex.height - bgSubSection.height, bgSubSection.width, bgSubSection.height), Vector2.zero);
            }
        }

        public void AddClickEvent(UnityAction<string> action)
        {
            if (buttonObj != null)
            {
                clicked = action;
                buttonObj.GetComponent<Button>().onClick.AddListener(ButtonClicked);
            }
        }

        private void ButtonClicked()
        {
            if (clicked != null && buttonName != null && active) clicked(buttonName);
        }

        public void UpdateText(string text)
        {
            if (textObj != null)
            {
                textObj.GetComponent<Text>().text = text;
            }
        }

        public void SetWidth(float width)
        {
            if (buttonObj != null)
            {
                buttonObj.GetComponent<RectTransform>().SetScaleX(width / buttonObj.GetComponent<RectTransform>().sizeDelta.x);
            }
        }

        public void SetHeight(float height)
        {
            if (buttonObj != null)
            {
                buttonObj.GetComponent<RectTransform>().SetScaleY(height / buttonObj.GetComponent<RectTransform>().sizeDelta.y);
            }
        }

        public void SetPosition(Vector2 pos)
        {
            if (buttonObj != null)
            {
                Vector2 sz = buttonObj.GetComponent<RectTransform>().sizeDelta;
                Vector2 position = new Vector2((pos.x + sz.x / 2f) / Screen.width, (Screen.height - (pos.y + sz.y / 2f)) / Screen.height);
                buttonObj.GetComponent<RectTransform>().anchorMin = position;
                buttonObj.GetComponent<RectTransform>().anchorMax = position;
            }
        }

        public void SetActive(bool b)
        {
            if (buttonObj != null)
            {
                buttonObj.SetActive(b);
                active = b;
                SetTextColor(b ? Color.white : Color.grey);
            }
        }

        public void Remove()
        {
            _canvas.RemoveButton(buttonName);
            if (buttonObj != null)
            {
                Object.Destroy(buttonObj);
            }
            if (textObj != null)
            {
                Object.Destroy(textObj);
            }
        }

        public void SetRenderIndex(int idx)
        {
            buttonObj.transform.SetSiblingIndex(idx);
        }

        public Vector2 GetPosition()
        {
            if (buttonObj != null)
            {
                Vector2 anchor = buttonObj.GetComponent<RectTransform>().anchorMin;
                Vector2 sz = buttonObj.GetComponent<RectTransform>().sizeDelta;

                return new Vector2(anchor.x * Screen.width - sz.x / 2f, Screen.height - anchor.y * Screen.height - sz.y / 2f);
            }

            return Vector2.zero;
        }

        public void MoveToTop()
        {
            if (buttonObj != null)
            {
                buttonObj.transform.SetAsLastSibling();
            }
        }

        public void SetTextColor(Color color)
        {
            if (textObj != null)
            {
                Text t = textObj.GetComponent<Text>();
                t.color = color;
            }
        }

        public string GetText()
        {
            if (textObj != null)
            {
                return textObj.GetComponent<Text>().text;
            }

            return null;
        }

        public void Destroy()
        {
            Object.Destroy(buttonObj);
            Object.Destroy(textObj);
        }
    }
}