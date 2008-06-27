using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Package;

namespace Nemerle.VisualStudio.Project
{
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Guid(NemerleConstants.FileNodePropertiesGuidString)]
	public class NemerleFileNodeProperties : SingleFileGeneratorNodeProperties
	{
		public NemerleFileNodeProperties(HierarchyNode node)
			: base(node)
		{
		}

		[Browsable(false)]
		public string Url
		{
			get { return "file:///" + Node.Url; }
		}

		[Browsable(false)]
		public string SubType
		{
			get { return ((NemerleFileNode)Node).SubType;  }
			set { ((NemerleFileNode)Node).SubType = value; }
		}

		[SRCategoryAttribute   (Microsoft.VisualStudio.Package.SR.Advanced)]
		[LocDisplayName        (Microsoft.VisualStudio.Package.SR.BuildAction)]
		[SRDescriptionAttribute(Microsoft.VisualStudio.Package.SR.BuildActionDescription)]
		public virtual NemerleBuildAction NemerleBuildAction
		{
			get
			{
				string value = Node.ItemNode.ItemName;

				if (string.IsNullOrEmpty(value))
					return NemerleBuildAction.None;

				return (NemerleBuildAction)Enum.Parse(typeof(NemerleBuildAction), value);
			}

			set
			{
				Node.ItemNode.ItemName = value.ToString();
			}
		}

		[Browsable(false)]
		public override BuildAction BuildAction
		{
			get
			{
				switch (NemerleBuildAction)
				{
					case NemerleBuildAction.ApplicationDefinition:
					case NemerleBuildAction.Page:
					case NemerleBuildAction.Resource:
						return BuildAction.Compile;

					default:
						return (BuildAction)Enum.Parse(typeof(BuildAction), NemerleBuildAction.ToString());
				}
			}

			set
			{
				this.NemerleBuildAction = (NemerleBuildAction)Enum.Parse(typeof(NemerleBuildAction), value.ToString());
			}
		}
	}
}