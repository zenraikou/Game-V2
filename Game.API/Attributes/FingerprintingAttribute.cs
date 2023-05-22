using Microsoft.AspNetCore.Mvc.Filters;

namespace Game.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class FingerprintingAttribute : Attribute, IFilterMetadata { }
