// ----------------------------------------------------------------------------
// <copyright file="ListDataReader`1.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Database
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Wrapper of IDataReader in List collection.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <remarks>
    /// Implementation copied from http://andreyzavadskiy.com/2017/07/03/converting-list-to-idatareader/.
    /// </remarks>
    public class ListDataReader<T> : IDataReader
        where T : class
    {
        #region Fields

        /// <summary>
        /// The iterator.
        /// </summary>
        private readonly IEnumerator<T> iterator;

        /// <summary>
        /// The property collection.
        /// </summary>
        private readonly List<PropertyInfo> properties = new List<PropertyInfo>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListDataReader{T}"/> class.
        /// </summary>
        /// <param name="list">The source collection.</param>
        public ListDataReader(IEnumerable<T> list)
        {
            this.iterator = list.GetEnumerator();

            this.properties.AddRange(
                typeof(T).GetProperties(
                    BindingFlags.GetProperty |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.DeclaredOnly));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the depth level.
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// Gets the field count.
        /// </summary>
        public int FieldCount
        {
            get { return this.properties.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether dertermines if the reader is closed.
        /// </summary>
        public bool IsClosed { get; }

        /// <summary>
        /// Gets the number of records affected.
        /// </summary>
        public int RecordsAffected { get; }

        /// <summary>
        /// The indexer by position.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The current object.</returns>
        object IDataRecord.this[int i]
        {
            get { return this.GetValue(i); }
        }

        /// <summary>
        /// The indexer by name.
        /// </summary>
        /// <param name="name">The fields name.</param>
        /// <returns>The current object.</returns>
        object IDataRecord.this[string name]
        {
            get { return this.GetValue(this.GetOrdinal(name)); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Closes the reader.
        /// </summary>
        public void Close()
        {
            this.iterator.Dispose();
        }

        /// <summary>
        /// The dispose method.
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// Gets a boolean.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The boolean.</returns>
        public bool GetBoolean(int i)
        {
            return (bool)this.GetValue(i);
        }

        /// <summary>
        /// Gets a byte.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The byte.</returns>
        public byte GetByte(int i)
        {
            return (byte)this.GetValue(i);
        }

        /// <summary>
        /// Gets the bytes number.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <param name="fieldOffset">the field offset.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferoffset">The buffer offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>The bytes number.</returns>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a char.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The char.</returns>
        public char GetChar(int i)
        {
            return (char)this.GetValue(i);
        }

        /// <summary>
        /// Gets the chars number.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <param name="fieldoffset">the field offset.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferoffset">The buffer offset.</param>
        /// <param name="length">The length.</param>
        /// <returns>The char number.</returns>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The data reader.</returns>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the datatype name.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The datatype name.</returns>
        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a datetime.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The datetime.</returns>
        public DateTime GetDateTime(int i)
        {
            return (DateTime)this.GetValue(i);
        }

        /// <summary>
        /// Gets a decimal.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The decimal.</returns>
        public decimal GetDecimal(int i)
        {
            return (decimal)this.GetValue(i);
        }

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The double.</returns>
        public double GetDouble(int i)
        {
            return (double)this.GetValue(i);
        }

        /// <summary>
        /// Gets the field type.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The field type.</returns>
        public Type GetFieldType(int i)
        {
            return this.properties[i].PropertyType;
        }

        /// <summary>
        /// Gets the float.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The float.</returns>
        public float GetFloat(int i)
        {
            return (float)this.GetValue(i);
        }

        /// <summary>
        /// Gets the guid.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The guid.</returns>
        public Guid GetGuid(int i)
        {
            return (Guid)this.GetValue(i);
        }

        /// <summary>
        /// Gets an int16.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The int16.</returns>
        public short GetInt16(int i)
        {
            return (short)this.GetValue(i);
        }

        /// <summary>
        /// Gets an int32.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The int32.</returns>
        public int GetInt32(int i)
        {
            return (int)this.GetValue(i);
        }

        /// <summary>
        /// Gets an int64.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The int64.</returns>
        public long GetInt64(int i)
        {
            return (long)this.GetValue(i);
        }

        /// <summary>
        /// The property name of the current property.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The property name.</returns>
        public string GetName(int i)
        {
            return this.properties[i].Name;
        }

        /// <summary>
        /// Gets the ordinal by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The ordinal.</returns>
        public int GetOrdinal(string name)
        {
            var index = this.properties.FindIndex(p => p.Name == name);

            return index;
        }

        /// <summary>
        /// Obtains the table schema.
        /// </summary>
        /// <returns>The datatable.</returns>
        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a string.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>the string.</returns>
        public string GetString(int i)
        {
            return (string)this.GetValue(i);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns>The value.</returns>
        public object GetValue(int i)
        {
            return this.properties[i].GetValue(this.iterator.Current, null);
        }

        /// <summary>
        /// Gets the value number..
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The value number.</returns>
        public int GetValues(object[] values)
        {
            int numberOfCopiedValues = Math.Max(this.properties.Count, values.Length);

            for (int i = 0; i < numberOfCopiedValues; i++)
            {
                values[i] = this.GetValue(i);
            }

            return numberOfCopiedValues;
        }

        /// <summary>
        /// Dertermines if the element is null.
        /// </summary>
        /// <param name="i">The element position into the record.</param>
        /// <returns><b>True</b> if the element is null,<b>False</b> in otherwise.</returns>
        public bool IsDBNull(int i)
        {
            return this.GetValue(i) == null;
        }

        /// <summary>
        /// Gets to next result.
        /// </summary>
        /// <returns><b>True</b> if exist next value, <b>False</b> in otherwise.</returns>
        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read an other record.
        /// </summary>
        /// <returns><b>True</b> if exist next record, <b>False</b> in otherwise.</returns>
        public bool Read()
        {
            return this.iterator.MoveNext();
        }

        #endregion Methods
    }
}