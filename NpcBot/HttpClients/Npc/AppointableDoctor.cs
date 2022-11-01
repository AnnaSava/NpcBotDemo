using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Models
{
    public class AppointableDoctor
    {
        public long Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string Info { get; set; }


        public override string ToString()
        {
            return $"{LastName.Trim()} {FirstName.Trim()} {Patronymic.Trim()}";
        }
    }
}
