using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using AssembleWeapon;
using System.Reflection;
using System.Text;

namespace CESupport
{
    public static class PathStatWorker_Magazine
    {
        public static bool GetValueUnfinalizedPrefix(ref float __result, StatRequest req, bool applyPostProcess)
        {
            ThingDef thingDef = GunDef(req);
            float? num;
            if (thingDef == null || CompAmmoUser == null || CompProperties_AmmoUser == null)
            {
                num = null;
                return true;
            }
            else
            {
                CompProperties compProperties = (req.HasThing ? (req.Thing as ThingWithComps)?.AllComps?.Find(x => x.GetType() == CompAmmoUser)?.props : thingDef?.comps.Find(x => x.GetType() == CompProperties_AmmoUser));
                if (compProperties == null)
                {
                    return true;
                }
                if (compProperties.GetType() != CompProperties_AmmoUser)
                {
                    return true;
                }
                num = new int?(compProperties.GetPrivateField<int>("magazineSize", CompProperties_AmmoUser));
            }
            __result = num ?? ((float)0);
            return false;
        }

        public static bool GetExplanationUnfinalizedPrefix(ref string __result, StatRequest req, ToStringNumberSense numberSense)
        {
            if (CompAmmoUser == null || CompProperties_AmmoUser == null)
            {
                return true;
            }
            StringBuilder stringBuilder = new StringBuilder();
            ThingDef thingDef = GunDef(req);
            CompProperties compProperties_AmmoUser = (req.HasThing ? (req.Thing as ThingWithComps)?.AllComps?.Find(x => x.GetType() == CompAmmoUser)?.props : thingDef?.comps.Find(x => x.GetType() == CompProperties_AmmoUser));
            if(compProperties_AmmoUser == null)
            {
                return true;
            }
            if (compProperties_AmmoUser.GetType() != CompProperties_AmmoUser)
            {
                return true;
            }
            stringBuilder.AppendLine("CE_MagazineSize".Translate() + ": " + ((float)compProperties_AmmoUser.GetPrivateField<int>("magazineSize", CompProperties_AmmoUser)).ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Absolute));
            stringBuilder.AppendLine("CE_ReloadTime".Translate() + ": " + compProperties_AmmoUser.GetPrivateField<float>("reloadTime", CompProperties_AmmoUser).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute) + " s");
            __result = stringBuilder.ToString().TrimEndNewlines();
            return false;
        }
        public static bool GetStatDrawEntryLabelPrefix(ref string __result, StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq)
        {
            if (CompAmmoUser == null || CompProperties_AmmoUser == null)
            {
                return true;
            }
            ThingDef thingDef = GunDef(optionalReq);
            CompProperties compProperties_AmmoUser = (optionalReq.HasThing ? (optionalReq.Thing as ThingWithComps)?.AllComps?.Find(x => x.GetType() == CompAmmoUser)?.props : thingDef?.comps.Find(x => x.GetType() == CompProperties_AmmoUser));
            if (compProperties_AmmoUser == null)
            {
                return true;
            }
            if (compProperties_AmmoUser.GetType() != CompProperties_AmmoUser)
            {
                return true;
            }
            __result = string.Concat(new object[]
            {
                compProperties_AmmoUser.GetPrivateField<int>("magazineSize", CompProperties_AmmoUser),
                " / ",
                compProperties_AmmoUser.GetPrivateField<float>("reloadTime", CompProperties_AmmoUser).ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute),
                " s"
            });
            return false;
        }
        private static ThingDef GunDef(StatRequest req)
        {
            ThingDef thingDef = req.Def as ThingDef;
            bool? flag;
            if (thingDef == null)
            {
                flag = null;
            }
            else
            {
                BuildingProperties building = thingDef.building;
                flag = ((building != null) ? new bool?(building.IsTurret) : null);
            }
            if (flag ?? false)
            {
                thingDef = thingDef.building.turretGunDef;
            }
            return thingDef;
        }
        public static Type CompProperties_AmmoUser;
        public static Type CompAmmoUser;
    }
}
