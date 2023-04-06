using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;


namespace GeneralTool.General.IniHelpers
{
    /// <summary>
    /// Provides methods for reading and writing to an INI file.
    /// </summary>
    public class IniHelper
    {
        #region Private 字段

        private static readonly Lazy<IniHelper> _instance;

        //The path of the file we are operating on.
        private readonly string m_path;

        #endregion Private 字段

        #region Public 字段

        /// <summary>
        /// The maximum size of a section in an ini file.
        /// </summary>
        /// <remarks>
        /// This property defines the maximum size of the buffers used to retreive data from an ini
        /// file. This value is the maximum allowed by the win32 functions
        /// GetPrivateProfileSectionNames() or GetPrivateProfileString().
        /// </remarks>
        public static int MaxSectionSize { get; set; } = 32767; // 32 KB

        /// <summary>
        /// Ini对象,使用默认的保存位置
        /// </summary>
        public static readonly IniHelper IniHelperInstance;



        #endregion Public 字段


        #region Public 构造函数

        static IniHelper()
        {
            _instance = new Lazy<IniHelper>(() => new IniHelper());
            IniHelperInstance = _instance.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IniHelper"/> class.
        /// </summary>
        /// <param name="path">
        /// The ini file to read and write from.
        /// </param>
        public IniHelper(string path)
        {
            //Convert to the full path.  Because of backward compatibility,
            // the win32 functions tend to assume the path should be the
            // root Windows directory if it is not specified.  By calling
            // GetFullPath, we make sure we are always passing the full path
            // the win32 functions.
            var file = new FileInfo(path);
            Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(file.Directory.FullName);
            m_path = System.IO.Path.GetFullPath(path);
        }

        /// <summary>
        /// </summary>
        public IniHelper() : this(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs\\default.ini"))
        {

        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// Gets the full path of ini file this object instance is operating on.
        /// </summary>
        /// <value>
        /// A file path.
        /// </value>
        public string Path
        {
            get
            {
                return m_path;
            }
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 获取新的Ini对象
        /// </summary>
        /// <param name="savePath">Ini配置文件需要保存到的路径</param>
        /// <returns></returns>
        public static IniHelper GetInstace(string savePath)
        {
            return new IniHelper(savePath);
        }

        /// <summary>
        /// Deletes the specified key from the specified section.
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section to remove the key from.
        /// </param>
        /// <param name="keyName">
        /// Name of the key to remove.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> are a null reference
        /// (Nothing in VB)
        /// </exception>
        public void DeleteKey(string sectionName, string keyName)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            WriteValueInternal(sectionName, keyName, null);
        }

        /// <summary>
        /// Deletes a section from the ini file.
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section to delete.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is a null reference (Nothing in VB)
        /// </exception>
        public void DeleteSection(string sectionName)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            WriteValueInternal(sectionName, null, null);
        }

        /// <summary>
        /// 获取指定的数组类型数据
        /// </summary>
        /// <typeparam name="T">
        /// 要获取的类型
        /// </typeparam>
        /// <param name="sectionName">
        /// </param>
        /// <param name="keyName">
        /// </param>
        /// <returns>
        /// </returns>
        public T[] GetArray<T>(string sectionName, string keyName)
        {
            if (!this.InTypeCode<T>())
            {
                throw new ArgumentException("无法转换指定的类型");
            }
            var strTmp = this.GetString(sectionName, keyName, "");
            var array = strTmp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return Array.ConvertAll<string, T>(array, new Converter<string, T>(s => { return (T)Convert.ChangeType(s, typeof(T)); }));
        }

        /// <summary>
        /// Gets the value of a setting in an ini file as a <see cref="T:System.Boolean"/>.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read from.
        /// </param>
        /// <param name="keyName">
        /// The name of the key in section to read.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the key cannot be found.
        /// </param>
        /// <returns>
        /// The value of the key, if found. Otherwise, returns <paramref name="defaultValue"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> are a null reference
        /// (Nothing in VB)
        /// </exception>
        public bool GetBoolean(string sectionName,
                                 string keyName,
                                 bool defaultValue)
        {
            string str = GetString(sectionName, keyName, "");
            if (String.IsNullOrEmpty(str))
                return defaultValue;

            if (!Boolean.TryParse(str, out bool retval))
            {
                retval = defaultValue;
            }

            return retval;
        }

        /// <summary>
        /// Gets the value of a setting in an ini file as a <see cref="T:System.Double"/>.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read from.
        /// </param>
        /// <param name="keyName">
        /// The name of the key in section to read.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the key cannot be found.
        /// </param>
        /// <returns>
        /// The value of the key, if found. Otherwise, returns <paramref name="defaultValue"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> are a null reference
        /// (Nothing in VB)
        /// </exception>
        public double GetDouble(string sectionName,
                                string keyName,
                                double defaultValue)
        {
            string str = GetString(sectionName, keyName, "");

            if (String.IsNullOrEmpty(str))
                return defaultValue;

            if (!Double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out double retval))
                retval = defaultValue;

            return retval;
        }

        /// <summary>
        /// Gets the value of a setting in an ini file as a <see cref="T:System.Int16"/>.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read from.
        /// </param>
        /// <param name="keyName">
        /// The name of the key in section to read.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the key cannot be found.
        /// </param>
        /// <returns>
        /// The value of the key, if found. Otherwise, returns <paramref name="defaultValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> are a null reference
        /// (Nothing in VB)
        /// </exception>
        public short GetInt16(string sectionName,
                              string keyName,
                              short defaultValue)
        {
            int retval = GetInt32(sectionName, keyName, defaultValue);

            return Convert.ToInt16(retval);
        }

        /// <summary>
        /// Gets the value of a setting in an ini file as a <see cref="T:System.Int32"/>.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read from.
        /// </param>
        /// <param name="keyName">
        /// The name of the key in section to read.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the key cannot be found.
        /// </param>
        /// <returns>
        /// The value of the key, if found. Otherwise, returns <paramref name="defaultValue"/>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> are a null reference
        /// (Nothing in VB)
        /// </exception>
        public int GetInt32(string sectionName,
                            string keyName,
                            int defaultValue)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            return NativeMethods.GetPrivateProfileInt(sectionName, keyName, defaultValue, m_path);
        }

