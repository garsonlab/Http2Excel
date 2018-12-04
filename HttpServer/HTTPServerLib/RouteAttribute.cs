using System;

namespace HTTPServerLib
{
    [AttributeUsage(AttributeTargets.Method)]
    class RouteAttribute:Attribute
    {
        public RouteMethod Method { get; set; }
        public string RoutePath { get; set; }
    }
}
