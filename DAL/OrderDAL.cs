﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.IO;

namespace DAL
{
    public class OrderDAL
    {
        internal List<Order> orderList = new List<Order>();
        int orderNumber = 100;

        public OrderDAL()
        {
            InitilizeList();
        }
        public void InitilizeList()
        {
            //StreamReader reader = new StreamReader(@"../../bin/debug/ListOfOrders.txt");
            //string line;
            //using(reader)
            //{
            //    for (int i = 0; (line = reader.ReadLine()) != null; i++)
            //    {
            //        string [] array =line.Split(' ');
            //        Order sale1 = new Order(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));
            //        orderList.Add(sale1);
            //        Console.WriteLine(orderNumber);
            //        Console.WriteLine(transaction.customerID);
            //        Console.WriteLine(transaction.productID);
            //    }
            //}
        }

        public void Create(int customerID, int productID)
        {

            Order order = new Order(orderNumber, customerID, productID);
            orderList.Add(order);
            orderNumber += 100;
        }

        public void Delete(int anOrderNumber)
        {
            for (int i = 0; i < orderList.Count; i++) 
            {
                if (anOrderNumber == orderList[i].OrderNumber)//checks if we've found the sought-for product number
                {

                    orderList.Remove(orderList[i]);
                    break;
                }
                if (i == orderList.Count)//if never found product number, it throws an exception.
                {
                    throw new ProductNumberNotFound();
                }
            }
        }

        public void Update(int anOrderNumber, int customerID, int productID)
        {
            Delete(anOrderNumber);
            Order order = new Order(anOrderNumber, customerID, productID);
            orderList.Add(order);
        }

        
        //no, you can read in a customer id OR productiD and then all you do is check if the order exists based on the ORDER PROPERTIES of customer id and product id- you aren't supposed to be referencing the customer list or product list at all. 
         public Order ReadOrderViaCustomer(int customerID)
         {
            try
            {
                int i;
                for (i = 0; i <orderList.Count; i++)
                {
                    if (orderList[i].customerID == customerID)
                        break;
                }
                   

                Order sale = orderList[i];
                return sale;
            }
             catch
            {
                throw new Exception("No orders for this customer");
            }
         }

        public Order ReadOrderViaProduct(int productID)
        {
           
            try
            {
                int i;
                for (i = 0; i < orderList.Count; i++)
                {
                    if (orderList[i].productID == productID)
                        break;
                }


                Order sale = orderList[i];
                return sale;
            }
            catch
            {
                throw new Exception("No orders for this customer");
            }
        }



        //public Order ReadOrderViaOrder(int orderNum)
        //{
        //    try
        //    {
        //        int i;
        //        for (i = 0; i < orderList.Count; i++)
        //        {
        //            if (orderList[i].OrderNumber == orderNumber)
        //                break;
        //        }


        //        Order sale = orderList[i];
        //        return sale;
        //    }
        //    catch
        //    {
        //        throw new Exception("No orders for this customer");
        //    }
        //}
        //public Order ReadOne(int productID)
        //{
        //    try
        //    {
        //        Order sale = new Order(transaction.OrderNumber, transaction.customerID, productID);
        //    }
        //    catch
        //    {
        //        throw new Exception("No orders found for this product");
        //    }
        //}



        //public List<Order> ReadALl()
        //{
        //    if(orderList.Count > 0)
        //    {
        //        List<Order> copy_list = orderList.ConvertAll(user => new Order(transaction.OrderNumber,  ));
        //        return copy_list;
        //    }
        //    else
        //    {
        //        throw new Exception("No order yet!");
        //    }

        //}


    }
}
