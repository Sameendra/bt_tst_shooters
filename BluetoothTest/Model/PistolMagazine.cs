using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothTest.Model
{
    class PistolMagazine : Magazine
    {
        public PistolMagazine()
        {
            MAX_AMMO_POSSIIBLE = 20;
            this.ammoLeft = MAX_AMMO_POSSIIBLE;
        }

    }
}
