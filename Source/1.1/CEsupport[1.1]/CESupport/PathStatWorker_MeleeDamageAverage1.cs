using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using AssembleWeapon;
using System.Reflection;

namespace CESupport
{
    public static class PathStatWorker_MeleeDamageAverage1
    {
        public static bool GetValueUnfinalizedPrefix(ref float __result, StatRequest req, bool applyPostProcess)
        {
            Type ToolCE = AccessTools.TypeByName("ToolCE");
            if (ToolCE == null)
            {
                return true;
            }
            CompAssembleWeapon comp = req.Thing?.TryGetComp<CompAssembleWeapon>();
            ThingDef thingDef = (req.Thing != null) ? req.Thing.def : req.Def as ThingDef;
            MethodInfo method1 = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamageBase").GetMethod("GetDamageVariationMin", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(Pawn) }, null);
            MethodInfo method2 = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamageBase").GetMethod("GetDamageVariationMax", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(Pawn) }, null);
            if (comp != null)
            {
                List<Tool> list = new List<Tool>();
                foreach (Tool t in comp.Tools)
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
                Thing thing = req.Thing;
                Pawn_EquipmentTracker pawn_EquipmentTracker;
                if ((pawn_EquipmentTracker = (((thing != null) ? thing.ParentHolder : null) as Pawn_EquipmentTracker)) != null)
                {
                    num = (float)method1.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                    num2 = (float)method2.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                }
                if (list.NullOrEmpty<Tool>())
                {
                    __result = 0f;
                }
                if (list.Any((Tool x) => !ToolCE.IsAssignableFrom(x.GetType())))
                {
                    Log.Error("Trying to get stat MeleeDamageAverage from " + req.Def.defName + " which has no support for Combat Extended.", false);
                    __result = 0f;
                    return false;
                }
                float num3 = 0f;
                foreach (Tool tool in list)
                {
                    num3 += tool.chanceFactor;
                }
                float num4 = 0f;
                foreach (Tool tool2 in list)
                {
                    if (tool2 == null) return true;
                    float adjustedDamage = PathStatWorker_MeleeDamage1.GetAdjustedDamage(tool2, req.Thing);
                    float num5 = adjustedDamage / tool2.cooldownTime * num;
                    float num6 = adjustedDamage / tool2.cooldownTime * num2;
                    float num7 = tool2.chanceFactor / num3;
                    num4 += num7 * ((num5 + num6) / 2f);
                }
                __result = num4;
                return false;
            }
            if (!((thingDef != null) ? thingDef.tools : null).NullOrEmpty<Tool>())
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
                Thing thing = req.Thing;
                Pawn_EquipmentTracker pawn_EquipmentTracker;
                if ((pawn_EquipmentTracker = (((thing != null) ? thing.ParentHolder : null) as Pawn_EquipmentTracker)) != null)
                {
                    num = (float)method1.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                    num2 = (float)method2.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                }
                if (list.NullOrEmpty<Tool>())
                {
                    __result = 0f;
                }
                if (list.Any((Tool x) => !ToolCE.IsAssignableFrom(x.GetType())))
                {
                    Log.Error("Trying to get stat MeleeDamageAverage from " + req.Def.defName + " which has no support for Combat Extended.", false);
                    __result = 0f;
                    return false;
                }
                float num3 = 0f;
                foreach (Tool tool in list)
                {
                    num3 += tool.chanceFactor;
                }
                float num4 = 0f;
                foreach (Tool tool2 in list)
                {
                    if (tool2 == null) return true;
                    float adjustedDamage = PathStatWorker_MeleeDamage1.GetAdjustedDamage(tool2, req.Thing);
                    float num5 = adjustedDamage / tool2.cooldownTime * num;
                    float num6 = adjustedDamage / tool2.cooldownTime * num2;
                    float num7 = tool2.chanceFactor / num3;
                    num4 += num7 * ((num5 + num6) / 2f);
                }
                __result = num4;
                return false;
            }
            return true;
        }
    }
}
