using System.Globalization;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace xrm
{






	public class SolutionPublish
	{
		private IOrganizationService _service;
		public SolutionPublish(IOrganizationService service)
		{
			_service = service;
		}
		public void Execute() {

			var response = (PublishAllXmlResponse)_service.Execute(new PublishAllXmlRequest());
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			var settings = args.ToDictionary(key => key.Split(':').First(), val => val.Split(':').Last());

			string server   = settings.ContainsKey("server") ? settings["server"] : "localhost";
			string port     = settings.ContainsKey("port") ? settings["port"] : "5555";
 
			if (!settings.ContainsKey("org")) { Console.WriteLine("org parameter must be specified"); return; }

			string org = settings["org"];
			//bool increment = settings.ContainsKey("incrementversion") || settings.ContainsKey("increment") || settings.ContainsKey("inc");

			var creds = new ClientCredentials();

			creds.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;

			var uri = new Uri(string.Format("http://{0}:{1}/{2}/XRMServices/2011/Organization.svc",server,port,org));
			Console.WriteLine("Connecting...");
			using (var proxy = new OrganizationServiceProxy(uri, null, creds, null))
			{
				proxy.EnableProxyTypes();
				while (args != null && args.First() != "exit")
				{
					settings        = args.ToDictionary(key => key.Split(':').First(), val => val.Split(':').Last());
					string solution = settings.ContainsKey("sol") ? settings["sol"] : null;
					string filename = "";
					string output   = settings.ContainsKey("to") ? settings["to"] : Path.Combine(Directory.GetCurrentDirectory(), "Export");
					bool managed    = settings.ContainsKey("managed");

					switch (args.First())
					{
						case "delete":
							if (settings.ContainsKey("plugin"))
							{
								RetrieveEntityRequest plugin = new RetrieveEntityRequest();
							}

							if (settings.ContainsKey("attribute") || settings.ContainsKey("attr"))
							{
								var attributeRequest = new RetrieveAttributeRequest
									{
										EntityLogicalName = settings["entity"],
										LogicalName = settings["attribute"],
										RetrieveAsIfPublished = true
									};
								try
								{
									var response = (RetrieveAttributeResponse)proxy.Execute(attributeRequest);

									DeleteComponent(response.AttributeMetadata.MetadataId.Value,
													(int)componenttype.Attribute, proxy);
								}
								catch (Exception e)
								{
									Console.WriteLine(e.Message);
								}
							}
							break;
						case "import":
							{

								var impSolReq = new ImportSolutionRequest
									{
										CustomizationFile = File.ReadAllBytes(Path.Combine(output, filename)),
										ImportJobId = Guid.NewGuid()
									};

								try
								{
									Console.WriteLine("Importing [{0}] ...", Path.Combine(output, filename));
									var response = proxy.Execute(impSolReq);
									Publish(proxy);
								}
								catch (Exception e)
								{
									Console.WriteLine("Error Importing Solution");
									Console.WriteLine(e.Message);
								}
							}
							break;
						case "export":
							{
								if (solution != null)
								{
									filename = solution + DateTime.Now.ToString("dd-MM-yy-hh-mm-ss") + ".zip";
								}

								Console.WriteLine("Exporting to {0}", solution);
								var export = new ExportSolutionRequest {Managed = managed, SolutionName = solution};

								var response = (ExportSolutionResponse) proxy.Execute(export);

								byte[] exportXml = response.ExportSolutionFile;

								if (Directory.Exists(output) == false)
								{
									Directory.CreateDirectory(output);
								}
								File.WriteAllBytes(Path.Combine(output, filename), exportXml);
								Console.WriteLine("Solution exported to {0}", output);
							}
							break;
						case "push":
							break;
						case "pub":
						case "publish":
							Publish(proxy);
							break;
						default:
							{
								Console.WriteLine("Valid commands include:");
								Console.WriteLine("import");
								Console.WriteLine("export");
								Console.WriteLine("publish");
								Console.WriteLine("delete");
								Console.WriteLine("push (coming soon!)");
								//Console.WriteLine("Unrecognized Command: {0}", args.FirstOrDefault());
								break;
							}
					}

					Console.Write(">");
					//Keeps the connection open (during dev) making publish quicker
					string input = Console.ReadLine();
					if (input != null)
					{
						args = input.Split(' ');                        
					}
				}
			}



		}

		private static componenttype WriteToConsole(Entity dependency, out Guid rid)
		{
			var rct = (componenttype) dependency.GetAttributeValue<OptionSetValue>("requiredcomponenttype").Value;
			var dct = (componenttype) dependency.GetAttributeValue<OptionSetValue>("dependentcomponenttype").Value;
			var dt = (componenttype) dependency.GetAttributeValue<OptionSetValue>("dependencytype").Value;
			var ot = dependency.GetAttributeValue<Guid>("dependentcomponentobjectid");
			rid = dependency.GetAttributeValue<Guid>("requiredcomponentobjectid");

			Console.WriteLine("Req. Component Type: {0}", rct);
			Console.WriteLine("Dep. Component Type: {0}", dct);
			Console.WriteLine("Dep. Type:           {0}", dt);
			return rct;
		}

		private static void DeleteComponent(Guid id, int type, OrganizationServiceProxy proxy)
		{
			var deps = new RetrieveDependenciesForDeleteRequest
			{
				ComponentType = type,
				ObjectId = id
			};
			var result = (RetrieveDependenciesForDeleteResponse)proxy.Execute(deps);

			foreach (var dependency in result.EntityCollection.Entities)
			{
				Guid rid;
				var rct = WriteToConsole(dependency, out rid);

				DeleteComponent(dependency.GetAttributeValue<Guid>("dependentcomponentobjectid"),
								dependency.GetAttributeValue<OptionSetValue>("dependentcomponenttype").Value, proxy);
			}

			switch ((componenttype)type)
			{
				case componenttype.EntityRelationship:
					{
						var res = proxy.Execute(new RetrieveRelationshipRequest {MetadataId = id});

						var rel = res.Results.First().Value as OneToManyRelationshipMetadata;
						if (rel != null)
						{
							proxy.Execute(new DeleteRelationshipRequest { Name = rel.SchemaName});
						}

						break;
					}
					case componenttype.Attribute:
					{

						var attr = new RetrieveAttributeRequest();
						attr.MetadataId = id;
						var attributeMetadata = (RetrieveAttributeResponse)proxy.Execute(attr);
						var deleted = proxy.Execute(new DeleteAttributeRequest()
							{

								EntityLogicalName = attributeMetadata.AttributeMetadata.EntityLogicalName,
								LogicalName = attributeMetadata.AttributeMetadata.LogicalName
							});
						break;
					}
					case componenttype.SDKMessageProcessingStep:
					case componenttype.SDKMessageProcessingStepImage:
					{
						var deleted = proxy.Execute(new DeleteRequest()
							{
								Target = new EntityReference()
									{
										Id = id,
										LogicalName = ((componenttype)type).ToString().ToLower(),
									}

							});

						break;
					}
				default:
					{
						throw new NotSupportedException("Dependency hasn't been configured for delete");
					}
			}

		}

		private static void Publish(OrganizationServiceProxy proxy)
		{
			Console.WriteLine("Publishing all changes...");
			new SolutionPublish(proxy).Execute();
			Console.WriteLine("Complete");
		}
	}
}
