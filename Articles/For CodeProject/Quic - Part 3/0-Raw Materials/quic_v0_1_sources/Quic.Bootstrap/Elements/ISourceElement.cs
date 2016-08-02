using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// To be implemented by a UI element that can derive its source from something.
/// </summary>
public interface ISourceElement
{
    /// <summary>
    /// Gets or sets the source of the element
    /// </summary>
    string Source { get; set; }
}