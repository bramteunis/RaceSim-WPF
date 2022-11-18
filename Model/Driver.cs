using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {
        public string? Name { get; set; }
        public int Points { get; set; }
        public IEquipment? Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public Driver(String Name,TeamColors kleur) { 
            Random random = new Random();
            Equipment = new Car();
            Equipment.Speed = random.Next(9, 11);
            Equipment.Performance = random.Next(1, 3);
            Equipment.IsBroken = false;
            Equipment.Quality = random.Next(60, 100);
            this.Name = Name;
            this.Points = 0;
            TeamColor = kleur;
        }
    }
}
