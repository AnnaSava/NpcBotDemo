using NpcBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcBot.Formatters
{
    public interface ITimetableFormatter
    {
        string Format(TimetableModel timetable);
    }
}
