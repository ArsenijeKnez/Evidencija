using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public delegate void DBUpdate(Load load);
    public class DeviationUpdate
    {
        public DeviationUpdate()
        {
        }

        public event DBUpdate DBUpdateLoad;

        public void UpdateForLoad(Load load, DBUpdate Method)
        {
            Method(load);
            DBUpdateLoad?.Invoke(load);
        }

        public void UpdateInMemory(Load load) 
        {
        //Ubaci u bazu
        }

        public void UpdateXML(Load load)
        {
            //Ubaci u bazu
        }
    }
}
