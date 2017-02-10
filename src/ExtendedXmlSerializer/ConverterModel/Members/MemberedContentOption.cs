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
using ExtendedXmlSerialization.ConverterModel.Elements;
using ExtendedXmlSerialization.TypeModel;

namespace ExtendedXmlSerialization.ConverterModel.Members
{
	class MemberedContentOption : ContentOptionBase
	{
		readonly IActivators _activators;
		readonly IMembers _members;

		public MemberedContentOption(ISelector selector) : this(new Members(selector)) {}
		public MemberedContentOption(IMembers members) : this(Activators.Default, members) {}

		public MemberedContentOption(IActivators activators, IMembers members) : base(IsActivatedTypeSpecification.Default)
		{
			_activators = activators;
			_members = members;
		}

		public override IConverter Get(TypeInfo parameter)
		{
			var members = _members.Get(parameter);
			var activate = _activators.Get(parameter.AsType());
			var reader = new MemberedReader(new DelegatedFixedActivator(activate), members.ToDictionary(x => x.DisplayName));
			var result = new DecoratedConverter(reader, new MemberWriter(members));
			return result;
		}
	}
}