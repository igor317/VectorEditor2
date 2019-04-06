using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEditor
{
    class IpLayer
    {
        private static UInt16 counter = 0;
        public string name;
        public bool active;

        public IpLayer()
        {
            active = true;
            name = "Layer " + Convert.ToString(counter);
            counter++;

        }

        public IpLayer(string Name)
        {
            active = true;
            name = Name;
            counter++;
        }

    }
}
