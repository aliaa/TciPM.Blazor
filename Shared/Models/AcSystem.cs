using System.ComponentModel;

namespace TciPM.Blazor.Shared.Models
{
    public class AcSystem : Equipment
    {
        public ElectricalEnclosure MainEnclosure { get; set; }

        [DisplayName("توان ترانسفرمر (KVA)")]
        public float TrasnformerPower { get; set; }
    }
}