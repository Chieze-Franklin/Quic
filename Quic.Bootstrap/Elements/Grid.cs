﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic;

/// <summary>
/// Represents a container that is made up of rows and columns.
/// </summary>
public class Grid : BootstrapContainerElement
{
    public Grid()
    {
        CssClasses.Add("container");
    }

    public override void Render()
    {
        if (this.Document.OutputFile is TextFile)
        {
            TextFile outputFile = (TextFile)this.Document.OutputFile;

            //opening tag
            outputFile.Write("<div");
            outputFile.Write(EmitID());
            outputFile.Write(EmitClasses());

            //custom properties
            outputFile.Write(EmitCustomProperties());

            //opening tag 2
            outputFile.WriteLine(">");

            //child elements
            foreach (var element in Elements)
            {
                element.BeginRender();
            }
            outputFile.WriteLine();

            //closing tag
            outputFile.WriteLine("</div>");
        }
    }
}