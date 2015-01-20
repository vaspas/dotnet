using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ipp;

namespace IppWrapper
{
    public static class IppHelper
    {
        public static void Do(IppStatus status)
        {
            if(status!=0)
                throw new Exception("Ipp function error " + status);
        }
    }
}
