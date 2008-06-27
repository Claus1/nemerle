﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Package.Automation;
using Microsoft.VisualStudio.Package;

namespace Nemerle.VisualStudio.Project
{
	class NemerleOAReferenceItem : OAReferenceItem
	{
		#region ctors

		public NemerleOAReferenceItem(OAProject project, ReferenceNode node)
			: base(project, node)
		{
		}

		#endregion

		/// <summary>
		/// Gets an enumeration indicating the type of object.
		/// </summary>
		public override string Kind
		{
			get { return Utils.GetTypeGuidAsString(Node); }
		}
	}
}
