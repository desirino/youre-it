using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExitGames.Client
{
    public class FriendInfo
    {
        public string Name { get; internal protected set; }
        public bool IsOnline { get; internal protected set; }
        public string Room { get; internal protected set; }
        public bool IsInRoom { get { return string.IsNullOrEmpty(this.Room); } }

        public override string ToString()
        {
            return string.Format("{0}\t({1})\tin room: '{2}'", this.Name, (this.IsOnline) ? "on": "off", this.Room );
        }
    }
}
