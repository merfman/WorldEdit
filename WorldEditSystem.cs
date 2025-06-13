using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace WorldEdit;
public class WorldEditSystem : ModSystem
{
    private UserInterface userInterface;
    public WorldEditUI myButtonState;

    public override void Load()
    {
        if (!Main.dedServ)
        {
            myButtonState = new WorldEditUI();
            userInterface = new UserInterface();
            userInterface.SetState(myButtonState);
        }
    }
    public override void UpdateUI(GameTime gameTime)
    {
        userInterface?.Update(gameTime);
    }

    //public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    //{
    //    int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
    //    if (inventoryIndex != -1)
    //    {
    //        // Insert your WorldEditUI *after* inventory to draw it on top
    //        layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
    //            "MyMod: WorldEditUI",
    //            delegate
    //            {
    //                if (userInterface?.CurrentState != null)
    //                {
    //                    userInterface.Draw(Main.spriteBatch, new GameTime());

    //                    // Block game input if hovering this UI
    //                    //if (myButtonState.IsMouseOnUi(Main.MouseScreen))
    //                    //{
    //                    //    Main.NewText("MouseHovering");
    //                    //    Main.blockInput = true;
    //                    //}
    //                }
    //                return true;
    //            },
    //            InterfaceScaleType.UI)
    //        );
    //    }
    //}
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        if (inventoryIndex != -1)
        {
            layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
                "WorldEdit: UI",
                () =>
                {
                    if (userInterface?.CurrentState != null)
                    {
                        userInterface.Draw(Main.spriteBatch, new GameTime());

                        if (userInterface?.CurrentState is UIState uiState && IsMouseOverUI(uiState))
                        {
                            //Main.NewText("MouseHovering");
                            //Main.blockInput = true; // Prevents mouse input from affecting world
                            Main.blockMouse = true;
                        }
                        else
                        {
                            Main.blockMouse = false;
                            //Main.NewText("MouseNotHovering");
                        }
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
    public static bool IsMouseOverUI(UIState state)
    {
        foreach (var element in state.Children) // 'Children' is a public wrapper for Elements
        {
            if (IsMouseOverUIRecursive(element))
                return true;
        }
        return false;
    }

    private static bool IsMouseOverUIRecursive(UIElement element)
    {
        if (element.ContainsPoint(Main.MouseScreen))
            return true;

        foreach (var child in element.Children)
        {
            if (IsMouseOverUIRecursive(child))
                return true;
        }

        return false;
    }
}
