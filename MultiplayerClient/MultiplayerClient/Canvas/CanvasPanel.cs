﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MultiplayerClient.Canvas
{
    public class CanvasPanel
    {
        private CanvasImage background;
        private GameObject canvas;
        private Vector2 position;
        private Vector2 size;
        private Dictionary<string, CanvasButton> buttons = new Dictionary<string, CanvasButton>();
        private Dictionary<string, CanvasPanel> panels = new Dictionary<string, CanvasPanel>();
        private Dictionary<string, CanvasImage> images = new Dictionary<string, CanvasImage>();
        private Dictionary<string, CanvasText> texts = new Dictionary<string, CanvasText>();
        private Dictionary<string, CanvasInput> inputs = new Dictionary<string, CanvasInput>();
        private Dictionary<string, CanvasToggle> toggles = new Dictionary<string, CanvasToggle>();

        public bool active = true;

        public CanvasPanel(GameObject parent, Texture2D tex, Vector2 pos, Vector2 sz, Rect bgSubSection)
        {
            if (parent == null) return;

            if (sz.x == 0 || sz.y == 0)
            {
                size = new Vector2(bgSubSection.width, bgSubSection.height);
            }
            else
            {
                size = sz;
            }

            position = pos;
            canvas = parent;
            background = new CanvasImage(parent, tex, pos, sz, bgSubSection);

            active = false;
        }

        public CanvasButton AddButton(string name, Texture2D tex, Vector2 pos, Vector2 sz, UnityAction<string> func, Rect bgSubSection, Font font = null, string text = null, int fontSize = 13)
        {
            CanvasButton button = new CanvasButton(canvas, name, tex, position + pos, sz, bgSubSection, this, font, text, fontSize);
            button.AddClickEvent(func);

            buttons.Add(name, button);

            return button;
        }

        public bool RemoveButton(string name)
        {
            bool removed = false;
            if (buttons.ContainsKey(name))
            {
                buttons.Remove(name);
                removed = true;
            }
            return removed;
        }

        public CanvasPanel AddPanel(string name, Texture2D tex, Vector2 pos, Vector2 sz, Rect bgSubSection)
        {
            CanvasPanel panel = new CanvasPanel(canvas, tex, position + pos, sz, bgSubSection);

            panels.Add(name, panel);

            return panel;
        }

        public CanvasImage AddImage(string name, Texture2D tex, Vector2 pos, Vector2 sz, Rect subSprite)
        {
            CanvasImage image = new CanvasImage(canvas, tex, position + pos, sz, subSprite);

            images.Add(name, image);

            return image;
        }

        public CanvasText AddText(string name, string text, Vector2 pos, Vector2 sz, Font font, int fontSize = 13, FontStyle style = FontStyle.Normal, TextAnchor alignment = TextAnchor.UpperLeft)
        {
            CanvasText t = new CanvasText(canvas, position + pos, sz, font, text, fontSize, style, alignment);

            texts.Add(name, t);

            return t;
        }

        public CanvasInput AddInput(string name, Texture2D texture, Vector2 pos, Vector2 sz, Rect bgSubSection, Font font = null, string inputText = "", string placeholderText = "", int fontSize = 13)
        {
            CanvasInput input = new CanvasInput(canvas, name, texture, position + pos, sz, bgSubSection, font, inputText, placeholderText, fontSize);

            inputs.Add(name, input);

            return input;
        }

        public CanvasToggle AddToggle(string name, Texture2D bgTexture, Texture2D checkTexture, Vector2 pos, Vector2 sz, Vector2 bgPos, Rect labelRect, UnityAction<bool> toggleAction, Font font = null, string labelText = "", int fontSize = 13)
        {
            CanvasToggle toggle = new CanvasToggle(canvas, name, bgTexture, checkTexture, position + pos, sz, bgPos, labelRect, font, labelText, fontSize);
            toggle.AddToggleEvent(toggleAction);

            toggles.Add(name, toggle);

            return toggle;
        }

        public CanvasButton GetButton(string buttonName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetButton(buttonName);
            }

            if (buttons.ContainsKey(buttonName))
            {
                return buttons[buttonName];
            }

            return null;
        }

        public CanvasImage GetImage(string imageName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetImage(imageName);
            }

            if (images.ContainsKey(imageName))
            {
                return images[imageName];
            }

            return null;
        }

        public CanvasPanel GetPanel(string panelName)
        {
            if (panels.ContainsKey(panelName))
            {
                return panels[panelName];
            }

            return null;
        }

        public CanvasText GetText(string textName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetText(textName);
            }

            if (texts.ContainsKey(textName))
            {
                return texts[textName];
            }

            return null;
        }

        public CanvasInput GetInput(string inputName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetInput(inputName);
            }

            if (inputs.ContainsKey(inputName))
            {
                return inputs[inputName];
            }

            return null;
        }
        
        public CanvasToggle GetToggle(string toggleName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetToggle(toggleName);
            }

            if (toggles.ContainsKey(toggleName))
            {
                return toggles[toggleName];
            }

            return null;
        }

        public void UpdateBackground(Texture2D tex, Rect subSection)
        {
            background.UpdateImage(tex, subSection);
        }

        public void ResizeBG(Vector2 sz)
        {
            background.SetWidth(sz.x);
            background.SetHeight(sz.y);
            background.SetPosition(position);
        }

        public float GetHeight()
        {
            return background.GetHeight();
        }

        public void SetPosition(Vector2 pos)
        {
            background.SetPosition(pos);

            Vector2 deltaPos = position - pos;
            position = pos;

            foreach (CanvasButton button in buttons.Values)
            {
                button.SetPosition(button.GetPosition() - deltaPos);
            }

            foreach (CanvasInput input in inputs.Values)
            {
                input.SetPosition(input.GetPosition() - deltaPos);
            }
            
            foreach (CanvasText text in texts.Values)
            {
                text.SetPosition(text.GetPosition() - deltaPos);
            }
            
            foreach (CanvasToggle toggle in toggles.Values)
            {
                toggle.SetPosition(toggle.GetPosition() - deltaPos);
            }

            foreach (CanvasPanel panel in panels.Values)
            {
                panel.SetPosition(panel.GetPosition() - deltaPos);
            }
        }

        public void TogglePanel(string name)
        {
            if (active && panels.ContainsKey(name))
            {
                panels[name].ToggleActive();
            }
        }

        public void ToggleActive()
        {
            active = !active;
            SetActive(active, false);
        }

        public void SetActive(bool b, bool panel)
        {
            background.SetActive(b);

            foreach (CanvasButton button in buttons.Values)
            {
                button.SetActive(b);
            }

            foreach (CanvasImage image in images.Values)
            {
                image.SetActive(b);
            }

            foreach (CanvasInput input in inputs.Values)
            {
                input.SetActive(b);
            }
            
            foreach (CanvasText t in texts.Values)
            {
                t.SetActive(b);
            }
            
            foreach (CanvasToggle toggle in toggles.Values)
            {
                toggle.SetActive(b);
            }

            if (panel)
            {
                foreach (CanvasPanel p in panels.Values)
                {
                    p.SetActive(b, false);
                }
            }

            active = b;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void FixRenderOrder()
        {
            foreach (CanvasText t in texts.Values)
            {
                t.MoveToTop();
            }

            foreach (CanvasButton button in buttons.Values)
            {
                button.MoveToTop();
            }

            foreach (CanvasImage image in images.Values)
            {
                image.SetRenderIndex(0);
            }

            foreach (CanvasPanel panel in panels.Values)
            {
                panel.FixRenderOrder();
            }

            background.SetRenderIndex(0);
        }

        public void Destroy()
        {
            background.Destroy();

            foreach (CanvasButton button in buttons.Values)
            {
                button.Destroy();
            }

            foreach (CanvasImage image in images.Values)
            {
                image.Destroy();
            }

            foreach (CanvasInput input in inputs.Values)
            {
                input.Destroy();
            }

            foreach (CanvasText t in texts.Values)
            {
                t.Destroy();
            }
            
            foreach (CanvasToggle toggle in toggles.Values)
            {
                toggle.Destroy();
            }

            foreach (CanvasPanel p in panels.Values)
            {
                p.Destroy();
            }
        }
    }
}