namespace Fanex.Athena.UnitTest.Common.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using NSubstitute;
    using NSubstitute.Core;

    public class DataAccessTestHelper
    {
        public static void MapReaderData(IDataReader reader, params object[] readerDataMappingList)
        {
            Func<CallInfo, object> firstFunc = CreateMappingFunc(readerDataMappingList[0]);
            if (readerDataMappingList.Length == 1)
            {
                reader[Arg.Any<string>()].Returns(firstFunc);
                return;
            }

            var funcList = new List<Func<CallInfo, object>>();

            for (int i = 1; i < readerDataMappingList.Length; i++)
            {
                funcList.Add(CreateMappingFunc(readerDataMappingList[i]));
            }

            reader[Arg.Any<string>()].Returns(firstFunc, funcList.ToArray());
        }

        private static Func<CallInfo, object> CreateMappingFunc(object mappingData)
        {
            var dictionary = ToDictionary(mappingData);
            return new Func<CallInfo, object>(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (dictionary.ContainsKey(paramName))
                {
                    return dictionary[paramName];
                }

                return string.Empty;
            });
        }

        private static IDictionary<string, object> ToDictionary(object data)
        {
            if (data is IDictionary)
            {
                return (IDictionary<string, object>)data;
            }

            var type = data.GetType();
            var properties = type.GetProperties();
            return properties.ToDictionary(x => x.Name, x => x.GetValue(data, null), StringComparer.OrdinalIgnoreCase);
        }
    }
}