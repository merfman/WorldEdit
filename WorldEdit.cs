using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace WorldEdit;
// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
public class WorldEdit : Mod
{
    public int Set(Player player = null)
    {
        if (player == null) player = Main.LocalPlayer;
        WorldEditPlayer modPlayer = null;
        try { modPlayer = player.GetModPlayer<WorldEditPlayer>(); }
        catch { Main.NewText($"mod player: null", Color.Red); return 0; }
        
        //Main.NewText($"mod player:{modPlayer.Player.name}");

        Vector2 selec1 = modPlayer.Selection1;
        Vector2 selec2 = modPlayer.Selection2;

        Point p1 = modPlayer.Selection1.ToPoint();
        Point p2 = modPlayer.Selection2.ToPoint();

        // Ensure correct bounds (handles reverse selections)
        int minX = (int)Math.Min(p1.X, p2.X);
        int maxX = (int)Math.Max(p1.X, p2.X);
        int minY = (int)Math.Min(p1.Y, p2.Y);
        int maxY = (int)Math.Max(p1.Y, p2.Y);

        Tile source = Main.tile[(int)selec1.X, (int)selec1.Y];
        try { source = modPlayer.clipboard.Tiles[0, 0]; }
        catch { Main.NewText($"clipboard: Null", Color.Red); return 0; }

        if (modPlayer.clipboard.Tiles == null || modPlayer.clipboard.Size == Vector2.Zero)
        {
            Main.NewText("Clipboard is empty or uninitialized", Color.Red);
            return 0;
        }

        // Changes the tiles
        for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            {
                if (!WorldGen.InWorld(x, y)) continue;

                Tile dest = Main.tile[x, y];
                dest.CopyFrom(source);
            }

        // Updates the Tiles 
        for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            {
                if (!WorldGen.InWorld(x, y)) continue;

                // Update visuals
                WorldGen.SquareTileFrame(x, y, true);

                WorldGen.SquareWallFrame(x, y, true);

                // Optional: sync tile in multiplayer
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, x, y, 1);
            }



        return 0;
    }
    public int Copy(Player player = null)
    {
        if (player == null) player = Main.LocalPlayer;
        WorldEditPlayer modPlayer = null;
        try { modPlayer = player.GetModPlayer<WorldEditPlayer>(); }
        catch { Main.NewText($"mod player: null", Color.Red); return 0; }
        Main.NewText($"mod player:{modPlayer.Player.name}");

        Vector2 selec1 = modPlayer.Selection1;
        Vector2 selec2 = modPlayer.Selection2;

        Point p1 = modPlayer.Selection1.ToPoint();
        Point p2 = modPlayer.Selection2.ToPoint();

        // Ensure correct bounds (handles reverse selections)
        int minX = (int)Math.Min(p1.X, p2.X);
        int maxX = (int)Math.Max(p1.X, p2.X);
        int minY = (int)Math.Min(p1.Y, p2.Y);
        int maxY = (int)Math.Max(p1.Y, p2.Y);


        modPlayer.clipboard = new Clipboard();

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        modPlayer.clipboard = new Clipboard
        {
            Tiles = new Tile[width, height],
            Size = new Vector2(width, height)
        };

        Main.NewText($"Size:{modPlayer.clipboard.Size}");

        // Changes the tiles
        for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            {
                if (!WorldGen.InWorld(x, y)) continue;

                int localX = x - minX;
                int localY = y - minY;

                modPlayer.clipboard.Tiles[localX, localY] = (Main.tile[x, y]);
            }

        return 0;
    }

    public int Paste(Player player = null)
    {
        if (player == null) player = Main.LocalPlayer;
        WorldEditPlayer modPlayer = null;
        try { modPlayer = player.GetModPlayer<WorldEditPlayer>(); }
        catch { Main.NewText($"mod player: null", Color.Red); return 0; }
        //Main.NewText($"mod player:{modPlayer.Player.name}");

        Vector2 selec1 = modPlayer.Selection1;
        Vector2 selec2 = modPlayer.Selection2;

        Point p1 = modPlayer.Selection1.ToPoint();
        Point p2 = modPlayer.Selection2.ToPoint();

        // Ensure correct bounds (handles reverse selections)
        int minX = (int)Math.Min(p1.X, p2.X);
        int maxX = (int)Math.Max(p1.X, p2.X);
        int minY = (int)Math.Min(p1.Y, p2.Y);
        int maxY = (int)Math.Max(p1.Y, p2.Y);

        //Clipboard? clipboard = modPlayer.clipboard;
        if (modPlayer.clipboard.Tiles == null || modPlayer.clipboard.Size == Vector2.Zero)
            {
            Main.NewText("Clipboard is empty or uninitialized", Color.Red);
            return 0;
        }

        int width = (int)modPlayer.clipboard.Size.X;
        int height = (int)modPlayer.clipboard.Size.Y;

        Main.NewText($"Size:{modPlayer.clipboard.Size}");
        Main.NewText($"width:({width}, {height})");
        // Changes the tiles
        for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            {
                if (!WorldGen.InWorld(x, y)) continue;

                int localX = (x - minX) % width;
                int localY = (y - minY) % height;

                if (localX < 0) localX += width;
                if (localY < 0) localY += height;

                //Main.NewText($"PasteTile:({modPlayer.clipboard.Tiles[localX, localY]})", Color.Green);
                Main.tile[x, y].CopyFrom(modPlayer.clipboard.Tiles[localX, localY]);
            }

        // Updates the Tiles 
        for (int x = minX; x <= maxX; x++)
            for (int y = minY; y <= maxY; y++)
            {
                if (!WorldGen.InWorld(x, y)) continue;

                // Update visuals
                WorldGen.SquareTileFrame(x, y, true);

                WorldGen.SquareWallFrame(x, y, true);

                // Optional: sync tile in multiplayer
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, x, y, 1);
            }

        return 0;
	}
}
