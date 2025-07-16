using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common;
using Azuro.Common.Configuration;
using System.Reflection;
using Microsoft.Xrm.Sdk;
using Azuro.Crm.KaseyaIntegration.v2011.Azuro_Kaseya_KaseyaWS;


namespace Azuro.Crm.KaseyaIntegration
{
	//TODO: Add tracing to the module
	public class KaseyaHelper
	{
		private decimal _sessionID = 0;

		private UpdateTicketRequest _updateTicketRequest;
		private UpdateTicketRequest UpdateTicketRequest { get { return _updateTicketRequest ?? (_updateTicketRequest = new UpdateTicketRequest()); } }

		private UpdateTicketResponse UpdateTicketResponse { get; set; }

		private AuthenticationRequest _authinticationRequest;
		private AuthenticationRequest AuthenticationRequest { get { return _authinticationRequest ?? (_authinticationRequest = new AuthenticationRequest()); } }

		private AuthenticationResponse AuthenticationResponse { get; set; }

		private GetTicketRequest _ticketRequest;
		private GetTicketRequest TicketRequest { get { return _ticketRequest ?? (_ticketRequest = new GetTicketRequest()); } }



		private KaseyaConfigurationSection _config = null;
		private KaseyaConfigurationSection Config { get { return _config ?? (_config = ConfigurationHelper.GetSection<KaseyaConfigurationSection>(KaseyaConfigurationSection.SectionName) ?? new KaseyaConfigurationSection()); } }

		private string _kaseyaWsUrl = null;
		private string KaseyaWsUrl { get { return _kaseyaWsUrl ?? (_kaseyaWsUrl = Config.KaseyaWsHost); } }

		private KaseyaWS _kaseysWS = null;
		private KaseyaWS KaseyaWS
		{
			get
			{
				if (_kaseysWS == null)
				{
					_kaseysWS = new KaseyaWS();
					_kaseysWS.Url = KaseyaWsUrl;
				}

				return _kaseysWS;
			}
		}

		private GetTicketListRequest _ticketListRequest;
		private GetTicketListRequest TicketListRequest { get { return _ticketListRequest ?? (_ticketListRequest = new GetTicketListRequest()); } }

		private GetTicketListResponse TicketListResponse { get; set; }

		public KaseyaHelper()
		{
		}

