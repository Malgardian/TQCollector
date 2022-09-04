using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TQCollector
{
    /// <summary>
    /// The Hashtable class is not directly serializable, so this
    /// class inherits from and extends the Generic List class to
    /// provide a simple way to serialize a hashtable.
    /// 
    /// This class can also be used directly, without ever instantiating
    /// a Hashtable.
    /// </summary>
    [Serializable()]
    public class OrderedTable : List<NameValuePair>
    {
        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public OrderedTable()
        {

        }

        /// <summary>
        /// Constructor that accepts a Hashtable and
        /// enumerates through it to populate the
        /// OrderedTable.
        /// </summary>
        /// <param name="table"></param>
        /*public OrderedTable(Hashtable table)
        {
            IDictionaryEnumerator enumerator = table.GetEnumerator();

            while (enumerator.MoveNext())
                this.AddIfNotExisting(Convert.ToString(enumerator.Key), 
                                      Convert.ToString(enumerator.Value));
        }*/

        /// <summary>
        /// Returns a Hashtable that represents
        /// the current OrderedTable (all data that
        /// is contained in the OrderedTable will be
        /// contained in the returned HashTable)
        /// </summary>
        public Hashtable Hashtable
        {
            get
            {
                Hashtable table = new Hashtable();

                foreach (NameValuePair pair in this)
                    if (!table.ContainsKey(pair.Name))
                        table.Add(pair.Name, pair.Value);

                return table;
            }
        }

        /// <summary>
        /// Adds an object to the table.
        /// </summary>
        /// <param name="key">Key to use for the object.</param>
        /// <param name="value">Value of the object</param>
        public void Add(string key, string value)
        {
            base.Add(new NameValuePair(key, value));
        }

        /// <summary>
        /// Adds an object if the given key doesn't exist
        /// </summary>
        /// <param name="key">Key to use for the object</param>
        /// <param name="value">Value of the object</param>
        public void AddIfNotExisting(string key, string value)
        {
            if (!Exists(key))
                this.Add(new NameValuePair(key, value));
        }

        /// <summary>
        /// Determines if a key exists in the table.
        /// </summary>
        /// <param name="key">Key to search for.</param>
        /// <returns>True if the key exists, false if it does not.</returns>
        public bool Exists(string key)
        {
            NameValuePair pair = this.Find(
                delegate(NameValuePair nvp)
                {
                    return nvp.Name == key;
                });

            if (pair == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Indexer to return an object associated with the given key.
        /// If the key doesn't exist, this returns null.
        /// </summary>
        /// <param name="key">Key to access.</param>
        /// <returns>The object associated with the given key.</returns>
        public string this[string key]
        {
            get
            {
                string valueObject = "...";

                NameValuePair pair = this.Find(
                    delegate(NameValuePair nvp)
                    {
                        return nvp.Name == key;
                    });

                if (pair != null)
                    valueObject = pair.Value;

                return valueObject;
            }
            set
            {
                NameValuePair pair = this.Find(
                    delegate(NameValuePair nvp)
                    {
                        return nvp.Name == key;
                    });

                if (pair != null)
                    pair.Value = value;
            }
        }

        /// <summary>
        /// Removes an object from the table.
        /// </summary>
        /// <param name="key">Key to remove.</param>
        public void Remove(string key)
        {
            foreach (NameValuePair pair in this)
            {
                if (pair.Name == key)
                {
                    base.Remove(pair);
                    break;
                }
            }
        }
    }
}
