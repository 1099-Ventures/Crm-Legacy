using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.Crm.KaseyaIntegration
{
	class Program
	{
		public enum test
		{
		

		}
		public void hhMain()
		{
			
		}
		static void Main(string[] args)
		{
			KaseyaCrmHelper crmhelper = (KaseyaCrmHelper)Azuro.Crm.Integration.CrmHelperFactory.Create(new Guid("129D15E7-8985-E211-8243-00155D004301"));
			crmhelper.OrganisationId = new Guid("129D15E7-8985-E211-8243-00155D004301");

			KaseyaHelper kaseyaHelper = new KaseyaHelper();
			//List<Azuro.Kaseya.Entities.TestUser> user_list = crmhelper.GetEntityList<Azuro.Kaseya.Entities.TestUser>(null);
			List<KaseyaTicket> list_tickets = kaseyaHelper.GetEntityList<KaseyaTicket>(null);
		

			//
			foreach(KaseyaTicket ticket in  list_tickets)
			{
				var convertEntity = kaseyaHelper.ChangeEntityType<KaseyaCase>(ticket);

				//Covert ID fields to the Guids

				//convertEntity.customerid = new Guid("f45d2fc5-9e85-e211-8243-00155d004301");
				crmhelper.InsertCaseEntity<KaseyaCase>(convertEntity);
			}
			
			Console.ReadLine();
		}
	}
}
