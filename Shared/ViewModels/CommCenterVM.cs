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
        [Required]
        public string CityId
        {
            get
            {
                if (City == ObjectId.Empty)
                    return null;
                return City.ToString();
            }
            set
            {
                if (ObjectId.TryParse(value, out ObjectId id))
                    City = id;
            }
        }

    }
}
