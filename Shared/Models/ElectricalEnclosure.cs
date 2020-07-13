using System.Collections.Generic;
using System.ComponentModel;

namespace TciPM.Blazor.Shared.Models
{
    public class ElectricalEnclosure : AcEquipment
    {
        public class InsideItem
        {
            public ElectricalSwitch Switch { get; set; }
            public AcEquipment Target { get; set; }
            public string TargetName { get; set; }
        }
        
        [DisplayName("توضیح")]
        public string Description { get; set; }

        public List<InsideItem> Items { get; set; } = new List<InsideItem>();
    }
}