using System;
using System.Collections.Generic;
using System.Text;

namespace TQCollector
{
    /// <summary>
    /// Simple class to hold name/value pairs.
    /// </summary>
    [Serializable()]
    public class NameValuePair
    {
        private string _name;
        private string _value;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NameValuePair()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name - must be a string</param>
        /// <param name="value">Value - can be any object</param>
        public NameValuePair(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Name of the pair
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Value of the pair
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
