/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2006-2012, EntitySpaces, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the EntitySpaces, LLC nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL EntitySpaces, LLC BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-------------------------------------------------------------------------------
*/

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

#if(WCF || SILVERLIGHT)
namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// This class can be used to serialize and deserialize the server side proxy stubs
    /// to the client side proxy stubs and back again
    /// </summary>
    /// <example>
    /// <code>
    /// // Select 3 Employees
    /// EmployeesCollection coll = new EmployeesCollection();
    /// coll.Query.es.Top = 3;
    /// if (coll.Query.Load())
    /// {
    ///    // Create Our Proxy Stubb
    ///    BusinessObjects.EmployeesCollectionProxyStub server = new BusinessObjects.EmployeesCollectionProxyStub(coll);
    /// 
    ///    // Serialize it into a string and return this string to Silverlight
    ///    string xml = esDataContractSerializer.ToXml(server);
    /// 
    ///    // Deserialize the string above into our Client side proxy
    ///    Proxies.EmployeesCollectionProxyStub client = esDataContractSerializer.FromXml(xml, typeof(Proxies.EmployeesCollectionProxyStub))
    ///        as Proxies.EmployeesCollectionProxyStub;
    /// 
    ///    // Set a property and notice that esRowState goes to Modified
    ///    client.Collection[0].LastName = "crazy_horse";
    /// 
    ///    // Serialize our client side proxy into xml and send it to the server
    ///    xml = esDataContractSerializer.ToXml(client);
    /// 
    ///    // Deserialize it on the server, the esRowState is modifed as we would expect
    ///    BusinessObjects.EmployeesCollectionProxyStub server1 = esDataContractSerializer.FromXml(xml, typeof(BusinessObjects.EmployeesCollectionProxyStub))
    ///        as BusinessObjects.EmployeesCollectionProxyStub;
    /// 
    ///    // Now save the Entity
    ///    server1.GetCollection().Save();
    /// }
    /// </code>
    /// </example>
    static public class esDataContractSerializer
    {
        #region ToXml
        /// <summary>
        /// Serialize an object 
        /// </summary>
        /// <param name="o">The object to convert to XML</param>
        /// <returns>The serialized contract in the form of a string</returns>
        static public string ToXml(object o)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType());
            return _ToXml(o, dcs);
        }

#if(!SILVERLIGHT)
        /// <summary>
        /// This overload can preserve object references
        /// </summary>
        /// <param name="o">The object to convert to XML</param>
        /// <param name="preserveObjectReferences">If 'True' then if the same object is in the graph twice it will only be serialized once. The other instance will be a pointer to the first</param>
        /// <returns>The serialized contract in the form of a string</returns>
        static public string ToXml(object o, bool preserveObjectReferences)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType(), null, Int32.MaxValue, true, true, null);
            return _ToXml(o, dcs);
        }
#endif

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="o">The object to convert to XML</para
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <returns>The serialized contract in the form of a string</returns>
        static public string ToXml(object o, string rootName, string rootNamespace)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType(), rootName, rootNamespace);
            return _ToXml(o, dcs);
        }

#if(!SILVERLIGHT)
        /// <summary>
        /// This overload can preserve object references
        /// </summary>
        /// <param name="o">The object to convert to XML</para
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <param name="preserveObjectReferences">If 'True' then if the same object is in the graph twice it will only be serialized once. The other instance will be a pointer to the first</param>
        /// <returns>The serialized contract in the form of a string</returns>
        static public string ToXml(object o, string rootName, string rootNamespace, bool preserveObjectReferences)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType(), rootName, rootNamespace, null, Int32.MaxValue, true, true, null);
            return _ToXml(o, dcs);
        }
#endif

        static private string _ToXml(object o, DataContractSerializer dcs)
        {
            string xml = "";

            using (MemoryStream ms = new MemoryStream())
            {
                dcs.WriteObject(ms, o);
                ms.Seek(0, SeekOrigin.Begin);

                using (StreamReader sr = new StreamReader(ms))
                {
                    xml = sr.ReadToEnd();
                }
            }

            return xml;
        }
        #endregion 

        #region ToByteArray

        /// <summary>
        /// Serialize an object 
        /// </summary>
        /// <param name="o">The object to convert to a byte array</param>
        /// <returns>The byte[] encoded with UTF8Encoding</returns>
        static public byte[] ToByteArray(object o)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType());
            string xml = _ToXml(o, dcs);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(xml);
        }

#if(!SILVERLIGHT)
        /// <summary>
        /// This overload can preserve object references
        /// </summary>
        /// <param name="o">The object to convert to a byte array</param>
        /// <param name="preserveObjectReferences">If 'True' then if the same object is in the graph twice it will only be serialized once. The other instance will be a pointer to the first</param>
        /// <returns>The byte[] encoded with UTF8Encoding</returns>
        static public byte[] ToByteArray(object o, bool preserveObjectReferences)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType(), null, Int32.MaxValue, true, true, null);
            string xml = _ToXml(o, dcs);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(xml);
        }
#endif

        /// <summary>
        /// Serialize an object 
        /// </summary>
        /// <param name="o">The object to convert to a byte array</param>
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <returns>The byte[] encoded with UTF8Encoding</returns>
        static public byte[] ToByteArray(object o, string rootName, string rootNamespace)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType(), rootName, rootNamespace);
            string xml =_ToXml(o, dcs);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(xml);
        }

