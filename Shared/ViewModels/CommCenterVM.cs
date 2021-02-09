using System.Collections.Generic;
using TciPM.Blazor.Shared.Models;
using TciPM.Blazor.Shared.Models.Equipments;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class CommCenterVM : CommCenterX
    {
        public List<Diesel> Diesels { get; set; } = new List<Diesel>();
        public List<RectifierAndBattery> RectifierAndBatteries { get; set; } = new List<RectifierAndBattery>();
        public List<Ups> Upses { get; set; } = new List<Ups>();
        public List<Compressor> Compressors { get; set; } = new List<Compressor>();
        public List<GasCable> GasCables { get; set; } = new List<GasCable>();
        public List<AirConditioner> AirConditioners { get; set; } = new List<AirConditioner>();
    }
}
