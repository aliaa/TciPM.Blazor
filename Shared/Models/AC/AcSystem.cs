using System.ComponentModel;
using TciPM.Blazor.Shared.Models.Equipments;

namespace TciPM.Blazor.Shared.Models.AC
{
    public class AcSystem : Equipment
    {
        public ElectricalEnclosure MainEnclosure { get; set; }

        [DisplayName("توان ترانسفرمر (KVA)")]
        public float TrasnformerPower { get; set; }
    }
}