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
    public static class PathStatWorker_MeleeDamage1
    {
        public static bool GetStatDrawEntryLabelPrefix(ref string __result, StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq)
        {
            Type ToolCE = AccessTools.TypeByName("ToolCE");
            if (ToolCE == null)
            {
                return true;
            }
            CompAssembleWeapon comp = optionalReq.Thing?.TryGetComp<CompAssembleWeapon>();
            ThingDef thingDef = (optionalReq.Thing != null) ? optionalReq.Thing.def : optionalReq.Def as ThingDef;
            MethodInfo method1 = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamageBase").GetMethod("GetDamageVariationMin", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(Pawn) }, null);
            MethodInfo method2 = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamageBase").GetMethod("GetDamageVariationMax", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(Pawn) }, null);
            if (comp != null)
            {
                float num = 0.5f;
                float num2 = 1.5f;
                Thing thing = optionalReq.Thing;
                Pawn_EquipmentTracker pawn_EquipmentTracker;
                if ((pawn_EquipmentTracker = (((thing != null) ? thing.ParentHolder : null) as Pawn_EquipmentTracker)) != null)
                {
                    num = (float)method1.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                    num2 = (float)method2.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                }
                List<Tool> list = new List<Tool>();
                foreach (Tool t in comp.Tools)
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
                if (list.NullOrEmpty<Tool>())
                {
                    __result = "";
                }
                if (list.Any((Tool x) => !ToolCE.IsAssignableFrom(x.GetType())))
                {
                    Log.Error("Trying to get stat MeleeDamage from " + optionalReq.Def.defName + " which has no support for Combat Extended.", false);
                    __result = "";
                }
                float num3 = 2.1474836E+09f;
                float num4 = 0f;
                foreach (Tool tool in list)
                {
                    if (tool == null) return true;
                    float adjustedDamage = PathStatWorker_MeleeDamage1.GetAdjustedDamage(tool, optionalReq.Thing);
                    if (adjustedDamage > num4)
                    {
                        num4 = adjustedDamage;
                    }
                    if (adjustedDamage < num3)
                    {
                        num3 = adjustedDamage;
                    }
                }
                __result = (num3 * num).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute) + " - " + (num4 * num2).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
                return false;
            }
            if (!((thingDef != null) ? thingDef.tools : null).NullOrEmpty<Tool>())
            {
                float num = 0.5f;
                float num2 = 1.5f;
                Thing thing = optionalReq.Thing;
                Pawn_EquipmentTracker pawn_EquipmentTracker;
                if ((pawn_EquipmentTracker = (((thing != null) ? thing.ParentHolder : null) as Pawn_EquipmentTracker)) != null)
                {
                    num = (float)method1.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                    num2 = (float)method2.Invoke(null, new object[] { pawn_EquipmentTracker.pawn });
                }
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
                if (list.NullOrEmpty<Tool>())
                {
                    __result = "";
                }
                if (list.Any((Tool x) => !ToolCE.IsAssignableFrom(x.GetType())))
                {
                    Log.Error("Trying to get stat MeleeDamage from " + optionalReq.Def.defName + " which has no support for Combat Extended.", false);
                    __result = "";
                }
                float num3 = 2.1474836E+09f;
                float num4 = 0f;
                foreach (Tool tool in list)
                {
                    if (tool == null) return true;
                    float adjustedDamage = PathStatWorker_MeleeDamage1.GetAdjustedDamage(tool, optionalReq.Thing);
                    if (adjustedDamage > num4)
                    {
                        num4 = adjustedDamage;
                    }
                    if (adjustedDamage < num3)
                    {
                        num3 = adjustedDamage;
                    }
                }
                __result = (num3 * num).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute) + " - " + (num4 * num2).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute);
                return false;
            }
            return true;
        }
        public static float GetAdjustedDamage(Tool tool, Thing thingOwner)
        {
            List<ToolCapacityDef> capacities = tool.capacities;
            DamageDef damageDef;
            if (capacities == null)
            {
                damageDef = null;
            }
            else
            {
                ToolCapacityDef toolCapacityDef = capacities.First<ToolCapacityDef>();
                if (toolCapacityDef == null)
                {
                    damageDef = null;
                }
                else
                {
                    IEnumerable<VerbProperties> verbsProperties = toolCapacityDef.VerbsProperties;
                    if (verbsProperties == null)
                    {
                        damageDef = null;
                    }
                    else
                    {
                        VerbProperties verbProperties = verbsProperties.First<VerbProperties>();
                        damageDef = ((verbProperties != null) ? verbProperties.meleeDamageDef : null);
                    }
                }
            }
            return tool.AdjustedBaseMeleeDamageAmount(thingOwner, damageDef);
        }
    }
}
