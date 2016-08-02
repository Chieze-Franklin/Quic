using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Base class for checkable elements
/// </summary>
public abstract class CheckElement : InputElement
{
    /// <summary>
    /// Gets or sets a value to determine if the element is checked
    /// </summary>
    public bool Checked { get; set; }
}