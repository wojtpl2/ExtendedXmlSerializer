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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ExtendedXmlSerialization.Configuration;
using ExtendedXmlSerialization.ContentModel.Converters;
using ExtendedXmlSerialization.ContentModel.Members;
using ExtendedXmlSerialization.Test.Support;
using Xunit;
using Defaults = ExtendedXmlSerialization.Configuration.Defaults;

namespace ExtendedXmlSerialization.Test.ContentModel.Members
{
	public class InstanceDefaultsMemberSpecificationsTests
	{
		[Fact]
		public void EmitValuesBasedOnInstanceDefaults()
		{
			var instance = new SubjectWithDefaultValue();
			SerializationSupport.Default.Assert(instance, 
				@"<?xml version=""1.0"" encoding=""utf-8""?><InstanceDefaultsMemberSpecificationsTests-SubjectWithDefaultValue xmlns=""clr-namespace:ExtendedXmlSerialization.Test.ContentModel.Members;assembly=ExtendedXmlSerializerTest""><SomeValue>This is a Default Value!</SomeValue></InstanceDefaultsMemberSpecificationsTests-SubjectWithDefaultValue>");

			var configuration = new ExtendedXmlConfiguration(
				Defaults.Property, Defaults.Field,
				new Dictionary<MemberInfo, IConverter>(), 
			    new MemberEmitSpecifications(InstanceDefaultsMemberSpecifications.Default, FixedMemberEmitSpecifications.Default), 
			    new Dictionary<MemberInfo, IRuntimeMemberSpecification>());

			var support = new SerializationSupport(configuration.Create());
			support.Assert(instance, 
				@"<?xml version=""1.0"" encoding=""utf-8""?><InstanceDefaultsMemberSpecificationsTests-SubjectWithDefaultValue xmlns=""clr-namespace:ExtendedXmlSerialization.Test.ContentModel.Members;assembly=ExtendedXmlSerializerTest"" />");
		}

		[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
		class SubjectWithDefaultValue
		{
			public SubjectWithDefaultValue() : this("This is a Default Value!") {}

			public SubjectWithDefaultValue(string someValue)
			{
				SomeValue = someValue;
			}
			public string SomeValue { get; set; }
		}
	}
}