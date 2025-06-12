using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace WorldEdit.Content.Items
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class WoodenAxe : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.WorldEdit.hjson' file.
		public override void SetDefaults()
		{
			DisplayName.Format("Wooden Axe");
			Tooltip.Format("To select World Edit positions");
			Item.damage = 50;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(silver: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

        public override bool? UseItem(Player player)
        {
			WorldEditPlayer modPlayer = player.GetModPlayer<WorldEditPlayer>();
			if (modPlayer == null) throw new System.NullReferenceException("modPlayer is null");

            int tileX = (int)(Main.MouseWorld.X / 16f);
            int tileY = (int)(Main.MouseWorld.Y / 16f);
            Tile tile = Main.tile[tileX, tileY];

            if (tile != null && tile.HasTile)
            {
                ushort type = tile.TileType;
                Main.NewText($"Tile at ({tileX}, {tileY}) is of type {type}", 255, 255, 0);
            }
            else
                Main.NewText($"No active tile at ({tileX}, {tileY})", 200, 200, 200);

            return base.UseItem(player);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
