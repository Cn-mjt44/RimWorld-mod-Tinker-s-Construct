using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Text;
using RimWorld;
using Verse;
using AssembleWeapon;
using System.Reflection;

namespace CESupport
{
    public static class PathStatWorker_MeleeDamageAverage2
    {
        public static bool GetExplanationUnfinalizedPrefix(ref string __result, StatRequest req, ToStringNumberSense numberSense)
        {
            Type ToolCE = AccessTools.TypeByName("ToolCE");
            if (ToolCE == null)
            {
                return true;
            }
            CompAssembleWeapon comp = req.Thing?.TryGetComp<CompAssembleWeapon>();
            StringBuilder stringBuilder = new StringBuilder();
            ThingDef thingDef = (req.Thing != null) ? req.Thing.def : req.Def as ThingDef;
            MethodInfo method1 = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamageBase").GetMethod("GetDamageVariationMin", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(Pawn) }, null);
            MethodInfo method2 = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamageBase").GetMethod("GetDamageVariationMax", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(Pawn) }, null);
            if (comp != null)
            {
                if (!comp.part.NullOrEmpty<Thing>())
                {
                    foreach(Thing t in comp.part)
                    {
                        stringBuilder.AppendLine(t.Label + ":\n\n");
                        stringBuilder.AppendLine(StatDefOf.MeleeWeapon_AverageDPS.Worker.GetExplanationUnfinalized(StatRequest.For(t), numberSense));
                    }
                    if (comp.Props.UseItselfeTools)
                    {
                        stringBuilder.AppendLine(req.Thing.Label + ":\n");
                        goto A;
                    }
                    __result = stringBuilder.ToString();
                }
                return false;
            }
            A:;
            if(!((thingDef != null) ? thingDef.tools : null).NullOrEmpty<Tool>())
            {
                List<Tool> list = new List<Tool>();
                foreach (Tool t in thingDef.tools)
                {
                    if (t != null)
                    {
                        if (!ToolCE.IsAssignableFrom(t.GetType()))
                        {
                            float num6 = (t.armorPenetration > 0) ? t.armorPenetration : t.power * 0.015f;
                            Tool cE = (Tool)Activator.CreateInstance(ToolCE);
                            cE.SetPrivateField(num6, "armorPenetrationBlunt", ToolCE);
                            cE.SetPrivateField(num6, "armorPenetrationSharp", ToolCE);
                            cE.SetPrivateField(Gender.None, "restrictedGender", ToolCE);
                            cE.armorPenetration = t.armorPenetration;
                            cE.capacities = t.capacities;
                            cE.label = t.label;
                            cE.power = t.power;
                            cE.cooldownTime = t.cooldownTime;
                            cE.linkedBodyPartsGroup = t.linkedBodyPartsGroup;
                            cE.chanceFactor = t.chanceFactor;
                            cE.ensureLinkedBodyPartsGroupAlwaysUsable = t.ensureLinkedBodyPartsGroupAlwaysUsable;
                            cE.hediff = t.hediff;
                            cE.surpriseAttack = t.surpriseAttack;
                            cE.id = t.id;
                            cE.labelUsedInLogging = t.labelUsedInLogging;
                            cE.alwaysTreatAsWeapon = t.alwaysTreatAsWeapon;
                            list.Add(cE);
                        }
                        else
                        {
                            list.Add(t);
                        }
                    }
                }
                float num = 0.5f;
                float num2 = 1.5f;
                int num3 = -1;
                Thing thing = req.Thing;
                Pawn_EquipmentTracker pawn_EquipmentTracker;
                if ((pawn_EquipmentTracker = (((thing != null) ? thing.ParentHolder : null) as Pawn_EquipmentTracker)) != null && pawn_EquipmentTracker.pawn != null)
                {
                    Pawn pawn = pawn_EquipmentTracker.pawn;
                    num = (float)method1.Invoke(null, new object[] { pawn });
                    num2 = (float)method2.Invoke(null, new object[] { pawn });
                    if (pawn.skills != null)
                    {
                        num3 = pawn.skills.GetSkill(SkillDefOf.Melee).Level;
                    }
                }
                if (list.NullOrEmpty())
                {
                    return true;
                }
                if (num3 >= 0)
                {
                    stringBuilder.AppendLine("WielderSkillLevel".Translate() + ": " + num3);
                }
                stringBuilder.AppendLine(string.Format("DamageVariation".Translate() + ": {0}% - {1}%", (100f * num).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), (100f * num2).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
                stringBuilder.AppendLine("");
                foreach (Tool tool in list)
                {
                    if (tool == null) return true;
                    float adjustedDamage = PathStatWorker_MeleeDamage1.GetAdjustedDamage(tool, req.Thing);
                    float num4 = adjustedDamage / tool.cooldownTime * num;
                    float num5 = adjustedDamage / tool.cooldownTime * num2;
                    IEnumerable<ManeuverDef> enumerable = from d in DefDatabase<ManeuverDef>.AllDefsListForReading where tool.capacities.Contains(d.requiredCapacity) select d;
                    string text = "(";
                    foreach (ManeuverDef maneuverDef in enumerable)
                    {
                        text = text + maneuverDef.ToString() + "/";
                    }
                    text = text.TrimmedToLength(text.Length - 1) + ")";
                    stringBuilder.AppendLine("  " + "Tool".Translate() + ": " + tool.ToString() + " " + text);
                    stringBuilder.AppendLine("    " + "BaseDamage".Translate() + ": " + tool.power.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute));
                    stringBuilder.AppendLine("    " + "AdjustedForWeapon".Translate() + ": " + adjustedDamage.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute));
                    stringBuilder.AppendLine("    " + "Cooldown".Translate() + ": " + tool.cooldownTime.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute) + "seconds".Translate());
                    stringBuilder.AppendLine("    " + "DamagePerSecond".Translate() + ": " + (adjustedDamage / tool.cooldownTime).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute));
                    stringBuilder.AppendLine(string.Format("    " + "DamageVariation".Translate() + ": {0} - {1}", num4.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), num5.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
                    stringBuilder.AppendLine("    " + "FinalAverageDamage".Translate() + ": " + ((num4 + num5) / 2f).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute));
                    stringBuilder.AppendLine();
                }
                __result = stringBuilder.ToString();
                return false;
            }
            return true;
        }
    }
}
