using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    class Program
    {
        static void Main(string[] args)
        {
            BackendService service = new BackendService();
            service.Run();
        }
    }
}
