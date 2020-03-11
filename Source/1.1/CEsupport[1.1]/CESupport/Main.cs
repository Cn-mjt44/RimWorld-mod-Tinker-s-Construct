using System;
using System.Reflection;
using HarmonyLib;
using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace CESupport
{
    [StaticConstructorOnStartup]
    public class Main
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        static Main()
        {
            PathStatWorker_Magazine.CompAmmoUser = AccessTools.TypeByName("CombatExtended.CompAmmoUser");
            PathStatWorker_Magazine.CompProperties_AmmoUser = AccessTools.TypeByName("CombatExtended.CompProperties_AmmoUser");
            Type StatWorker_MeleeDamage = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamage");
            Type StatWorker_MeleeArmorPenetration = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeArmorPenetration");
            Type StatWorker_MeleeDamageAverage = AccessTools.TypeByName("CombatExtended.StatWorker_MeleeDamageAverage");
            Type Verb_LaunchProjectileCE = AccessTools.TypeByName("CombatExtended.Verb_LaunchProjectileCE");
            Type StatWorker_Magazine = AccessTools.TypeByName("CombatExtended.StatWorker_Magazine");
            harmonyInstance = new Harmony("com.RimStove.AssembleWeapon.mod.CESupport");
            MethodInfo method4;
            MethodInfo method3;
            MethodInfo method2;
            MethodInfo method;
            if (StatWorker_MeleeArmorPenetration != null)
            {
                method = typeof(PathStatWorker_MeleeArmorPenetration1).GetMethod("GetStatDrawEntryLabelPrefix");
                method2 = typeof(PathStatWorker_MeleeArmorPenetration2).GetMethod("GetExplanationUnfinalizedPrefix");
                method3 = StatWorker_MeleeArmorPenetration.GetMethod("GetStatDrawEntryLabel", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatDef), typeof(float), typeof(ToStringNumberSense), typeof(StatRequest) }, null);
                method4 = StatWorker_MeleeArmorPenetration.GetMethod("GetExplanationUnfinalized", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatRequest), typeof(ToStringNumberSense) }, null);
                harmonyInstance.Patch(method3, new HarmonyMethod(method));
                harmonyInstance.Patch(method4, new HarmonyMethod(method2));
            }
            if(StatWorker_MeleeDamage != null)
            {
                method = typeof(PathStatWorker_MeleeDamage1).GetMethod("GetStatDrawEntryLabelPrefix");
                method2 = typeof(PathStatWorker_MeleeDamage2).GetMethod("GetExplanationUnfinalizedPrefix");
                method3 = StatWorker_MeleeDamage.GetMethod("GetStatDrawEntryLabel", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatDef), typeof(float), typeof(ToStringNumberSense), typeof(StatRequest) }, null);
                method4 = StatWorker_MeleeDamage.GetMethod("GetExplanationUnfinalized", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatRequest), typeof(ToStringNumberSense) }, null);
                harmonyInstance.Patch(method3, new HarmonyMethod(method));
                harmonyInstance.Patch(method4, new HarmonyMethod(method2));
            }
            if(StatWorker_MeleeDamageAverage != null)
            {
                method = typeof(PathStatWorker_MeleeDamageAverage1).GetMethod("GetValueUnfinalizedPrefix");
                method2 = typeof(PathStatWorker_MeleeDamageAverage2).GetMethod("GetExplanationUnfinalizedPrefix");
                method3 = StatWorker_MeleeDamageAverage.GetMethod("GetValueUnfinalized", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatRequest), typeof(bool) }, null);
                method4 = StatWorker_MeleeDamageAverage.GetMethod("GetExplanationUnfinalized", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatRequest), typeof(ToStringNumberSense) }, null);
                harmonyInstance.Patch(method3, new HarmonyMethod(method));
                harmonyInstance.Patch(method4, new HarmonyMethod(method2));
            }
            if(StatWorker_Magazine != null)
            {
                method = typeof(PathStatWorker_Magazine).GetMethod("GetValueUnfinalizedPrefix");
                method2 = typeof(PathStatWorker_Magazine).GetMethod("GetExplanationUnfinalizedPrefix");
                method3 = StatWorker_Magazine.GetMethod("GetValueUnfinalized", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatRequest), typeof(bool) }, null);
                method4 = StatWorker_Magazine.GetMethod("GetExplanationUnfinalized", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatRequest), typeof(ToStringNumberSense) }, null);
                harmonyInstance.Patch(method3, new HarmonyMethod(method));
                harmonyInstance.Patch(method4, new HarmonyMethod(method2));

                method = typeof(PathStatWorker_Magazine).GetMethod("GetStatDrawEntryLabelPrefix");
                method3 = StatWorker_Magazine.GetMethod("GetStatDrawEntryLabel", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[] { typeof(StatDef), typeof(float), typeof(ToStringNumberSense), typeof(StatRequest) }, null);
                harmonyInstance.Patch(method3, new HarmonyMethod(method));
            }
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }

        public static Harmony harmonyInstance;
    }
}
