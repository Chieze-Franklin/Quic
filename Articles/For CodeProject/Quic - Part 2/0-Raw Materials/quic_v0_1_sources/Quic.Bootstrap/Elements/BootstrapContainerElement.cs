using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Base class for all bootstrap container elements
/// </summary>
public abstract class BootstrapContainerElement : BootstrapElement
{
    public BootstrapContainerElement() 
    {
        IsContainer = true;
        IsFormControl = false;
    }
}
