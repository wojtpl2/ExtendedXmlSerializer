﻿// MIT License
//
// Copyright (c) 2016-2018 Wojciech Nagórski
//                    Michael DeMond
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using ExtendedXmlSerializer.Configuration;
using System;
using System.IO;
using System.Xml;

namespace ExtendedXmlSerializer.Samples.MigrationMap
{
	public class MigrationMapSamples
	{
		public static void RunSimpleConfig()
		{
			Program.PrintHeader("Deserialization old version of xml");
// MigrationsConfiguration

IExtendedXmlSerializer serializer = new ConfigurationContainer().Type<TestClass>()
																.AddMigration(new TestClassMigrations())
																.Create();
// EndMigrationsConfiguration
            Run(serializer);
		}

//        public static void RunAutofacConfig()
//        {
//            Program.PrintHeader("Deserialization old version of xml - autofac config");
//
//            var builder = new ContainerBuilder();
//            builder.RegisterModule<AutofacExtendedXmlSerializerModule>();
//            builder.RegisterType<TestClassConfig>().As<ExtendedXmlSerializerConfig<TestClass>>().SingleInstance();
//            var containter = builder.Build();
//
//            var serializer = containter.Resolve<IExtendedXmlSerializer>();
//            Run(serializer);
//        }

		static void Run(IExtendedXmlSerializer serializer)
		{
			string xml =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<TestClass xmlns=""clr-namespace:ExtendedXmlSerializer.Samples.MigrationMap;assembly=ExtendedXmlSerializer.Samples"">
<Id>1</Id>
<Type>Type</Type>
</TestClass>";
			Console.WriteLine(xml);
			TestClass obj = serializer.Deserialize<TestClass>(xml);

			Console.WriteLine("Obiect Id = " + obj.Id);
			Console.WriteLine("Obiect Name = " + obj.Name);
			Console.WriteLine("Obiect Value = " + obj.Value);

			Console.WriteLine("Serialization to new version");
		    string xml2 = serializer.Serialize(new XmlWriterSettings {Indent = true}, obj);

            File.WriteAllText("bin\\XmlLastVersion.xml", xml2);
            Console.WriteLine(xml2);
		}
	}
}