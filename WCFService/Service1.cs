﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using System.Threading;

namespace WCFService
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service1 : IService1
    {
        List<bus> BusList = new List<bus>();
        readonly bus_dbEntities BusDBEntities = new bus_dbEntities();
        readonly List<BusDataType> BusInfirmationList = new List<BusDataType>();
        readonly List<StationOnLineType> LineList = new List<StationOnLineType>();
        readonly List<StationType> StationList = new List<StationType>();
        readonly List<TimetableType> TimetableList = new List<TimetableType>();
        readonly List<LineTraceType> BusTraceList = new List<LineTraceType>();
        List<BusDataType> halii = new List<BusDataType>();






        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<StationType> GetStationList()
        {
            var vStationList = BusDBEntities.station.ToList();
            StationList.Clear();

            foreach (var i in vStationList)
            {
                StationList.Add(new StationType(i));
            }
            return StationList;
        }



        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<LineTraceType> GetLineTraceList()
        {
            //Lekérdezzük az egyedi BusId-kat.
            var vDistinctLineIdList = (from myLine in BusDBEntities.line
                                      orderby myLine.id
                                      select myLine.id).ToList();

            BusTraceList.Clear();

            List<LineTraceDataType> vPointList = new List<LineTraceDataType>();

            //Sorra vesszük az összes kapott BusId-t.
            foreach (var i in vDistinctLineIdList)
            {
                var vTraceList = new LineTraceType
                {
                    LineTraceData = new List<LineTraceDataType>()
                };

                //Megkeressük az adott BusId-hoz tartozó koordinátákat.
                var vPointsList = BusDBEntities.line_trace.Where(e => e.line_id == i).
                    Select(e => new { e.latitude, e.longitude, e.direction, e.order_number }).ToList();

                foreach (var j in vPointsList)
                {
                    //A kapott koordinátákat párban hozzáadjuk az TraceType típushoz.
                    vTraceList.LineTraceData.Add(new LineTraceDataType(j.latitude, j.longitude, j.direction, j.order_number));
                }

                //Beállítjuk a TraceType típus BusId mezőjét.
                vTraceList.line_id = i;
                BusTraceList.Add(vTraceList);
            }

            return BusTraceList;
        }


        
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<StationOnLineType> GetStationOnLineList()
        {
            //Lekérdezzük a különböző vonalak LineID paraméterét.
            var vDistinctLineIdList = BusDBEntities.line.Select(e => e.id).ToList();

            LineList.Clear();
            foreach (var i in vDistinctLineIdList)
            {
                var vLineList = new StationOnLineType
                {
                    LineData = new List<StationOnLineDataType>()
                };

                //Lekérdezzük az összes LineID-hez tartozó StationID és StationNr adatokat.
                var vStationList = BusDBEntities.station_on_line.Where(e => e.line_id == i).Select(e => new { e.station_id, e.direction, e.order_number }).ToList();
                foreach (var j in vStationList)
                {
                    //Egy LineID-hez több StationID és StationNr tartozik.
                    vLineList.LineData.Add(new StationOnLineDataType(j.station_id, j.direction, j.order_number));
                }
                //Végül a LineType adatunkhoz hozzáadjuk a LineID-t is.
                vLineList.line_id = i;
                LineList.Add(vLineList);
            }

            return LineList;
        }



        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<TimetableType> GetTimetableList()
        {
            TimetableList.Clear();
            List<timetable> vTimetableList;
            DayOfWeek day = DateTime.Now.DayOfWeek;
            if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
            {
                vTimetableList = BusDBEntities.timetable.Where(x => x.weekday_flag == 0).ToList();
            }
            else
            {
                vTimetableList = BusDBEntities.timetable.Where(x => x.weekday_flag == 1).ToList();
            }
            foreach (var i in vTimetableList)
            {
                TimetableList.Add(new TimetableType(i));
            }
            return TimetableList;
        }



        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<TimetableType> GetTimetableList44()
        {
            TimetableList.Clear();
            List<timetable> vTimetableList;
            DayOfWeek day = DateTime.Now.DayOfWeek;
            if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
            {
                vTimetableList = BusDBEntities.timetable.Where(x => x.weekday_flag == 0).ToList();
            }
            else
            {
                vTimetableList = BusDBEntities.timetable.Where(e => e.weekday_flag == 1 & e.line_id == "44").ToList();
            }
            foreach (var i in vTimetableList)
            {
                TimetableList.Add(new TimetableType(i));
            }
            return TimetableList;
        }



        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<bus> GetBusList()
        {
            BusList = (from bus in BusDBEntities.bus
                       select bus).ToList();

            return BusList;
        }



        // Ez felel azért, hogy hol járnak a buszok.
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<BusDataType> GetBusInformationList()
        {

            var vBusInformationList = (from bus in BusDBEntities.bus
                                       join busData in BusDBEntities.bus_data
                                       on bus.id equals busData.bus_id
                                       //where (System.DateTime.Now - busData.Measurement_Timestamp).TotalSeconds <= 2
                                       where System.Data.Entity.DbFunctions.DiffSeconds(busData.measurement_timestamp, System.DateTime.Now) < 6
                                       select new
                                       {
                                           busData.bus_id,
                                           bus.line_id,
                                           busData.latitude,
                                           busData.longitude,
                                           busData.measurement_timestamp
                                       }).ToList();

            foreach(var i in vBusInformationList)
            {
                BusInfirmationList.Clear();
                BusInfirmationList.Add(new BusDataType(i.bus_id, i.line_id, i.latitude, i.longitude, i.measurement_timestamp));
            }

            return BusInfirmationList;
        }



        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void PostBusMeasurement(MeasurementType busInfo)
        {
            //Enviroment.CurrentDirectory
            string path = Path.Combine(@"D: \university", "MyTest2.txt");
            // This text is added only once to the file.
            StreamWriter sw;
            if (!File.Exists(path))
            {
                // Create a file to write to.
                sw = File.CreateText(path);
                sw.WriteLine("Data arrived at: " + DateTime.Now.ToString());
                sw.WriteLine(busInfo.BusId.ToString() + " " + busInfo.Actual_Latitude.ToString() + " " + busInfo.Actual_Longitude.ToString());

            }
            else
            {
                sw = File.AppendText(path);
                sw.WriteLine("Data arrived at: " + DateTime.Now.ToString());
                sw.WriteLine(busInfo.BusId.ToString() + " " + busInfo.Actual_Latitude.ToString() + " " + busInfo.Actual_Longitude.ToString() + " " + busInfo.Timestamp.ToString());

            }
           

            var parameters = new List<SqlParameter> {
                //if this works do the same for normal bus data
                new SqlParameter("@busid", busInfo.BusId),
                new SqlParameter("@lat", busInfo.Actual_Latitude),
                new SqlParameter("@long", busInfo.Actual_Longitude),
            };

            var query = @"SELECT COUNT(Id)
                            FROM BusTrace
                            WHERE dbo.calcDistanceKM(Latitude, @lat, Longitude, @long) <= 0.08 and BusId = @busid;";

            int matches = BusDBEntities.Database.SqlQuery<int>(query, parameters.ToArray()).First();

            if (matches >= 1)
            {
                sw.WriteLine("Data saved having matching points:" + matches.ToString());
                //Storing Data
                bus_measurement trace = new bus_measurement
                {
                    bus_id = busInfo.BusId,
                    latitude = busInfo.Actual_Latitude,
                    longitude = busInfo.Actual_Longitude,
                    position_accuracy = busInfo.Position_Accuracy,
                    speed = busInfo.Actual_Speed,
                    speed_accuracy = busInfo.Speed_Accuracy,
                    direction = busInfo.Direction,
                    accel_x = busInfo.Acceleration[0],
                    accel_y = busInfo.Acceleration[1],
                    accel_z = busInfo.Acceleration[2],
                    gyro_x = busInfo.Gyroscope[0],
                    gyro_y = busInfo.Gyroscope[1],
                    gyro_z = busInfo.Gyroscope[2]       //2019-09-23 12:59:45
                };
                DateTime.TryParseExact(busInfo.Timestamp.ToString(), "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out DateTime temp);
                trace.measurement_timestamp = temp;
                BusDBEntities.bus_measurement.Add(trace);
                BusDBEntities.SaveChanges();
            }
            else
            {
                sw.WriteLine("Data not saved:" + matches.ToString());
            }
            sw.Close();
        }



        public string GetData(int value)
        {
            WebOperationContext.Current.OutgoingResponse.Headers
                .Add("Access-Control-Allow-Origin", "*");
            //identify preflight request and add extra headers
            if (WebOperationContext.Current.IncomingRequest.Method == "OPTIONS")
            {
                WebOperationContext.Current.OutgoingResponse.Headers
                    .Add("Access-Control-Allow-Methods", "POST, OPTIONS, GET");
                WebOperationContext.Current.OutgoingResponse.Headers
                    .Add("Access-Control-Allow-Headers",
                         "Content-Type, Accept, Authorization, x-requested-with");
                return null;
            }
            return string.Format("You entered: {0}", value);
        }



        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        //public List<ArrivalTimeType> GetArrivalTimeList(int StationID)
        //{
        //    var idParam = new SqlParameter { ParameterName = "@stationID", Value = StationID };
        //    List<ArrivalTimeType> array = BusDBEntities.Database.SqlQuery<ArrivalTimeType>("stationSchedule @stationID", idParam).ToList();
        //    return array;
        //}
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<ArrivalTimeType> GetArrivalTimeList(int StationID)
        {
            var idParam = new SqlParameter { ParameterName = "@stationID", Value = StationID };
            List<ArrivalTimeType> array = BusDBEntities.Database.SqlQuery<ArrivalTimeType>("station_schedule @stationID", idParam).ToList();
            return array;
        }



        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string Synchronization()
        {
            var vDateTimeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return vDateTimeStr;
        }
    }
}