using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quic.Utils;

namespace Quic
{
    /// <summary>
    /// Represents a directory to be outputed after a file translation.
    /// </summary>
    public class OutputDirectory : OutputObject
    {
        List<OutputObject> outputs;

        /// <summary>
        /// Creates a new instance of Quic.OutputDirectory
        /// </summary>
        /// <param name="name"></param>
        public OutputDirectory(string name) 
            : base(name)
        {
            outputs = new List<OutputObject>();
        }

        /// <summary>
        /// Adds an output object to the directory with the option of overriding an existing object with the same name.
        /// Returns true if the object was added.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="canOverride"></param>
        /// <returns>Returns true if the object was added.</returns>
        public bool Add(OutputObject output, bool canOverride) 
        {
            if (output == null)
                throw new NullReferenceException("File cannot be null.");

            if (Contains(output))
            {
                //throw new Exception(string.Format("A file with the name '{0}' already exists in this directory.", file.Name));

                if (canOverride)
                {
                    //remove (skip) file(s) with the same name
                    outputs = outputs.SkipWhile(f => f.Name.Equals(output.Name, StringComparison.CurrentCultureIgnoreCase)).ToList();
                }
                else
                    return false;
            }

            outputs.Add(output);
            output.ParentDirectory = this;
            return true;
        }
        /// <summary>
        /// Gets a member object with the specified name.
        /// Returns null if no such object exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public OutputObject Get(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NullReferenceException("File name cannot be null or empty.");

            return outputs.FirstOrDefault(f => f.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
        /// <summary>
        /// Saves the directory and its contents.
        /// </summary>
        /// <param name="canOverride"></param>
        public override void Commit(bool canOverride) 
        {
            Commit(canOverride, canOverride);
        }
        /// <summary>
        /// Saves the directory and its contents, with the opportunity to specify different override parameters for files and directories.
        /// </summary>
        /// <param name="canOverrideDirectories"></param>
        /// <param name="canOverrideFiles"></param>
        public void Commit(bool canOverrideDirectories, bool canOverrideFiles)
        {
            //save directory
            FileSystemServices.CreateDirectory(FullName, canOverrideDirectories);

            //save files and subdirectories
            foreach (var file in outputs)
            {
                if (file is OutputDirectory)
                    ((OutputDirectory)file).Commit(canOverrideDirectories, canOverrideFiles);
                else
                    file.Commit(canOverrideFiles);
            }
        }
        /// <summary>
        /// Returns true if the specified object exists in the directory.
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool Contains(OutputObject output)
        {
            if (output == null)
                throw new NullReferenceException("File cannot be null.");

            return outputs.Any(f => f.Name.Equals(output.Name, StringComparison.CurrentCultureIgnoreCase));
        }
        /// <summary>
        /// Returns true if an object with the specified name exists in the directory.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NullReferenceException("File name cannot be null or empty.");

            return outputs.Any(f => f.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
