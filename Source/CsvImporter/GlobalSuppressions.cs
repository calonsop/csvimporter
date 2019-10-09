// ----------------------------------------------------------------------------
// <copyright file="GlobalSuppressions.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

#region DocumentationRules

////[assembly: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]

#endregion DocumentationRules

#region ReadabilityRules

//// [assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:PrefixLocalCallsWithThis", Justification = "Reviewed.")]
[assembly: SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:DoNotUseRegions", Justification = "Reviewed.")]

#endregion ReadabilityRules

#region MaintainabilityRules

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1413:UseTrailingCommasInMultiLineInitializers", Justification = "Reviewed.")]

#endregion MaintainabilityRules

#region SonarQube

[assembly: SuppressMessage("SonarQube", "S1075:Refactor your code not to use hardcoded absolute paths or URIs", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S1118:Utility classes should not have public constructors", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S1210:When implementing IComparable<T>, you should also override Equals, ==, !=", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S1699:Remove this call from a constructor to the overridable '...' method", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S2365:Refactor '...' into a method, properties should not copy collections", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S2386:Use an immutable collection or reduce the accessibility of this field", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S2436:Reduce the number of generic parameters in the '...' class to no more than the 2 authorized", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S3343:Caller information parameters should come at the end of the parameter list", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S3875:Remove this overload of 'operator =='", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S3881:Fix this implementation of 'IDisposable' to conform to the dispose pattern", Justification = "Reviewed.")]
[assembly: SuppressMessage("SonarQube", "S3887:Use an immutable collection or reduce the accessibility of this field", Justification = "Reviewed.")]

#endregion SonarQube