        /// <summary>
        /// Gets the names of all keys under a specific section in the ini file.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read key names from.
        /// </param>
        /// <returns>
        /// An array of key names.
        /// </returns>
        /// <remarks>
        /// The total length of all key names in the section must be less than 32KB in length.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is a null reference (Nothing in VB)
        /// </exception>
        public string[] GetKeyNames(string sectionName)
        {
            int len;
            string[] retval;

            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            //Allocate a buffer for the returned section names.
            IntPtr ptr = Marshal.AllocCoTaskMem(IniHelper.MaxSectionSize);

            try
            {
                //Get the section names into the buffer.
                len = NativeMethods.GetPrivateProfileString(sectionName,
                                                            null,
                                                            null,
                                                            ptr,
                                                            (uint)IniHelper.MaxSectionSize,
                                                            m_path);

                retval = ConvertNullSeperatedStringToStringArray(ptr, len);
            }
            finally
            {
                //Free the buffer
                Marshal.FreeCoTaskMem(ptr);
            }

            return retval;
        }

        /// <summary>
        /// Gets the names of all sections in the ini file.
        /// </summary>
        /// <returns>
        /// An array of section names.
        /// </returns>
        /// <remarks>
        /// The total length of all section names in the section must be less than 32KB in length.
        /// </remarks>
        public string[] GetSectionNames()
        {
            string[] retval;
            int len;

            //Allocate a buffer for the returned section names.
            IntPtr ptr = Marshal.AllocCoTaskMem((int)IniHelper.MaxSectionSize);

            try
            {
                //Get the section names into the buffer.
                len = NativeMethods.GetPrivateProfileSectionNames(ptr,
                    (uint)IniHelper.MaxSectionSize, m_path);

                retval = ConvertNullSeperatedStringToStringArray(ptr, len);
            }
            finally
            {
                //Free the buffer
                Marshal.FreeCoTaskMem(ptr);
            }

            return retval;
        }

