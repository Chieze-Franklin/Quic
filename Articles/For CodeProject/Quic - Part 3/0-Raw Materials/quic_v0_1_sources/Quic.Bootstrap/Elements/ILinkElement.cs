using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// To be implemented by a UI element that can link to something.
/// </summary>
public interface ILinkElement
{
    /// <summary>
    /// Gets or sets the link of the element
    /// </summary>
    string Link { get; set; }
}