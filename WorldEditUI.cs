using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis;
using Terraria.ModLoader;

namespace WorldEdit;

public class WorldEditUI : UIState
{
    public UITextPanel<string> CopyButton;
    public UITextPanel<string> PasteButton;
    public UITextPanel<string> SetButton;
    public DraggableUIPanel frame;

    public float Margin = 10f;
    public float ButtonHeight = 50f;

    public override void OnInitialize()
    {
        float compountHeight = 0;
        float vMargin = Margin;
        CopyButton = new UITextPanel<string>("Copy ", 0.8f, true) { Left = new(Margin, 0f), Top = new(vMargin, 0f), Width = new(150f, 0f), Height = new(ButtonHeight, 0f) };
        CopyButton.OnLeftClick += OnCopyButtonClick;

        compountHeight += ButtonHeight;
        vMargin += Margin;
        PasteButton = new UITextPanel<string>("Paste ", 0.8f, true) { Left = new(Margin, 0f), Top = new(vMargin + compountHeight, 0f), Width = new(150f, 0f), Height = new(ButtonHeight, 0f) };
        PasteButton.OnLeftClick += OnPasteButtonClick;

        compountHeight += ButtonHeight;
        vMargin += Margin;
        SetButton = new UITextPanel<string>("Set ", 0.8f, true) { Left = new(Margin, 0f), Top = new(vMargin + compountHeight, 0f), Width = new(150f, 0f), Height = new(ButtonHeight, 0f) };
        SetButton.OnLeftClick += OnSetButtonClick;

        compountHeight += ButtonHeight;
        frame = new DraggableUIPanel() { Left = new(300f, 0f), Top = new(200f, 0f), Width = new(150f + Margin * 4, 0f), Height = new(compountHeight + vMargin + (Margin * 4), 0f), BackgroundColor = new Color(73, 94, 171) };
        
        frame.Append(CopyButton);
        frame.Append(PasteButton);
        frame.Append(SetButton);
        Append(frame);
    }

    private void OnCopyButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
        WorldEdit mod = ModContent.GetInstance<WorldEdit>();
        mod.Copy();
        Main.NewText("Copy Button clicked!", Color.Green);
    }
    private void OnPasteButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
        WorldEdit mod = ModContent.GetInstance<WorldEdit>();
        mod.Paste();

        Main.NewText("Paste Button clicked!", Color.Green);
    }
    private void OnSetButtonClick(UIMouseEvent evt, UIElement listeningElement)
    {
        WorldEdit mod = ModContent.GetInstance<WorldEdit>();
        mod.Set();

        Main.NewText("Set Button clicked!", Color.Green);
    }

    public class SimpleUIElement : UIElement
    {
        public UIElement Element { get; set; }

        private Vector2 _offset = Vector2.Zero;
        public Vector2 Offset { get => _offset;
            set
            {
                _offset = value;
                Element.Left = new StyleDimension(_left.Pixels + _offset.X, _left.Percent);
                Element.Top = new StyleDimension(_top.Pixels + _offset.Y, _top.Percent);
            }
        }

        private StyleDimension _left = StyleDimension.Empty;
        public StyleDimension Left
        {
            get => Element.Left;
            set
            {
                _left = value;
                Element.Left = new StyleDimension(_left.Pixels + _offset.X, _left.Percent);
            }
        }

        private StyleDimension _top = StyleDimension.Empty;
        public StyleDimension Top
        {
            get => Element.Top;
            set
            {
                _top = value;
                Element.Top = new StyleDimension(_top.Pixels + _offset.Y, _top.Percent);
            }
        }

        public StyleDimension Width { get => Element.Width; set => Element.Width = value; }
        public StyleDimension Height { get => Element.Height; set => Element.Height = value; }

        public SimpleUIElement() { }
        public SimpleUIElement(UIElement element)
        {
            Element = element;
        }
        public SimpleUIElement(UIElement element, StyleDimension width, StyleDimension height) : this(element)
        {
            Element = element;
            Width = width;
            Height = height;
        }
    }

    public class DraggableUIPanel : UIPanel
    {
        private Vector2 offset;
        private bool dragging;

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            // Only handle the click if it wasn’t on a child element
            foreach (var child in Elements)
            {
                if (child.ContainsPoint(evt.MousePosition))
                    return; // A child element already handled this
            }

            base.LeftMouseDown(evt);
            offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
            dragging = true;
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            dragging = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f);
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate(); // Applies updated position
            }
        }
    }
}
