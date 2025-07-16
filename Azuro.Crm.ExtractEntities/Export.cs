using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.ExtractEntities
{
	public static class Export
	{
		public static void ExportEntity(EntityMetadata entity, string path)
		{
			Console.WriteLine("Exporting entity [{0}] - [{1}]", entity.LogicalName, entity.ObjectTypeCode);			

			using (TextWriter tw = File.CreateText(Path.Combine(path, GetDisplayName(entity.DisplayName, entity.LogicalName) + ".cs")))
			{
				WriteHeader(entity, tw);

				//	TODO: Relationships

				var fieldNames = new List<string>();
				var attributes = new Dictionary<string, AttributeMetadata>(entity.Attributes.Count());
				foreach (var attribute in entity.Attributes)
				{
					attributes.Add(attribute.LogicalName, attribute);
					if (attribute.IsPrimaryId == true && string.Compare(attribute.LogicalName, entity.PrimaryIdAttribute) == 0)
					{
						WriteIdField(tw, attribute);
					}
					else if (attribute.AttributeType == AttributeTypeCode.ManagedProperty)
						continue;
					else
					{
						if (attribute.AttributeType == AttributeTypeCode.Money && attribute.LogicalName.EndsWith("_base")
							|| attribute.AttributeType == AttributeTypeCode.Virtual)
							continue;

						WriteField(tw, fieldNames, attribute);
					}
				}
				tw.WriteLine("\t}");
				tw.WriteLine("}");

				tw.Flush();
				tw.Close();
			}
		}

		private static void WriteHeader(EntityMetadata entity, TextWriter tw)
		{
			tw.WriteLine("using System;");
			tw.WriteLine("using System.Collections.Generic;");
			tw.WriteLine("using System.Linq;");
			tw.WriteLine("using System.Text;");
			tw.WriteLine("using System.Xml.Serialization;");
			tw.WriteLine("using Azuro.Common;");
			tw.WriteLine("using Azuro.Crm.Integration;");
			tw.WriteLine("using System.IO;");
			tw.WriteLine();
			tw.WriteLine("namespace Azuro.Crm.Entities");
			tw.WriteLine("{");
			tw.WriteLine("\t[CrmEntity(\"{0}\")]", entity.LogicalName);
			tw.WriteLine("\tpublic class {0} : CrmEntity<{0}> // {1} - {2} - {3}", GetDisplayName(entity.DisplayName, entity.LogicalName), entity.ObjectTypeCode, entity.PrimaryIdAttribute, entity.PrimaryNameAttribute);
			tw.WriteLine("\t{");
		}

		private static void WriteField(TextWriter tw, List<string> fieldNames, AttributeMetadata attribute)
		{
			var entityAttributeLine = string.Format("\t\t[CrmField(\"{0}\"", attribute.LogicalName);
			if (attribute.RequiredLevel.Value == AttributeRequiredLevel.SystemRequired)
				entityAttributeLine += ", IsRequired = true";

			string dataType = "string";
			bool isPicklist = false;
			switch (attribute.AttributeType)
			{
				case AttributeTypeCode.DateTime:
					dataType = "DateTime";
					if (attribute.RequiredLevel.Value != AttributeRequiredLevel.SystemRequired)
						dataType += "?";
					break;
				case AttributeTypeCode.Money:
					dataType = "decimal";
					break;
				case AttributeTypeCode.Memo:
				case AttributeTypeCode.String:
					dataType = "string";
					break;
				case AttributeTypeCode.Decimal:
					dataType = "float";
					break;
				case AttributeTypeCode.Double:
					dataType = "double";
					break;
				case AttributeTypeCode.Integer:
					dataType = "int";
					break;
				case AttributeTypeCode.Boolean:
					dataType = "bool";
					break;
				case AttributeTypeCode.BigInt:
					dataType = "long";
					break;
				case AttributeTypeCode.Lookup:
				case AttributeTypeCode.Owner:
				case AttributeTypeCode.Customer:
					dataType = "CrmEntityReference";
					break;
				case AttributeTypeCode.Uniqueidentifier:
					dataType = "Guid";
					break;
				//	TODO: Picklists and PartyLists
				case AttributeTypeCode.Picklist:
				case AttributeTypeCode.State:
				case AttributeTypeCode.Status:
					dataType = "int";
					isPicklist = true;
					break;
				case AttributeTypeCode.PartyList:
					dataType = "List<CrmEntity>";
					break;
			}

			if (isPicklist)
				entityAttributeLine += ", IsPicklist = true";
			entityAttributeLine += ")]";

			var displayName = GetDisplayName(attribute, fieldNames);

			tw.WriteLine("\t\tprivate {0} _{1};", dataType, displayName);
			tw.WriteLine("\t\t/// <summary>");
			tw.WriteLine("\t\t/// {0}", attribute.LogicalName);
			tw.WriteLine("\t\t/// </summary>");
			tw.WriteLine(entityAttributeLine);
			tw.WriteLine("\t\tpublic {0} {1} {{ get {{ return _{1}; }} set {{ _{1}=value; AddUpdatedAttribute(\"{2}\", value); }} }} // {3} - {4} - {5}", dataType, displayName, attribute.LogicalName, attribute.AttributeType, attribute.SchemaName, attribute.RequiredLevel.Value);
			tw.WriteLine();
		}

		private static void WriteIdField(TextWriter tw, AttributeMetadata attribute)
		{
			tw.WriteLine("\t\tprivate Guid _id;");
			tw.WriteLine("/// <summary>");
			tw.WriteLine("/// {0}", attribute.LogicalName);
			tw.WriteLine("/// </summary>");
			tw.WriteLine("\t\t[CrmField(\"{0}\", true)]", attribute.LogicalName);
			tw.WriteLine("\t\tpublic {0} {1} {{ get; set; }} // {2} - {3} - {4}", "Guid", "Id", attribute.AttributeType, attribute.SchemaName, attribute.RequiredLevel.Value);
			tw.WriteLine();
		}

		public static string GetDisplayName(Microsoft.Xrm.Sdk.Label label, string defaultName)
		{
			var retval = GetLabel(label, defaultName);

			retval = CleanLabel(retval);

			return retval;
		}

		private static string GetDisplayName(AttributeMetadata attribute, List<string> fieldNames)
		{
			string schemaName = attribute.SchemaName;
			Microsoft.Xrm.Sdk.Label label = attribute.DisplayName;
			string defaultName = attribute.LogicalName;
			var retval = string.Empty;

			if (attribute.IsCustomAttribute == true)
				retval = GetLabel(label, defaultName);

			if (string.IsNullOrEmpty(retval))
				retval = !string.IsNullOrEmpty(schemaName) ? schemaName : GetLabel(label, defaultName);

			retval = CleanLabel(retval);

			retval = MakeUnique(fieldNames, retval);

			fieldNames.Add(retval.ToLower());

			return retval;
		}

		private static string MakeUnique(List<string> fieldNames, string retval)
		{
			if (fieldNames.Contains(retval.ToLower()))
			{
				retval = retval + (fieldNames.Count(f => f.Contains(retval.ToLower())) + 1).ToString();
			}
			return retval;
		}

		private static string CleanLabel(string label)
		{
			if (label.IndexOf('(') > 0)
				label = label.Substring(0, label.IndexOf('('));

			label = label
				.Replace(" ", "")
				.Replace("?", "")
				.Replace("-", "")
				.Replace("_", "")
				.Replace(".", "")
				.Replace("\\", "")
				.Replace("/", "")
				.Replace(":", "")
				.Replace("'", "")
				.Replace("%", "Percentage");

			if (label.StartsWith("0") ||
				label.StartsWith("1") ||
				label.StartsWith("2") ||
				label.StartsWith("3") ||
				label.StartsWith("4") ||
				label.StartsWith("5") ||
				label.StartsWith("6") ||
				label.StartsWith("7") ||
				label.StartsWith("8") ||
				label.StartsWith("9"))
				label = "_" + label;

			return label;
		}

		private static string GetLabel(Microsoft.Xrm.Sdk.Label label, string defaultName)
		{
			return (label.UserLocalizedLabel != null) ? label.UserLocalizedLabel.Label : (label.LocalizedLabels != null && label.LocalizedLabels.Count > 0) ? label.LocalizedLabels[0].Label : defaultName;
		}
	}
}
