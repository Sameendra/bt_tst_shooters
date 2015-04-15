using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluetoothTest.Model
{
    class EmptyAmmoException : Exception
    {

        public EmptyAmmoException(String message) : base(message +"No Ammo Left")
        {
           
        }

        public EmptyAmmoException()
        {
            // TODO: Complete member initialization
        }
    }
}
