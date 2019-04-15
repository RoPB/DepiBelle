using System;
using System.Collections.Generic;
using Plugin.CloudFirestore;

namespace DepiBelle.Models
{
    public enum QueryWhereEnum { Equals, GreaterThan, GreaterThanOrEquals, LessThan, LessThanOrEquals }

    public class QueryOrderBy
    {
        public string OrderByField { get; set; }
        public bool IsDescending { get; set; }
    }

    public class QueryWhere
    {
        public string WhereField { get; set; }
        public object ValueField { get; set; }
        public QueryWhereEnum Type { get; set; } = QueryWhereEnum.Equals;
    }

    public class QueryLike
    {
        public string LikeField { get; set; }
        public object LikeValue { get; set; }
    }

}
