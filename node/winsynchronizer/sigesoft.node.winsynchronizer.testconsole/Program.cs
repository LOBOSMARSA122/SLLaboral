using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sigesoft.Node.WinClient.DAL;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using System.Linq.Dynamic;
using System.Xml.Serialization;
using System.IO;

namespace Sigesoft.Node.WinSynchronizer.TestConsole
{
    class Program
    {
        public static DateTime UFS = new DateTime(2012, 12, 30);
        
        static void Main(string[] args)
        {
            // Leer las tablas
            Serialize_applicationhierarchy();
            Serialize_datahierarchy();
            Serialize_systemparameter();
            Console.WriteLine("Fin de la serialización");

            Console.WriteLine("De-Serialización");
            Deserialize_datahierarchy();

            Console.WriteLine("Fin de la aplicación");
            Console.ReadLine();
        }

        static void Serialize_datahierarchy()
        {            
            // datahierarchy
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            Type mitipo = typeof(datahierarchy);

            var query_created =
                        from a in dbContext.datahierarchy
                        where a.d_InsertDate > UFS
                        orderby a.i_GroupId, a.i_ItemId
                        select a;

            var query_updated =
                        from a in dbContext.datahierarchy
                        where a.d_UpdateDate > UFS
                        orderby a.i_GroupId, a.i_ItemId
                        select a;

            Create(query_created, @"d:\list_datahierarchy_created.xml");
            Create(query_updated, @"d:\list_datahierarchy_updated.xml");

            var objDataListDtoCreated = query_created.ToList().ToDTOs();
            var objDataListDtoUpdated = query_updated.ToList().ToDTOs();

            Serialize(objDataListDtoCreated, @"d:\list_datahierarchy_created.xml");
            Serialize(objDataListDtoUpdated, @"d:\list_datahierarchy_updated.xml");

            //foreach (var item in objDataListDto)
            //{
            //    Console.WriteLine(string.Format("{0} / {1} / {2} / {3} / {4}", item.i_GroupId, item.i_ItemId, item.v_Value1, item.d_InsertDate, item.d_UpdateDate));
            //}
            
        }

        static void Create(IQueryable<datahierarchy> query, string file)
        {
            List<datahierarchy> objDataList = query.ToList();
            //objDataListDto = objDataList.ToDTOs();
            //List<datahierarchyDto> objDataListDto = datahierarchyAssembler.ToDTOs(objDataList);

            XmlSerializer serializer = new XmlSerializer(objDataList.GetType());
            using (StreamWriter writer = new StreamWriter(file))
            {
                serializer.Serialize(writer, objDataList);
            }
        }

        static void Serialize(object data, string file)
        {
            XmlSerializer serializer = new XmlSerializer(data.GetType());
            using (StreamWriter writer = new StreamWriter(file))
            {
                serializer.Serialize(writer, data);
            }

        }

        static void Deserialize_datahierarchy()
        {
            // datahierarchy
            List<datahierarchyDto> obj = null;

            XmlSerializer serializer = new XmlSerializer(typeof(List<datahierarchyDto>));
            using (StreamReader reader = new StreamReader(@"d:\list_datahierarchy_created.xml"))
            {
                obj = (List<datahierarchyDto>) serializer.Deserialize(reader);
            }

            foreach (var item in obj)
            {
                Console.WriteLine(string.Format("{0} / {1} / {2} / {3} / {4}", item.i_GroupId, item.i_ItemId, item.v_Value1, item.d_InsertDate, item.d_UpdateDate));
            }

        }

        static void Serialize_applicationhierarchy()
        {
            // applicationhierarchy
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            IQueryable<applicationhierarchy> query =
                        from a in dbContext.applicationhierarchy
                        where a.d_InsertDate > UFS
                        select a;


            var objDataList = query.ToList();
            var objDataListDto = objDataList.ToDTOs();

            XmlSerializer serializer = new XmlSerializer(objDataListDto.GetType());
            using (StreamWriter writer = new StreamWriter(@"d:\list_applicationhierarchy.xml"))
            {
                serializer.Serialize(writer, objDataListDto);
            }
        }

        static void Serialize_systemparameter()
        {
            // applicationhierarchy
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var query =
                        from a in dbContext.systemparameter
                        where a.d_InsertDate > UFS
                        select a;


            var objDataList = query.ToList();
            var objDataListDto = objDataList.ToDTOs();

            XmlSerializer serializer = new XmlSerializer(objDataListDto.GetType());
            using (StreamWriter writer = new StreamWriter(@"d:\list_systemparameter.xml"))
            {
                serializer.Serialize(writer, objDataListDto);
            }
        }

    }
}
