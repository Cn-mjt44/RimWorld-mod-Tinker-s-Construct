using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Text;
using RimWorld;
using Verse;
using AssembleWeapon;

namespace CESupport
{
    public static class PathStatWorker_MeleeArmorPenetration2
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
            ThingDef thingDef = (req.Thing != null) ? req.Thing.def : req.Def as ThingDef ;
            if (comp != null)
            {
                if (!comp.part.NullOrEmpty<Thing>())
                {
                    foreach(Thing t in comp.part)
                    {
                        stringBuilder.AppendLine(t.Label + ":\n\n");
                        stringBuilder.AppendLine(DefDatabase<StatDef>.GetNamed("MeleeWeapon_AverageArmorPenetration").Worker.GetExplanationUnfinalized(StatRequest.For(t), numberSense));
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
                Thing thing = req.Thing;
                float num = (thing != null) ? thing.GetStatValue(DefDatabase<StatDef>.GetNamed("MeleePenetrationFactor"), true) : 1f;
                stringBuilder.AppendLine("WeaponPenetrationFactor".Translate() + " : " + num.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute));
                stringBuilder.AppendLine();
                foreach (Tool tool in list)
                {
                    if (tool == null) return true;
                    IEnumerable<ManeuverDef> enumerable = from d in DefDatabase<ManeuverDef>.AllDefsListForReading where tool.capacities.Contains(d.requiredCapacity) select d;
                    string text = "(";
                    foreach (ManeuverDef maneuverDef in enumerable)
                    {
                        text = text + maneuverDef.ToString() + "/";
                    }
                    float armorPenetrationSharp = tool.GetPrivateField<float>("armorPenetrationSharp", ToolCE);
                    float armorPenetrationBlunt = tool.GetPrivateField<float>("armorPenetrationBlunt", ToolCE);
                    text = text.TrimmedToLength(text.Length - 1) + ")";
                    stringBuilder.AppendLine("  " + "Tool".Translate() + ": " + tool.ToString() + " " + text);
                    stringBuilder.AppendLine(string.Format("    " + "SharpPenetration".Translate() + ": {0} x {1} = {2} mm RHA", armorPenetrationSharp.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), num.ToStringByStyle(ToStringStyle.FloatMaxThree, ToStringNumberSense.Absolute), (armorPenetrationSharp * num).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
                    stringBuilder.AppendLine(string.Format("    " + "BluntPenetration".Translate() + ": {0} x {1} = {2} MPa", armorPenetrationBlunt.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), num.ToStringByStyle(ToStringStyle.FloatMaxThree, ToStringNumberSense.Absolute), (armorPenetrationBlunt * num).ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute)));
                    stringBuilder.AppendLine();
                }
                __result = stringBuilder.ToString();
                return false;
            }
            return true;
        }
    }
}
