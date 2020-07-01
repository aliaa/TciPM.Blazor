using System;
using System.Collections.Generic;
using System.Text;
using TciCommon.Models;

namespace TciPM.Blazor.Shared.Models
{
    public class ClientAuthUser : BaseAuthUser
    {
        public string ProvincePrefix { get; set; }
    }
}
