//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFService
{
    using System;
    using System.Collections.Generic;
    
    public partial class bus_trace
    {
        public int id { get; set; }
        public string line_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public byte direction { get; set; }
        public short order_number { get; set; }
        public string exactTime { get; set; }
    }
}
