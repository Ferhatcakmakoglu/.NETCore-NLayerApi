using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public abstract class BaseEntity
    {

        //Burdaki Id yi EF Core otomatik ForeignKey olarak algılar
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }

        //Update ilk esnada null olmalı bu yuzden DateTime? oldu
        public DateTime? Updatedtime { get; set; }
    }
}
