using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abdal_Admin_Finder
{
    class AbdalControler
    {
        private static bool v_unauthorized_process = false;
        private static bool v_stop_force_process = false;
        
        public static bool unauthorized_process
        {
            get { return v_unauthorized_process; }
            set { v_unauthorized_process = value; }
        } 
        
        public static bool stop_force_process
        {
            get { return v_stop_force_process; }
            set { v_stop_force_process = value; }
        }
    }
}
