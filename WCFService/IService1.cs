﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<StationType> GetStationList();


        [OperationContract]
        List<LineType> GetLineList();


        [OperationContract]
        List<LineTraceType> GetLineTraceList();


        [OperationContract]
        List<StationOnLineType> GetStationOnLineList();


        [OperationContract]
        List<ArrivalTimeType> GetArrivalTimeList(int StationID);


        [OperationContract]
        List<CourseDataType> GetCourseDataList();


        [OperationContract]
        List<bus> GetBusList();


        [OperationContract]
        string GetData(int value);


        [OperationContract]
        List<TimetableType> GetTimetableList();


        [OperationContract]
        List<TimetableType> GetTimetableList44();


        //[OperationContract(IsOneWay = true)]
        //void PostBusInformation(PositionType busInfo);


        [OperationContract(IsOneWay = true)]
        void PostBusMeasurement(MeasurementType busInfo);


        [OperationContract]
        string Synchronization();
    }



    // A megállókat összefoglaló típus
    [DataContract]
    public class StationType
    {
        public StationType(station stationInstance)
        {
            id = stationInstance.id;
            station_name = stationInstance.station_name;
            latitude = stationInstance.latitude;
            longitude = stationInstance.longitude;
        }

        [DataMember]
        public int id;

        [DataMember]
        public string station_name;

        [DataMember]
        public double latitude;

        [DataMember]
        public double longitude;
    }



    // A vonalakat összefoglaló típus.
    [DataContract]
    public class LineType
    {
        public LineType(String pId, String pRouteName, short pStartStationId, String pStartStationName, short pEndStationId, String pEndStationName)
        {
            id = pId;
            routeName = pRouteName;
            startStationId = pStartStationId;
            startStationName = pStartStationName;
            endStationId = pEndStationId;
            endStationName = pEndStationName;
        }

        [DataMember]
        public string id;

        [DataMember]
        public string routeName;

        [DataMember]
        public short startStationId;

        [DataMember]
        public string startStationName;

        [DataMember]
        public short endStationId;

        [DataMember]
        public string endStationName;
    }



    [DataContract]
    public class PointType
    {
        public PointType(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }
    }


    //[DataContract]
    //public class TraceType
    //{
    //    [DataMember]
    //    public string BusId;

    //    [DataMember]
    //    public List<Point> Points;
    //}


    [DataContract]
    public class LineFollowerType
    {
        [DataMember]
        public List<PointType> pointList;

        [DataMember]
        public int counter = 0;

        [DataMember]
        public int order_number;
    }



    [DataContract]
    public class LineTraceDataType
    {
        public LineTraceDataType(double pLatitude, double pLongitude, byte pPirection, short pOrderNumber)
        {
            latitude = pLatitude;
            longitude = pLongitude;
            direction = pPirection;
            order_number = pOrderNumber;
        }

        [DataMember]
        public double latitude;

        [DataMember]
        public double longitude;

        [DataMember]
        public byte direction;

        [DataMember]
        public int order_number;
    }


    [DataContract]
    public class LineTraceType
    {
        [DataMember]
        public string line_id;

        [DataMember]
        public List<LineTraceDataType> LineTraceData;
    }



    //Every busstop on a line
    //[DataContract]
    //public class LinePair
    //{
    //    public LinePair(int stationID, int stationNr)
    //    {
    //        StationID = stationID;
    //        StationNr = stationNr;
    //    }
    //    [DataMember]
    //    public int StationID;

    //    [DataMember]
    //    public int StationNr;
    //}



    ////Line details
    //[DataContract]
    //public class LineType
    //{
    //    [DataMember]
    //    public string LineID;

    //    [DataMember]
    //    public List<LinePair> Stations;

    //}


    //Every busstop on a line
    [DataContract]
    public class StationOnLineDataType
    {
        public StationOnLineDataType(byte pStationId, byte pDirection, byte pOrderNumber)
        {
            station_id = pStationId;
            direction = pDirection;
            order_number = pOrderNumber;
        }
        [DataMember]
        public byte station_id;

        [DataMember]
        public byte direction;

        [DataMember]
        public byte order_number;
    }



    //Line details
    [DataContract]
    public class StationOnLineType
    {
        [DataMember]
        public string line_id;

        [DataMember]
        public List<StationOnLineDataType> LineData;
    }



    [DataContract]
    public class ArrivalTimeType
    {
        [DataMember]
        public string BusID { get; set; }

        [DataMember]
        public TimeSpan RequiredTime { get; set; }
    }


    //Bus details for tracking from BusData
    [DataContract]
    public class BusDataType
    {
        public BusDataType(short busId, string courseId, string lineId, int direction, double latitude, double longitude, System.DateTime measurementTimestamp)
        {
            BusId = busId;
            CourseId = courseId;
            LineId = lineId;
            Direction = direction;
            Latitude = latitude;
            Longitude = longitude;
            Measurement_Timestamp = measurementTimestamp;
        }

        [DataMember]
        public short BusId { get; set; }

        [DataMember]
        public string CourseId { get; set; }

        [DataMember]
        public string LineId { get; set; }

        [DataMember]
        public int Direction { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public System.DateTime Measurement_Timestamp { get; set; }
    }



    // A járatok adatait leíró osztály.
    [DataContract]
    public class CourseDataType
    {
        public CourseDataType(string courseId, string lineId, int direction, double latitude, double longitude, System.DateTime measurementTimestamp)
        {
            CourseId = courseId;
            LineId = lineId;
            Direction = direction;
            Latitude = latitude;
            Longitude = longitude;
            Measurement_Timestamp = measurementTimestamp;
        }

        [DataMember]
        public string CourseId { get; set; }

        [DataMember]
        public string LineId { get; set; }

        [DataMember]
        public int Direction { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public System.DateTime Measurement_Timestamp { get; set; }
    }



    //[DataContract]
    //public class TraceType
    //{
    //    public TraceType(string BusTraceInstance, List<Point> points)
    //    {
    //        BusId = BusTraceInstance;
    //        Points = points;
    //    }

    //    [DataMember]
    //    public string BusId { get; set; }
    //    [DataMember]
    //    public List<Point> Points;
    //}


    [DataContract]
    public class MeasurementType
    {
        public MeasurementType(BusDataType instance)
        {
            BusId = instance.BusId;
            BusName = instance.LineId;
            Actual_Latitude = instance.Latitude;
            Actual_Longitude = instance.Longitude;
        }

        [DataMember]
        public short BusId { get; set; }

        [DataMember]
        public string BusName { get; set; }

        [DataMember]
        public double Actual_Latitude { get; set; }

        [DataMember]
        public double Actual_Longitude { get; set; }

        [DataMember]
        public double Position_Accuracy { get; set; }

        [DataMember]
        public double Actual_Speed { get; set; }
        [DataMember]
        public double Speed_Accuracy { get; set; }
        [DataMember]
        public int Direction { get; set; }
        [DataMember]
        public List<double> Acceleration { get; set; }
        [DataMember]
        public List<double> Gyroscope { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
    }


    [DataContract]
    public class PositionType
    {
        public PositionType(string insBusName, bus_data busInstance)
        {
            //BusId = busInstance.bus_id;
            BusName = insBusName;
            Actual_Latitude = busInstance.latitude;
            Actual_Longitude = busInstance.longitude;
        }

        [DataMember]
        public short BusId { get; set; }

        [DataMember]
        public string BusName { get; set; }

        [DataMember]
        public double Actual_Latitude { get; set; }

        [DataMember]
        public double Actual_Longitude { get; set; }
    }



    [DataContract]
    public class TimetableType
    {
        public TimetableType(timetable timetableInstance)
        {
            BusNr = timetableInstance.line_id.ToString();
            StationId = timetableInstance.station_id;
            StartTime = timetableInstance.start_time.ToString();
        }


        [DataMember]
        public string BusNr { get; set; }

        [DataMember]
        public int StationId { get; set; }
        
        [DataMember]
        public string StartTime { get; set; }
    }
}