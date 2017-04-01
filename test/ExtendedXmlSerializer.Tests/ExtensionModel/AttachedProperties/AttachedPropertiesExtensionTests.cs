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

using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.ExtensionModel.AttachedProperties;
using ExtendedXmlSerializer.ExtensionModel.Types;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using ExtendedXmlSerializer.Tests.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace ExtendedXmlSerializer.Tests.ExtensionModel.AttachedProperties
{
	public class AttachedPropertiesExtensionTests
	{
		[Fact]
		public void Verify()
		{
			var subject = new Subject {Message = "Hello World!"};
			NameProperty.Default.Assign(subject, "SubjectName");
			NumberProperty.Default.Assign(subject, 6776);

			var serializer =
				new SerializationSupport(new ExtendedConfiguration().EnableAttachedProperties(NameProperty.Default,
				                                                                              NumberProperty.Default));
			serializer.Serialize(subject)
			          .Should()
			          .Be(
				          @"<?xml version=""1.0"" encoding=""utf-8""?><AttachedPropertiesExtensionTests-Subject xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.AttachedProperties;assembly=ExtendedXmlSerializer.Tests""><Message>Hello World!</Message><AttachedPropertiesExtensionTests-NameProperty.Default>SubjectName</AttachedPropertiesExtensionTests-NameProperty.Default><AttachedPropertiesExtensionTests-NumberProperty.Default>6776</AttachedPropertiesExtensionTests-NumberProperty.Default></AttachedPropertiesExtensionTests-Subject>");
		}

		[Fact]
		public void VerifyAttributes()
		{
			var subject = new Subject {Message = "Hello World!"};
			NameProperty.Default.Assign(subject, "SubjectName");
			NumberProperty.Default.Assign(subject, 6776);

			var serializer =
				new SerializationSupport(new ExtendedConfiguration().UseAutoFormatting()
				                                                    .EnableAttachedProperties(NameProperty.Default,
				                                                                              NumberProperty.Default));
			serializer.Serialize(subject)
			          .Should()
			          .Be(
				          @"<?xml version=""1.0"" encoding=""utf-8""?><AttachedPropertiesExtensionTests-Subject Message=""Hello World!"" AttachedPropertiesExtensionTests-NameProperty.Default=""SubjectName"" AttachedPropertiesExtensionTests-NumberProperty.Default=""6776"" xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.AttachedProperties;assembly=ExtendedXmlSerializer.Tests"" />");
		}

		[Fact]
		public void VerifyConfiguration()
		{
			var subject = new Subject {Message = "Hello World!"};
			NumberProperty.Default.Assign(subject, 6776);

			var serializer =
				new SerializationSupport(
					new ExtendedConfiguration().UseAutoFormatting()
					                           .Type<NumberProperty>()
					                           .Name("ConfiguredAttachedProperty")
					                           .Configuration
					                           .AttachedProperty(() => NumberProperty.Default)
					                           .With(x => x.DeclaringProperty.Name("ConfiguredAttachedProperty"))
					                           .Name("NewNumberPropertyName").Configuration);

			serializer.Serialize(subject)
			          .Should()
			          .Be(
				          @"<?xml version=""1.0"" encoding=""utf-8""?><AttachedPropertiesExtensionTests-Subject Message=""Hello World!"" ConfiguredAttachedProperty.NewNumberPropertyName=""6776"" xmlns=""clr-namespace:ExtendedXmlSerializer.Tests.ExtensionModel.AttachedProperties;assembly=ExtendedXmlSerializer.Tests"" />");
		}

		sealed class Subject
		{
			public string Message { [UsedImplicitly] get; set; }
		}

		sealed class NameProperty : ReferenceProperty<Subject, string>
		{
			public const string DefaultMessage = "The Name Has Not Been Set";

			public static NameProperty Default { get; } = new NameProperty();
			NameProperty() : base(() => Default, x => DefaultMessage) {}
		}

		sealed class NumberProperty : StructureProperty<Subject, int>
		{
			public const int DefaultValue = 123;

			public static NumberProperty Default { get; } = new NumberProperty();
			NumberProperty() : base(() => Default, x => DefaultValue) {}
		}
	}
}