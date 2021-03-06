﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CivilMapServices
{
    /* -------------------------------Request Model------------------------------ */
    [DataContractAttribute()]
    public class UserData
    {
        [DataMemberAttribute()]
        public string username;
        [DataMemberAttribute()]
        public string password;
        [DataMemberAttribute()]
        public string token;
    }


    [DataContractAttribute()]
    public class RequestPoint
    {
        [DataMemberAttribute()]
        public string type;
        [DataMemberAttribute()]
        public string date_range;
        [DataMemberAttribute()]
        public string date_start;
        [DataMemberAttribute()]
        public string date_end;
        [DataMemberAttribute()]
        public string beat;
    }

    [DataContractAttribute()]
    public class RequestPointByRadius
    {
        [DataMemberAttribute()]
        public string type;
        [DataMemberAttribute()]
        public string date_range;
        [DataMemberAttribute()]
        public string date_start;
        [DataMemberAttribute()]
        public string date_end;
        [DataMemberAttribute()]
        public double radius;
        [DataMemberAttribute()]
        public double center_longitude;
        [DataMemberAttribute()]
        public double center_latitude;
    }

    [DataContractAttribute()]
    public class UploadedPoints
    {
        [DataMember(Order = 0, IsRequired = true)]
        public List<CustomizedPoint> points { get; set; }
    }

    [DataContract]
    public class CustomizedPoint {
        [DataMember]
        public string longitude { get; set; }
        [DataMember]
        public string latitude { get; set; }
    }

    [DataContractAttribute()]
    public class Polygon
    {
        [DataMemberAttribute()]
        public string type;
        [DataMemberAttribute()]
        public string option;
        [DataMemberAttribute()]
        public string area;
        [DataMemberAttribute()]
        public string district;
        [DataMemberAttribute()]
        public string sector;
        [DataMemberAttribute()]
        public string beat;
    }
    /* ------------------------------Response Model------------------------------ */
    public class TokenValidationStatus
    {     
        public bool Expired { get; set; }
        public bool WrongUser { get; set; }
    }

    public class PointSet {
        public PointSet() {
            this.Points = new List<Point>();
        }
        public List<Point> Points { get; set; }
        public string error;
        public int count { get; set; }
    }

    public class Point {    
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string date { get; set; }
        public double distance { get; set; }
    }
}