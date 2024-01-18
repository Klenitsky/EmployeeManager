using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.Interfaces
{
    public interface IReport
    {
        public void PrintToXLSX(string filename);
        public void PrintToWord(string filename);
        public void PrintToPDF(string filename);
        public void PrintToTXT(string filename);
    }
}
