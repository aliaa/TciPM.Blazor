using EasyMongoNet;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TciPM.Blazor.Shared.Models;

namespace TciPM.Blazor.Shared.Models
{
    public enum EquipmentType
    {
        [Display(Name="دیزل ژنراتور")]
        Diesel,
        [Display(Name="یکسو کننده")]
        Rectifier,
        [Display(Name="باتری")]
        Battery,
        [Display(Name="UPS")]
        UPS,
        [Display(Name = "کمپرسور گاز کنترل")]
        Compressor,
        [Display(Name = "کابل هواخور")]
        GasCable
    }

    public static class EquipmentTypeExtentions
    {
        public static Type GetPmType(this EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.Diesel:      return typeof(DieselPM);
                case EquipmentType.Rectifier:   return typeof(RectifierPM);
                case EquipmentType.Battery:     return typeof(BatteryPM);
                case EquipmentType.UPS:         return typeof(UpsPM);
                //case EquipmentType.Compressor:  return typeof(CompressorPM);
                //case EquipmentType.GasCable:    return typeof(GasCablePM);
                default:    throw new NotImplementedException();
            }
        }

        public static string GetPmFieldName(this EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.Diesel:      return nameof(EquipmentsPM.DieselsPM);
                case EquipmentType.Rectifier:   return nameof(EquipmentsPM.RectifiersPM);
                case EquipmentType.Battery:     return nameof(EquipmentsPM.BatteriesPM);
                case EquipmentType.UPS:         return nameof(EquipmentsPM.UpsPM);
                //case EquipmentType.Compressor:  return nameof(EquipmentsPM.CompressorsPM);
                //case EquipmentType.GasCable:    return nameof(EquipmentPM.GasCablesPM);
                default: throw new NotImplementedException();
            }
        }

        //public static IEnumerable<Equipment> GetEquipments(this EquipmentType equipmentType, IReadOnlyDbContext db)
        //{
        //    switch (equipmentType)
        //    {
        //        case EquipmentType.Diesel:      return db.All<Diesel>();
        //        case EquipmentType.Rectifier: 
        //        case EquipmentType.Battery:     return db.All<RectifierAndBattery>();
        //        case EquipmentType.UPS:         return db.All<Ups>();
        //        default: throw new NotImplementedException();
        //    }
        //}

        public static IEnumerable<EquipmentPM> GetEquipmentPm(this EquipmentType equipmentType, EquipmentsPM pm)
        {
            switch (equipmentType)
            {
                case EquipmentType.Diesel:      return pm.DieselsPM;
                case EquipmentType.Rectifier:   return pm.RectifiersPM;
                case EquipmentType.Battery:     return pm.BatteriesPM;
                case EquipmentType.UPS:         return pm.UpsPM;
                //case EquipmentType.Compressor: return pm.CompressorsPM);
                //case EquipmentType.GasCable:    pm.GasCablesPM);
                default: throw new NotImplementedException();
            }
        }

    }
}