		public void ValidateSessionID(ref decimal sessionID)
		{
			if (sessionID == 0)
			{
				CreateSessionID();
				sessionID = _sessionID;

			}
			else 
			{
				_sessionID = sessionID;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filter"></param>
		/// <returns></returns>
		public List<T> GetEntityList<T>(List<KeyValuePair<string, string>> filter)
		{
			List<T> listOfEntities = new List<T>();
			//Check if not null, logic not yet included
			if (filter != null)
			{
				//Check if the is a way to filter down the tickets
			}
			else
			{
				GetKaseyaTicketList();
				if (!(string.IsNullOrEmpty(TicketListResponse.ErrorMessage)))
				{
					if (TicketListResponse.ErrorMessage.Contains("Session does not exist"))
					{
						//TODO: Log the error message or Session ID

						//This is just to indicate that failuture type for the main program
						throw new InvalidSessionIDException(TicketListResponse.ErrorMessage);
					}
					else
					{
						//TODO: Log the erro message
						throw new Exception(TicketListResponse.ErrorMessage);
					}
				}
				if (TicketListResponse.Tickets != null)
				{
					foreach (Ticket ticket in TicketListResponse.Tickets)
					{

						T item = Azuro.Common.Util.CreateObject<T>(typeof(T));
						var fullTicket = GetkaseyaTicket(ticket.TicketID);
						MapFields<T>(ref item, fullTicket);

						listOfEntities.Add(item);
					}
				}
				else
				{
					//TODO: Log to inform that the is a problem with Kaseya module
				}
			}
			return listOfEntities;
		}
		
		public void UpdateTicket(KaseyaTicket entity, string tempVal)
		{
			//TODO: Define update.

			TicketField field = new TicketField();
			field.Name = "Status";
			field.Value = "3";
			string tic_ID = entity.TicketID;
			UpdateTicketRequest.TicketID = Int32.Parse(tic_ID.Substring(tic_ID.IndexOf('-') + 1));
			UpdateTicketRequest.SessionID = _sessionID;

			UpdateTicketRequest.TicketFields = new TicketField[] { field };
			UpdateTicketResponse = KaseyaWS.UpdateTicket(UpdateTicketRequest);


			if (!(string.IsNullOrEmpty(UpdateTicketResponse.ErrorMessage)))
			{
				if (UpdateTicketResponse.ErrorMessage.Contains("Session does not exist"))
				{
					//TODO: Log the error message or Session ID

					//This is just to indicate that failuture type for the main program
					throw new InvalidSessionIDException(UpdateTicketResponse.ErrorMessage);
				}
				else
				{
					//TODO: Log the erro message
					throw new Exception(UpdateTicketResponse.ErrorMessage);
				}
			}

		}

		private void MapFields<T>(ref T item, object objectToAbstract)
		{
			foreach (PropertyInfo pi in item.GetType().GetProperties())
			{

				KaseyaFieldAttribute field = AttributeHelper.GetCustomAttribute<KaseyaFieldAttribute>(pi);
				if (field != null)
				{
					PropertyInfo absractionPI = objectToAbstract.GetType().GetProperty(field.Name);

					if (absractionPI != null)
					{
						var _value = absractionPI.GetValue(objectToAbstract, null);

						if (_value != null)
						{
							if (pi.Name == "TicketID")
								_value = "kaseyaticket-" + _value;

							pi.SetValue(item, _value, null);
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void GetKaseyaTicketList()
		{
			TicketListRequest.SessionID = _sessionID;
			TicketListResponse = KaseyaWS.GetTicketList(TicketListRequest);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ticketID"></param>
		/// <returns></returns>
		private GetTicketResponse GetkaseyaTicket(int ticketID)
		{
			TicketRequest.SessionID = _sessionID;
			TicketRequest.TicketID = ticketID;
			GetTicketResponse ticketResponse = KaseyaWS.GetTicket(TicketRequest);

			return ticketResponse;
		}


		/// <summary>
		/// 
		/// </summary>
		private void CreateSessionID()
		{
			try
			{
				string wsUsername = Config.KaseyaUsername;
				string wsPassword = Config.KaseyaPassword;
				string hashAlgorithm = Config.KaseyaHashAlgorithm;
				string randomNumber = Util.HashToCoveredPassword(wsUsername, ref wsPassword, hashAlgorithm);

				AuthenticationRequest.UserName = wsUsername;
				AuthenticationRequest.CoveredPassword = wsPassword;
				AuthenticationRequest.HashingAlgorithm = hashAlgorithm;
				AuthenticationRequest.RandomNumber = randomNumber;

				AuthenticationResponse = KaseyaWS.Authenticate(AuthenticationRequest);

				_sessionID = AuthenticationResponse.SessionID;
			}
			catch (System.Net.WebException exception)
			{
				System.Console.WriteLine(exception.Message);
				_sessionID = (decimal)0;
			}
		}


		public T ChangeEntityType<T>(object entity)
		{
			T newEntity = Azuro.Common.Util.CreateObject<T>(typeof(T));
			foreach (PropertyInfo pi in entity.GetType().GetProperties())
			{
				KaseyaFieldAttribute kaseyaAttr = AttributeHelper.GetCustomAttribute<KaseyaFieldAttribute>(pi);
				if (kaseyaAttr != null)
				{
					if (!(String.IsNullOrEmpty(kaseyaAttr.CrmFieldMapp)))
					{
						PropertyInfo merg_pi = newEntity.GetType().GetProperty(kaseyaAttr.CrmFieldMapp);
						if (merg_pi != null)
						{
							merg_pi.SetValue(newEntity, SafeTypeConvert(pi.GetValue(entity, null), merg_pi.PropertyType), null);
						}
					}
				}
			}

			return newEntity;
		}

		private static object SafeTypeConvert(object crmValue, Type type)
		{
			if (crmValue is Microsoft.Xrm.Sdk.OptionSetValue)
			{
				//	TODO: Properly Parse the Enum, just in case
				return ((Microsoft.Xrm.Sdk.OptionSetValue)crmValue).Value;
			}
			else if (crmValue is Microsoft.Xrm.Sdk.Money)
			{
				return ((Microsoft.Xrm.Sdk.Money)crmValue).Value;
			}
			else if (crmValue is Microsoft.Xrm.Sdk.EntityReference)
			{
				EntityReference er = (Microsoft.Xrm.Sdk.EntityReference)crmValue;
				return new Tuple<string, string, Guid>(er.LogicalName, er.Name, er.Id);
			}
			else
				return Azuro.Common.Util.ChangeType(crmValue, type);
		}

		public string GetAccountName()
		{
			return Config.KaseyaCrmAccountName ?? null;
		}
		public string GetContractName()
		{
			return Config.KaseyaCrmContact ?? null;
		}
		public string GetOrganisationID()
		{
			return Config.KaseyaCrmOrganisationID ?? null;
		}
	}
}
