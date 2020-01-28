using ExtendedXmlSerializer.ContentModel.Content;
using ExtendedXmlSerializer.ContentModel.Format;
using ExtendedXmlSerializer.Core.Specifications;
using ExtendedXmlSerializer.ExtensionModel.Xml;
using System;
using System.Linq;
using System.Reflection;

namespace ExtendedXmlSerializer.ExtensionModel.References
{
	sealed class ReferenceAwareSerializers : ISerializers
	{
		readonly ISpecification<object>        _conditions;
		readonly ISpecification<TypeInfo>      _contains;
		readonly IStaticReferenceSpecification _specification;
		readonly IReferences                   _references;
		readonly ISerializers                  _serializers;

		// ReSharper disable once TooManyDependencies
		public ReferenceAwareSerializers(IStaticReferenceSpecification specification,
		                                 IContainsCustomSerialization custom, IReferences references,
		                                 ISerializers serializers)
			: this(new InstanceConditionalSpecification(), custom, specification, references, serializers) {}

		// ReSharper disable once TooManyDependencies
		public ReferenceAwareSerializers(ISpecification<object> conditions, ISpecification<TypeInfo> contains,
		                                 IStaticReferenceSpecification specification, IReferences references,
		                                 ISerializers serializers)
		{
			_conditions    = conditions;
			_contains      = contains;
			_specification = specification;
			_references    = references;
			_serializers   = serializers;
		}

		public ContentModel.ISerializer Get(TypeInfo parameter)
		{
			var serializer = _serializers.Get(parameter);
			var result = _specification.IsSatisfiedBy(parameter)
				             ? new Serializer(AssignedSpecification<TypeInfo>.Default.And(_contains.Inverse()),
				                              _conditions, _references, serializer)
				             : serializer;
			return result;
		}

		sealed class Serializer : ContentModel.ISerializer
		{
			readonly ISpecification<TypeInfo> _default;
			readonly ISpecification<object>   _conditions;
			readonly IReferences              _references;
			readonly ContentModel.ISerializer _container;

			// ReSharper disable once TooManyDependencies
			public Serializer(ISpecification<TypeInfo> @default, ISpecification<object> conditions,
			                  IReferences references, ContentModel.ISerializer container)
			{
				_default    = @default;
				_conditions = conditions;
				_references = references;
				_container  = container;
			}

			public object Get(IFormatReader parameter) => _container.Get(parameter);

			public void Write(IFormatWriter writer, object instance)
			{
				if (_conditions.IsSatisfiedBy(writer.Get()))
				{
					var typeInfo = instance?.GetType()
					                       .GetTypeInfo();
					if (_default.IsSatisfiedBy(typeInfo))
					{
						var references = _references.Get(instance);
						if (references.Any())
						{
							var line = Environment.NewLine;
							var message =
								$"{line}{line}Here is a list of found references:{line}{string.Join(line, references.Select(x => $"- {x}"))}";

							throw new CircularReferencesDetectedException(
							                                              $"The provided instance of type '{typeInfo}' contains circular references within its graph. Serializing this instance would result in a recursive, endless loop. To properly serialize this instance, please create a serializer that has referential support enabled by extending it with the ReferencesExtension.{message}",
							                                              _container);
						}
					}
				}

				_container.Write(writer, instance);
			}
		}
	}
}