using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Structures.Reports.Interfaces
{
    public interface IReportBuilder<T> where T:IReport
    {
        public void LoadData();
        public void CountMetrics();

        public T GetResult();
    }
}
