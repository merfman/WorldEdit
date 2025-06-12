using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;

namespace WorldEdit;
public struct Clipboard
{
    public int[,] values;
    public Vector2 offset;
}