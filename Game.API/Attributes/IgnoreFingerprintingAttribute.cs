using Microsoft.AspNetCore.Mvc.Filters;

namespace Game.API.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class IgnoreFingerprintingAttribute : Attribute, IFilterMetadata { }
