﻿// MIT License
// 
// Copyright (c) 2016 Wojciech Nagórski
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

using System;
using System.Diagnostics;
using ExtendedXmlSerialization.Profiles;
using ExtendedXmlSerialization.Test.TestObject;
using Xunit;

namespace ExtendedXmlSerialization.Test
{
	public class FuturesTest
	{
		class TestClassPrimitiveTypes2
		{
			public TestEnum PropEnum { get; set; }
		}

		[Fact]
		public void VerifyAutoAttributes()
		{
			var serializer = ExtendedSerialization.Default.Get(SerializerFuturesProfile.Default.Identifier);
			var instance = new TestClassPrimitiveTypes().Init();
			var data = serializer.Serialize(instance);
			Assert.Equal(
				@"<?xml version=""1.0"" encoding=""utf-8""?><TestClassPrimitiveTypes exs:type=""ExtendedXmlSerialization.Test.TestObject.TestClassPrimitiveTypes, ExtendedXmlSerializerTest"" PropInt=""-1"" PropuInt=""2234"" PropDecimal=""3.346"" PropDecimalMinValue=""-79228162514264337593543950335"" PropDecimalMaxValue=""79228162514264337593543950335"" PropFloat=""7.4432"" PropFloatNaN=""NaN"" PropFloatPositiveInfinity=""INF"" PropFloatNegativeInfinity=""-INF"" PropFloatMinValue=""-3.40282347E+38"" PropFloatMaxValue=""3.40282347E+38"" PropDouble=""3.4234"" PropDoubleNaN=""NaN"" PropDoublePositiveInfinity=""INF"" PropDoubleNegativeInfinity=""-INF"" PropDoubleMinValue=""-1.7976931348623157E+308"" PropDoubleMaxValue=""1.7976931348623157E+308"" PropLong=""234234142"" PropUlong=""2345352534"" PropShort=""23"" PropUshort=""2344"" PropDateTime=""2014-01-23T00:00:00"" PropByte=""23"" PropSbyte=""33"" PropChar=""103"" PropString=""TestString"" xmlns:exs=""https://github.com/wojtpl2/ExtendedXmlSerializer/futures"" />",
				data
			);
		}

		[Fact]
		public void VerifyAutoAttributesWithLongContent()
		{
			var serializer = ExtendedSerialization.Default.Get(SerializerFuturesProfile.Default.Identifier);
			var instance = new TestClassPrimitiveTypes().Init();
			instance.PropString =
				"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed urna sapien, pulvinar et consequat sit amet, fermentum in volutpat. This sentence should break the property out into content.";
			var data = serializer.Serialize(instance);
			Assert.Equal(
				@"<?xml version=""1.0"" encoding=""utf-8""?><TestClassPrimitiveTypes exs:type=""ExtendedXmlSerialization.Test.TestObject.TestClassPrimitiveTypes, ExtendedXmlSerializerTest"" PropInt=""-1"" PropuInt=""2234"" PropDecimal=""3.346"" PropDecimalMinValue=""-79228162514264337593543950335"" PropDecimalMaxValue=""79228162514264337593543950335"" PropFloat=""7.4432"" PropFloatNaN=""NaN"" PropFloatPositiveInfinity=""INF"" PropFloatNegativeInfinity=""-INF"" PropFloatMinValue=""-3.40282347E+38"" PropFloatMaxValue=""3.40282347E+38"" PropDouble=""3.4234"" PropDoubleNaN=""NaN"" PropDoublePositiveInfinity=""INF"" PropDoubleNegativeInfinity=""-INF"" PropDoubleMinValue=""-1.7976931348623157E+308"" PropDoubleMaxValue=""1.7976931348623157E+308"" PropLong=""234234142"" PropUlong=""2345352534"" PropShort=""23"" PropUshort=""2344"" PropDateTime=""2014-01-23T00:00:00"" PropByte=""23"" PropSbyte=""33"" PropChar=""103"" xmlns:exs=""https://github.com/wojtpl2/ExtendedXmlSerializer/futures""><PropString>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed urna sapien, pulvinar et consequat sit amet, fermentum in volutpat. This sentence should break the property out into content.</PropString></TestClassPrimitiveTypes>",
				data
			);
		}

		[Fact]
		public void WriterExtensionForAttachedProperties()
		{
			var instance = new TestClassPrimitiveTypes { PropDateTime = new DateTime(2000, 5, 6) };
			var serializer = ExtendedSerialization.Default.Get(SerializationProfileVersion20.Default.Identifier);
			var temp = serializer.Serialize(instance);
			/*Debugger.Break();*/
			/*var host = new SerializationToolsFactoryHost();
			var services = new List<object>();
			var writings = new WritingFactory(host, services);
			var plan = AutoAttributeWritePlanComposer.Default.Compose();
			var serializer = new ExtendedXmlSerializer(host, services, new AssignmentFactory(host), writings, new Serializer(plan, writings));*/
		}

		[Fact]
		public void CustomWritePlanForListsWithInheritance() {}
	}
}