        /// <summary>
        /// Gets all of the values in a section as a dictionary.
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section to retrieve values from.
        /// </param>
        /// <returns>
        /// A <see cref="Dictionary{T, T}"/> containing the key/value pairs found in this section.
        /// </returns>
        /// <remarks>
        /// If a section contains more than one key with the same name, this function only returns
        /// the first instance. If you need to get all key/value pairs within a section even when
        /// keys have the same name, use <see cref="GetSectionValuesAsList"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is a null reference (Nothing in VB)
        /// </exception>
        public Dictionary<string, string> GetSectionValues(string sectionName)
        {
            List<KeyValuePair<string, string>> keyValuePairs;
            Dictionary<string, string> retval;

            keyValuePairs = GetSectionValuesAsList(sectionName);

            //Convert list into a dictionary.
            retval = new Dictionary<string, string>(keyValuePairs.Count);

            foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
            {
                //Skip any key we have already seen.
                if (!retval.ContainsKey(keyValuePair.Key))
                {
                    retval.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return retval;
        }

        /// <summary>
        /// Gets all of the values in a section as a list.
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section to retrieve values from.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> containing <see cref="KeyValuePair{T1, T2}"/> objects that
        /// describe this section. Use this verison if a section may contain multiple items with the
        /// same key value. If you know that a section cannot contain multiple values with the same
        /// key name or you don't care about the duplicates, use the more convenient <see
        /// cref="GetSectionValues"/> function.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is a null reference (Nothing in VB)
        /// </exception>
        public List<KeyValuePair<string, string>> GetSectionValuesAsList(string sectionName)
        {
            List<KeyValuePair<string, string>> retval;
            string[] keyValuePairs;
            string key, value;
            int equalSignPos;

            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            //Allocate a buffer for the returned section names.
            IntPtr ptr = Marshal.AllocCoTaskMem((int)IniHelper.MaxSectionSize);

            try
            {
                //Get the section key/value pairs into the buffer.
                int len = NativeMethods.GetPrivateProfileSection(sectionName,
                                                                 ptr,
                                                                 (uint)IniHelper.MaxSectionSize,
                                                                 m_path);

                keyValuePairs = ConvertNullSeperatedStringToStringArray(ptr, len);
            }
            finally
            {
                //Free the buffer
                Marshal.FreeCoTaskMem(ptr);
            }

            //Parse keyValue pairs and add them to the list.
            retval = new List<KeyValuePair<string, string>>(keyValuePairs.Length);

            for (int i = 0; i < keyValuePairs.Length; ++i)
            {
                //Parse the "key=value" string into its constituent parts
                equalSignPos = keyValuePairs[i].IndexOf('=');

                key = keyValuePairs[i].Substring(0, equalSignPos);

                value = keyValuePairs[i].Substring(equalSignPos + 1,
                                                   keyValuePairs[i].Length - equalSignPos - 1);

                retval.Add(new KeyValuePair<string, string>(key, value));
            }

            return retval;
        }

        /// <summary>
        /// Gets the value of a setting in an ini file as a <see cref="T:System.String"/>.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read from.
        /// </param>
        /// <param name="keyName">
        /// The name of the key in section to read.
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return if the key cannot be found.
        /// </param>
        /// <returns>
        /// The value of the key, if found. Otherwise, returns <paramref name="defaultValue"/>
        /// </returns>
        /// <remarks>
        /// The retreived value must be less than 32KB in length.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> are a null reference
        /// (Nothing in VB)
        /// </exception>
        public string GetString(string sectionName,
                                string keyName,
                                string defaultValue)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            var builder = new StringBuilder(MaxSectionSize);

            var result = NativeMethods.GetPrivateProfileString(sectionName,
                                                  keyName,
                                                  defaultValue,
                                                  builder,
                                                  IniHelper.MaxSectionSize,
                                                  m_path);


            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetString2(string sectionName,
                              string keyName)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");


            //Allocate a buffer for the returned section names.
            IntPtr ptr = Marshal.AllocCoTaskMem(IniHelper.MaxSectionSize);

            try
            {
                //Get the section names into the buffer.
                var len = NativeMethods.GetPrivateProfileString(sectionName,
                                                             keyName,
                                                             "",
                                                             ptr,
                                                             (uint)IniHelper.MaxSectionSize,
                                                            m_path);

                string buff = Marshal.PtrToStringAuto(ptr, len - 1);

            }
            finally
            {
                //Free the buffer
                Marshal.FreeCoTaskMem(ptr);
            }

            return "";
        }


        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="sectionName">
        /// </param>
        /// <param name="keyName">
        /// </param>
        /// <param name="defaultVal">
        /// </param>
        /// <returns>
        /// </returns>
        public T GetValue<T>(string sectionName, string keyName, T defaultVal = default)
        {
            if (!this.InTypeCode<T>())
            {
                throw new ArgumentException("无法转换指定的类型");
            }

            var strTmp = this.GetString(sectionName, keyName, defaultVal + "");
            return (T)Convert.ChangeType(strTmp, typeof(T));
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="Node">
        /// </param>
        /// <returns>
        /// </returns>
        public T GetValue<T>(Node<T> Node)
        {
            if (!this.InTypeCode<T>())
                throw new ArgumentException("无法转换指定的类型");

            return GetValue<T>(Node.SectionName, Node.KeyName, Node.Value);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="node">
        /// </param>
        /// <returns>
        /// </returns>
        public T[] GetValues<T>(Node<T> node)
        {
            if (!this.InTypeCode<T>())
                throw new ArgumentException("无法转换指定的类型");

            return GetArray<T>(node.SectionName, node.KeyName);
        }

        /// <summary>
        /// Writes a <see cref="T:System.String"/> value to the ini file.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to write to .
        /// </param>
        /// <param name="keyName">
        /// The name of the key to write to.
        /// </param>
        /// <param name="value">
        /// The string value to write
        /// </param>
        /// <exception cref="T:System.ComponentModel.Win32Exception">
        /// The write failed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> or <paramref name="keyName"/> or <paramref name="value"/>
        /// are a null reference (Nothing in VB)
        /// </exception>
        public void WriteValueString(string sectionName, string keyName, string value)
        {
            if (sectionName == null)
                throw new ArgumentNullException("sectionName");

            if (keyName == null)
                throw new ArgumentNullException("keyName");

            if (value == null)
                throw new ArgumentNullException("value");

            WriteValueInternal(sectionName, keyName, value);
        }



        /// <summary>
        /// 写入值
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="sectionName">
        /// </param>
        /// <param name="keyName">
        /// </param>
        /// <param name="arrary">
        /// </param>
        public void WriteValue<T>(string sectionName, string keyName, IEnumerable<T> arrary)
        {
            var str = String.Join(",", arrary);
            this.WriteValueString(sectionName, keyName, str);
        }

        /// <summary>
        /// 写入值
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="sectionName">
        /// </param>
        /// <param name="keyName">
        /// </param>
        /// <param name="item">
        /// </param>
        public void WriteValueT<T>(string sectionName, string keyName, T item)
        {
            if (!this.InTypeCode<T>())
                throw new ArgumentException("无法转换指定的类型");
            else
            {
                WriteValueString(sectionName, keyName, item.ToString());
            }
        }

        /// <summary>
        /// 写入值
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="node">
        /// </param>
        public void WriteValue<T>(Node<T> node)
        {
            this.WriteValueT<T>(node.SectionName, node.KeyName, node.Value);
        }

        #endregion Public 方法

        #region Private 方法

        /// <summary>
        /// Converts the null seperated pointer to a string into a string array.
        /// </summary>
        /// <param name="ptr">
        /// A pointer to string data.
        /// </param>
        /// <param name="valLength">
        /// Length of the data pointed to by <paramref name="ptr"/>.
        /// </param>
        /// <returns>
        /// An array of strings; one for each null found in the array of characters pointed at by
        /// <paramref name="ptr"/>.
        /// </returns>
        private static string[] ConvertNullSeperatedStringToStringArray(IntPtr ptr, int valLength)
        {
            string[] retval;

            if (valLength == 0)
            {
                //Return an empty array.
                retval = new string[0];
            }
            else
            {
                //Convert the buffer into a string.  Decrease the length
                //by 1 so that we remove the second null off the end.
                string buff = Marshal.PtrToStringAuto(ptr, valLength - 1);

                //Parse the buffer into an array of strings by searching for nulls.
                retval = buff.Split('\0');
            }

            return retval;
        }

        private bool InTypeCode<T>()
        {
            return Enum.TryParse<TypeCode>(typeof(T).Name, out _);
        }

        /// <summary>
        /// Writes a <see cref="T:System.String"/> value to the ini file.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to write to .
        /// </param>
        /// <param name="keyName">
        /// The name of the key to write to.
        /// </param>
        /// <param name="value">
        /// The string value to write
        /// </param>
        /// <exception cref="T:System.ComponentModel.Win32Exception">
        /// The write failed.
        /// </exception>
        private void WriteValueInternal(string sectionName, string keyName, string value)
        {
            if (!NativeMethods.WritePrivateProfileString(sectionName, keyName, value, m_path))
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        #endregion Private 方法

        #region Private 类

        /// <summary>
        /// A static class that provides the win32 P/Invoke signatures used by this class.
        /// </summary>
        /// <remarks>
        /// Note:  In each of the declarations below, we explicitly set CharSet to Auto. By default
        /// in C#, CharSet is set to Ansi, which reduces performance on windows 2000 and above due
        /// to needing to convert strings from Unicode (the native format for all .Net strings) to
        /// Ansi before marshalling. Using Auto lets the marshaller select the Unicode version of
        /// these functions when available.
        /// </remarks>
        [System.Security.SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            #region Public 方法

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileInt(string lpAppName,
                                                          string lpKeyName,
                                                          int lpDefault,
                                                          string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileSection(string lpAppName,
                                                              IntPtr lpReturnedString,
                                                              uint nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer,
                                                                   uint nSize,
                                                                   string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName,
                                                              string lpKeyName,
                                                              string lpDefault,
                                                              StringBuilder lpReturnedString,
                                                              int nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern uint GetPrivateProfileString(string lpAppName,
                                                              string lpKeyName,
                                                              string lpDefault,
                                                              [In, Out] char[] lpReturnedString,
                                                              int nSize,
                                                              string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetPrivateProfileString(string lpAppName,
                                                             string lpKeyName,
                                                             string lpDefault,
                                                             IntPtr lpReturnedString,
                                                             uint nSize,
                                                             string lpFileName);

            //We explicitly enable the SetLastError attribute here because
            // WritePrivateProfileString returns errors via SetLastError.
            // Failure to set this can result in errors being lost during
            // the marshal back to managed code.
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool WritePrivateProfileString(string lpAppName,
                                                                string lpKeyName,
                                                                string lpString,
                                                                string lpFileName);

            #endregion Public 方法
        }

        #endregion Private 类
    }
}