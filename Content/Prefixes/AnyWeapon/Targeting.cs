﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using MrPlagueRaces.Content.Buffs;
using MrPlagueRaces.Content.Projectiles;
using MrPlagueRaces.Content.Prefixes;
using static Terraria.ModLoader.ModContent;

namespace MrPlagueRaces.Content.Prefixes
{
	public class Targeting : ModPrefix
	{
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		public override bool CanRoll(Item item) {
			return false;
		}

		public override void ModifyValue(ref float valueMult) {
			valueMult *= 1.75f;
		}

		public override void Apply(Item item)
		{
			if (item.rare > -12) {
				if (item.rare >= ItemRarityID.LightRed) {
					item.rare = ItemRarityID.Cyan;
				}
				else {
					item.rare = ItemRarityID.LightRed;
				}
			}
		}

		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			damageMult = 1.15f;
			critBonus = 5;
		}
	}

	public class TargetingItem : GlobalItem
	{
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			int index = tooltips.FindIndex(tt => tt.Name.Equals("PrefixDamage") && tt.Mod == "Terraria");
			if (item.prefix == PrefixType<Targeting>())
			{
				var linePower = new TooltipLine(Mod, "Targeting", "While attacking, enemies that are on your cursor are weakened")
				{
					OverrideColor = new Color(212, 163, 255)
				};
				tooltips.Insert(index, linePower);
			}
		}

		public override bool? UseItem(Item item, Player player)
		{
			if (item.prefix == PrefixType<Targeting>())
			{
				NPC closestNPC = null;
				float sqrMaxDetectDistance = 20 * 20;

				for (int k = 0; k < Main.maxNPCs; k++) {
					NPC target = Main.npc[k];
					if (target.CanBeChasedBy()) {
						float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Main.MouseWorld);

						if (sqrDistanceToTarget < sqrMaxDetectDistance) {
							sqrMaxDetectDistance = sqrDistanceToTarget;
							closestNPC = target;
						}
					}
				}
				if (closestNPC != null) {
					closestNPC.AddBuff(BuffType<Targeted>(), 60);
				}
			}
			return base.UseItem(item, player);
		}
	}

	public class TargetingPlayer : ModPlayer
	{

	}
}