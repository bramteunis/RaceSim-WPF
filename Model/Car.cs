using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Car : IEquipment
    {
        int IEquipment.Quality { get; set; }
        int IEquipment.Performance { get; set; }
        int IEquipment.Speed { get; set; }
        bool IEquipment.IsBroken { get; set; }
        public Car() {
        }
    }
}
