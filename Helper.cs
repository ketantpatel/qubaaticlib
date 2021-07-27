using MDACLib.adapter;
using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib
{
    public class Helper : MasterAdapter
    {
        public Helper(UserBean user)
        {
            this.AppUserBean = user;
        }
        
    }
}
