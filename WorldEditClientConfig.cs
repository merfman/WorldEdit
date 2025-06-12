using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace WorldEdit;
public class WorldEditClientConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [Label("Fast Clipboard (Trades tile accuracy for better speed and memory use)")]
    [Tooltip("Trades tile accuracy and may not save some mod specific tile attributes for better speed and memory use")] // How to add a more detailed information section for a config property
    [DefaultValue(false)]
    /// <summary>Controls how the clipboard stores tile data</summary>
    /// <remarks>When true it trades tile accuracy for better speed and memory use</remarks>
    public bool FastClipboard { get; set; } = false;
}
public class WorldEditServerConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;


}
