using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace WorldEdit;
public class WorldEditPlayer : ModPlayer
{
    public Vector2 Selection1;
    public Vector2 Selection2;
    public Clipboard clipboard;
    
}

public struct Clipboard
{
    public int[,] values;
    public Vector2 offset;
}