#if(!SILVERLIGHT)
        /// <summary>
        /// This overload can preserve object references
        /// </summary>
        /// <param name="o">The object to convert to a byte array</param>
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <param name="preserveObjectReferences">If 'True' then if the same object is in the graph twice it will only be serialized once. The other instance will be a pointer to the first</param>
        /// <returns></returns>
        static public byte[] ToByteArray(object o, string rootName, string rootNamespace, bool preserveObjectReferences)
        {
            DataContractSerializer dcs = new DataContractSerializer(o.GetType(), rootName, rootNamespace, null, Int32.MaxValue, true, true, null);
            string xml = _ToXml(o, dcs);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(xml);
        }
#endif

        #endregion

        #region FromXml

        /// <summary>
        /// Deserialize an object 
        /// </summary>
        /// <param name="xml">The results of a previous call to <see cref="ToXml"/></param>
        /// <param name="type">Type type of the object, in C# use typeof(), VB use GetType()</param>
        /// <returns>The deserialized object</returns>
        static public object FromXml(string xml, Type type)
        {
            DataContractSerializer serializer = new DataContractSerializer(type);
            return _FromXml(xml, serializer);
        }

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <typeparam name="T">The Type of object being deserialized</typeparam>
        /// <param name="xml">The results of a previous call to <see cref="ToXml"/></param>
        /// <returns>The deserialized object</returns>
        static public T FromXml<T>(string xml)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            return (T)_FromXml(xml, serializer);
        }

        /// <summary>
        /// Deserialize an object 
        /// </summary>
        /// <param name="xml">The results of a previous call to <see cref="ToXml"/></param>
        /// <param name="type">Type type of the object, in C# use typeof(), VB use GetType()</param>
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <returns>The deserialized object</returns>
        static public object FromXml(string xml, Type type, string rootName, string rootNamespace)
        {
            DataContractSerializer serializer = new DataContractSerializer(type, rootName, rootNamespace);
            return _FromXml(xml, serializer);
        }

        /// <summary>
        /// Deserialize an object 
        /// </summary>
        /// <typeparam name="T">The Type of object being deserialized</typeparam>
        /// <param name="xml">The results of a previous call to <see cref="ToXml"/></param>
        /// <param name="type">Type type of the object, in C# use typeof(), VB use GetType()</param>
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <returns>The deserialized object</returns>        
        static public T FromXml<T>(string xml, string rootName, string rootNamespace)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T), rootName, rootNamespace);
            return (T)_FromXml(xml, serializer);
        }



        static private object _FromXml(string xml, DataContractSerializer serializer)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.MaxCharactersFromEntities = long.MaxValue;
                settings.MaxCharactersInDocument = long.MaxValue;

                using (var reader = System.Xml.XmlDictionaryReader.Create(memoryStream, settings))
                {
                    // Deserialize
                    return serializer.ReadObject(reader); ;
                }
            }
        }
        
        #endregion

        #region FromByteArray

        /// <summary>
        /// Deserialize an object 
        /// </summary>
        /// <param name="bytes">The results of a previous call to <see cref="ToByteArray"/></param>        
        /// <param name="type">Type type of the object, in C# use typeof(), VB use GetType()</param>
        /// <returns>The deserialized object</returns>
        static public object FromByteArray(byte[] bytes, Type type)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
#if(!SILVERLIGHT)
            string xml = enc.GetString(bytes);
#else
            string xml = enc.GetString(bytes, 0, bytes.Length);
#endif

            DataContractSerializer serializer = new DataContractSerializer(type);
            return _FromXml(xml, serializer);
        }

        /// <summary>
        /// Deserialize an object
        /// </summary>
        /// <typeparam name="T">The Type of object being deserialized</typeparam>
        /// <param name="bytes">The results of a previous call to <see cref="ToByteArray"/></param>
        /// <returns>The deserialized object</returns>        
        static public T FromByteArray<T>(byte[] bytes)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
#if(!SILVERLIGHT)
            string xml = enc.GetString(bytes);
#else
            string xml = enc.GetString(bytes, 0, bytes.Length);
#endif

            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            return (T)_FromXml(xml, serializer);
        }

        /// <summary>
        /// Deserialize an object 
        /// </summary>
        /// <param name="bytes">The results of a previous call to <see cref="ToByteArray"/></param>
        /// <param name="type">Type type of the object, in C# use typeof(), VB use GetType()</param>
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <returns>The deserialized object</returns>        
        static public object FromByteArray(byte[] bytes, Type type, string rootName, string rootNamespace)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
#if(!SILVERLIGHT)
            string xml = enc.GetString(bytes);
#else
            string xml = enc.GetString(bytes, 0, bytes.Length);
#endif

            DataContractSerializer serializer = new DataContractSerializer(type, rootName, rootNamespace);
            return _FromXml(xml, serializer);
        }

        /// <summary>
        /// Deserialize an object 
        /// </summary>
        /// <typeparam name="T">The Type of object being deserialized</typeparam>
        /// <param name="bytes">The results of a previous call to <see cref="ToByteArray"/></param>
        /// <param name="type">Type type of the object, in C# use typeof(), VB use GetType()</param>
        /// <param name="rootName">Typically the object name, 'Employees', for instance</param>
        /// <param name="rootNamespace">An xml namespaces, example, 'http://www.entityspaces.net'</param>
        /// <returns>The deserialized object</returns>         
        static public T FromByteArray<T>(byte[] bytes, string rootName, string rootNamespace)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
#if(!SILVERLIGHT)
            string xml = enc.GetString(bytes);
#else
            string xml = enc.GetString(bytes, 0, bytes.Length);
#endif

            DataContractSerializer serializer = new DataContractSerializer(typeof(T), rootName, rootNamespace);
            return (T)_FromXml(xml, serializer);
        }


        #endregion
    }
}
#endif
