using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TciPM.Blazor.Shared.Models;

namespace TciPM.Blazor.Shared.ViewModels
{
    public class CommCenterVM : CommCenterX
    {
        public List<Diesel> Diesels { get; set; } = new List<Diesel>();
        public List<RectifierAndBattery> RectifierAndBatteries { get; set; } = new List<RectifierAndBattery>();
        public List<Ups> Upses { get; set; } = new List<Ups>();
        public List<AirConditioner> AirConditioners { get; set; } = new List<AirConditioner>();
    }
}
