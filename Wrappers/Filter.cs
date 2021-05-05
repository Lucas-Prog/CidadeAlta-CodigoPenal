using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CidadeAlta_CodigoPenal.Wrappers
{
    public class Filter
    {
        public string valueType { get; set; }
        public string value { get; set; }

        public Filter(string filter)
        {
            var data = filter.Split("_");

            this.valueType = data[0];
            this.value = data[1];
        }
    }
}
