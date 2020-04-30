﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HSE_Notification.Models
{
    public class Bus
    {
        public DateTime DepartureTime { get; set; }
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
        public string Occupancy { get; set; }
        public bool Notify { get; set; }
        public string Day { get; set; }
    }
}