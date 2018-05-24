using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TareaIngenieria.Models;

namespace TareaIngenieria.Controllers
{
    [Route("Inventory")]
    public class ProductosController : Controller
    {
        [HttpGet("Stock")]
        public IActionResult Index()
        {
            IList<ConsultaVentas> lista = new List<ConsultaVentas>();
            ConsultaVentas respuesta = null;
            string connectionString =
             "Data Source=192.168.1.115\\devbc;Initial Catalog=AdventureWorks2014; User Id = sa; Password=1234";

            string queryString = @"SELECT top 10 
                                        P.ProductID,
                                        P.Name as ProductName, 
                                        SUM(PIV.Quantity) AS Stock, 
                                        SUM(SOD.OrderQty) AS QuantitySold,
                                        MAX(SOH.OrderDate) AS LastSoldDate,
                                            (select TOP 1 pe.LastName + ' ' + pe.FirstName 
                                                from Sales.SalesOrderDetail sd 
                                                inner join Sales.SalesOrderHeader soh on sd.SalesOrderID =soh.SalesOrderID
                                                inner join Sales.Customer c on soh.CustomerID=c.CustomerID
                                                inner join Person.Person pe on c.PersonID=pe.BusinessEntityID 
                                                where ProductID = P.ProductID
                                                GROUP BY pe.LastName + ' ' + pe.FirstName 
                                                ORDER BY COUNT(1) DESC) as BestCustomer
                                                FROM Production.ProductInventory PIV
                                                INNER JOIN Production.Product P ON PIV.ProductID=P.ProductID 
                                                INNER JOIN Sales.SalesOrderDetail SOD ON P.ProductID = SOD.ProductID 
                                                INNER JOIN Sales.SalesOrderHeader SOH ON SOD.SalesOrderID = SOH.SalesOrderID 
                                                GROUP By  P.ProductID, P.Name
                                                ORDER By  Stock asc, QuantitySold desc";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        respuesta = new ConsultaVentas() { ProductID = Convert.ToInt16(lector.GetValue(0)),
                            ProductName = lector.GetValue(1).ToString(),
                            Stock = Convert.ToInt32(lector.GetValue(2)),
                            QuantitySold = Convert.ToInt32(lector.GetValue(3)),
                            LastSoldDate = Convert.ToDateTime(lector.GetValue(4)),
                            BestCustomer = lector.GetValue(5).ToString()};
                        lista.Add(respuesta);
                    }
                    lector.Close();
                    return Ok(lista);
                    
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
        }

        [HttpGet("query/stock")]
        public IActionResult otroMetodo()
        {
            IList<ConsultaStock> lista = new List<ConsultaStock>();
            ConsultaStock respuesta = null;
            string connectionString =
            "Data Source=192.168.1.115\\devbc;Initial Catalog=AdventureWorks2014; User Id = sa; Password=1234";

            string queryString = @"SELECT
                                        PCS.ProductSubcategoryID, 
                                        PCS.Name,
                                        SUM(CAST (WO.StockedQty AS bigint)) AS WorkOrderQty,
                                        SUM(CAST (WR.ActualCost*WO.OrderQty AS bigint)) AS WorkOrderCost,
                                        SUM(CAST (POD.OrderQty AS bigint)) PurchaseOrderQty,
                                        SUM(CAST (POD.UnitPrice*POD.OrderQty AS bigint)) AS PurchaseOrderCost 
                                        FROM Purchasing.PurchaseOrderDetail POD
                                        INNER JOIN Production.Product P ON POD.ProductID=POD.ProductID
                                        INNER JOIN Production.ProductSubcategory PCS ON P.ProductSubcategoryID=PCS.ProductSubcategoryID
                                        INNER JOIN Production.WorkOrder WO ON P.ProductID=WO.ProductID
                                        INNER JOIN Production.WorkOrderRouting WR ON WR.WorkOrderID = WO.WorkOrderID
                                        where year(WO.StartDate)=2014
                                        GROUP BY PCS.ProductSubcategoryID, PCS.Name;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(queryString, connection);


                try
                {
                    connection.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        respuesta = new ConsultaStock() { ProductSubcategoryID = Convert.ToInt32(lector.GetValue(0)),
                            Name = lector.GetValue(1).ToString(),
                            WorkOrderQty = Convert.ToInt32(lector.GetValue(2)),
                            WorkOrderCost = Convert.ToInt32(lector.GetValue(3)),
                            PurchaseOrderQty = Convert.ToInt32(lector.GetValue(4)),
                            PurchaseOrderCost = Convert.ToInt32(lector.GetValue(5))
                        };
                        lista.Add(respuesta);
                    }
                    lector.Close();
                    return Ok(lista);
                    
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
        }

        [HttpGet("query/products/procedure")]
        public IActionResult consultaConProcedimientos()
        {
            List<ConsultaProducto> lista = new List<ConsultaProducto>();
            ConsultaProducto respuesta = null;
            string connectionString =
             @"Data Source=DESKTOP-MG3N9KE;Initial Catalog=AdventureWorks2017;User Id = ronald; Password=ronald2016";

            string queryString = "dbo.obtenerMasVendidos";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(queryString, connection);
                comando.CommandType = System.Data.CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        respuesta = new ConsultaProducto() { ProductId = (int)lector[0], Descripcion = (string)lector[1], CantidadVendida = (int)lector[2], Total = (decimal)lector[3] };
                        lista.Add(respuesta);
                    }
                    lector.Close();
                    return Ok(lista);

                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
        }

        [HttpGet("query/products/procedure2")]
        public IActionResult consultaConProcedimientos2()
        {
            List<ConsultaProducto> lista = new List<ConsultaProducto>();
            ConsultaProducto respuesta = null;
            string connectionString =
             @"Data Source=DESKTOP-MG3N9KE;Initial Catalog=AdventureWorks2017;User Id = ronald; Password=ronald2016";

            string queryString = "dbo.obtenerIngresoProductos";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(queryString, connection);
                comando.CommandType = System.Data.CommandType.StoredProcedure;


                try
                {
                    connection.Open();
                    SqlDataReader lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        respuesta = new ConsultaProducto() { ProductId = (int)lector[0], Descripcion = (string)lector[1], Total = (decimal)lector[2] };
                        lista.Add(respuesta);
                    }
                    lector.Close();
                    return Ok(lista);

                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
        }
    }
}