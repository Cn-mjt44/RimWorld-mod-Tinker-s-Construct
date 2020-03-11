using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using AssembleWeapon;
using System.Text;

namespace CESupport
{
    public static class PathStatWorker_MeleeArmorPenetration1
    {
        public static bool GetStatDrawEntryLabelPrefix(ref string __result, StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq)
        {
            Type ToolCE = AccessTools.TypeByName("ToolCE");
            if(ToolCE == null)
            {
                return true;
            }
            CompAssembleWeapon comp = optionalReq.Thing?.TryGetComp<CompAssembleWeapon>();
            ThingDef thingDef = (optionalReq.Thing != null) ? optionalReq.Thing.def : optionalReq.Def as ThingDef;
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
                if (list.NullOrEmpty<Tool>())
                {
                    __result = "";
                }
                if (list.Any((Tool x) => !ToolCE.IsAssignableFrom(x.GetType())))
                {
                    Log.Error("Trying to get stat MeleeArmorPenetration from " + optionalReq.Def.defName + " which has no support for Combat Extended.", false);
                    __result = "";
                }
                float num = 0f;
                foreach (Tool tool in list)
                {
                    num += tool.chanceFactor;
                }
                float num2 = 0f;
                float num3 = 0f;
                foreach (Tool tool2 in list)
                {
                    if (tool2 == null) return true;
                    float num4 = tool2.chanceFactor / num;
                    num2 += num4 * tool2.GetPrivateField<float>("armorPenetrationSharp", ToolCE);
                    num3 += num4 * tool2.GetPrivateField<float>("armorPenetrationBlunt", ToolCE);
                }
                Thing thing = optionalReq.Thing;
                float num5 = (thing != null) ? thing.GetStatValue(DefDatabase<StatDef>.GetNamed("MeleePenetrationFactor"), true) : 1f;
                __result = (num2 * num5).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute) + "mm RHA, " + (num3 * num5).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute) + " MPa";
                return false;
            }
            else if (!((thingDef != null) ? thingDef.tools : null).NullOrEmpty<Tool>())
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
                if (list.NullOrEmpty())
                {
                    __result = "";
                }
                if (list.Any((Tool x) => !ToolCE.IsAssignableFrom(x.GetType())))
                {
                    Log.Error("Trying to get stat MeleeArmorPenetration from " + optionalReq.Def.defName + " which has no support for Combat Extended.", false);
                    __result = "";
                }
                float num = 0f;
                foreach (Tool tool in list)
                {
                    num += tool.chanceFactor;
                }
                float num2 = 0f;
                float num3 = 0f;
                foreach (Tool tool2 in list)
                {
                    if (tool2 == null) return true;
                    float num4 = tool2.chanceFactor / num;
                    num2 += num4 * tool2.GetPrivateField<float>("armorPenetrationSharp", ToolCE);
                    num3 += num4 * tool2.GetPrivateField<float>("armorPenetrationBlunt", ToolCE);
                }
                Thing thing = optionalReq.Thing;
                float num5 = (thing != null) ? thing.GetStatValue(DefDatabase<StatDef>.GetNamed("MeleePenetrationFactor"), true) : 1f;
                __result = (num2 * num5).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute) + "mm RHA, " + (num3 * num5).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute) + " MPa";
                return false;
            }
            return true;
        }
    }
}
