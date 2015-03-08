using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothTest.Model
{
    public class PlayerList
    {

        public PlayerList()
        {
            this.Players = new ObservableCollection<Player>();
        }
        public ObservableCollection<Player> Players { get; set; }

       
    }
}
