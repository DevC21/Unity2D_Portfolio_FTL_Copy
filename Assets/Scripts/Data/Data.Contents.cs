using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Data
{ 
#region Stat
	[Serializable]
	public class Stat
	{
		public int shipCode;
		public int reactor;
		public int hp;
		public int maxHp;
        public int piloting;
        public int doors;
        public int sensors;
        public int medbay;
        public int oxygen;
        public int shields;
        public int engines;
        public int weapons;
    }

	[Serializable]
	public class StatData : ILoader<int, Stat>
	{
		public List<Stat> stats = new List<Stat>();

		public Dictionary<int, Stat> MakeDict()
		{
			Dictionary<int, Stat> dict = new Dictionary<int, Stat>();
			foreach (Stat stat in stats)
				dict.Add(stat.shipCode, stat);
			return dict;
		}
	}
#endregion

#region Event
    [Serializable]
    public class Event
    {
        public int EventCode;
        public string EventText;
        public EventFlag Flag;
    }

    [Serializable]
    public class EventData : ILoader<int, Event>
    {
        public List<Event> Events = new List<Event>();

        public Dictionary<int, Event> MakeDict()
        {
            Dictionary<int, Event> dict = new Dictionary<int, Event>();
            foreach (Event evt in Events)
                dict.Add(evt.EventCode, evt);
            return dict;
        }
    }
#endregion
}