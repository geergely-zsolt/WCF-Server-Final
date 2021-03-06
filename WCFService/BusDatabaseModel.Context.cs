﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BusDBEntities : DbContext
    {
        public BusDBEntities()
            : base("name=BusDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        //public virtual DbSet<Bus> Bus { get; set; }
        public virtual DbSet<bus> Bus { get; set; }
        public virtual DbSet<bus_data> BusData { get; set; }
        public virtual DbSet<bus_driver_data> BusDriverData { get; set; }
        public virtual DbSet<bus_position> BusPosition { get; set; }
        public virtual DbSet<line_trace> BusTrace { get; set; }
        public virtual DbSet<car_data> CarData { get; set; }
        public virtual DbSet<line> Line { get; set; }
        public virtual DbSet<MeasuredData> MeasuredData { get; set; }
        public virtual DbSet<bus_measurement> Measurement { get; set; }
        public virtual DbSet<bus_reference> Reference { get; set; }
        public virtual DbSet<bus_simulation> SimulatedBus { get; set; }
        //public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<station> Station { get; set; }
        public virtual DbSet<timetable> Timetable { get; set; }
    
        public virtual int arrivalTime(string busID, Nullable<int> stationID, ObjectParameter time_needed)
        {
            var busIDParameter = busID != null ?
                new ObjectParameter("busID", busID) :
                new ObjectParameter("busID", typeof(string));
    
            var stationIDParameter = stationID.HasValue ?
                new ObjectParameter("stationID", stationID) :
                new ObjectParameter("stationID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("arrivalTime", busIDParameter, stationIDParameter, time_needed);
        }
    
        public virtual int simulateBus(string busID)
        {
            var busIDParameter = busID != null ?
                new ObjectParameter("busID", busID) :
                new ObjectParameter("busID", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("simulateBus", busIDParameter);
        }
    
        //public virtual ObjectResult<stationSchedule_Result> stationSchedule(Nullable<int> stationId)
        //{
        //    var stationIdParameter = stationId.HasValue ?
        //        new ObjectParameter("stationId", stationId) :
        //        new ObjectParameter("stationId", typeof(int));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<stationSchedule_Result>("stationSchedule", stationIdParameter);
        //}
    }
}
