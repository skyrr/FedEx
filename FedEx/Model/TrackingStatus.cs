﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FedEx.Model
{
    class TrackingStatus
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string DeliveryStatus { get; set; }
    }
}
