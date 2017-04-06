// MIT License
// 
// Copyright (c) 2016 Wojciech Nag�rski
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

using System.Linq;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Properties;
using ExtendedXmlSerializer.ContentModel.Xml;

namespace ExtendedXmlSerializer.ContentModel
{
	sealed class Classification : IClassification
	{
		readonly static IdentityStore IdentityStore = IdentityStore.Default;

		readonly IIdentityStore _identities;
		readonly IGenericTypes _generic;
		readonly ITypes _types;

		public Classification(IGenericTypes generic, ITypes types) : this(IdentityStore, generic, types) {}

		public Classification(IIdentityStore identities, IGenericTypes generic, ITypes types)
		{
			_identities = identities;
			_generic = generic;
			_types = types;
		}

		public TypeInfo Get(IContentAdapter parameter) => FromAttributes(parameter) ?? FromIdentity(parameter);

		TypeInfo FromAttributes(IContentAdapter parameter)
			=> parameter.Any()
				? ExplicitTypeProperty.Default.Get(parameter) ?? ItemTypeProperty.Default.Get(parameter) ?? Generic(parameter)
				: null;

		TypeInfo Generic(IContentAdapter parameter)
		{
			var arguments = ArgumentsTypeProperty.Default.Get(parameter);
			var result = arguments.HasValue
				? _generic.Get(_identities.Get(parameter.Name, parameter.Identifier))
				          .MakeGenericType(arguments.Value.ToArray())
				          .GetTypeInfo()
				: null;
			return result;
		}

		TypeInfo FromIdentity(IContentAdapter parameter) => _types.Get(_identities.Get(parameter.Name, parameter.Identifier));
	}
}