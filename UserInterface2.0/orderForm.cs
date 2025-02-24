﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL;
using Entities;

namespace UserInterface2._0
{
    public partial class orderForm : Form
    {
        OrderBLL orderBLL = new OrderBLL();
        ProductBLL productBLL = new ProductBLL(0);

        //for use in modify etc- allows computer to temporarily define a list containing either customer orders or product orders so that we can use it in a few different methods ie modify delete and print
        List<Order> temporaryStorage = new List<Order>();
        public orderForm()
        {
            InitializeComponent();

        }

        private void buttonReturnMenu_Click(object sender, EventArgs e)
        {
            ResetMainMenu();
            HideEverything();
        }

        private void ResetMainMenu()
        {
            labelOrderTitle.Text = "Order Menu";
            buttonCreate.Visible = true;
            buttonReadOne.Visible = true;
            buttonReadAll.Visible = true;
            buttonReturnMenu.Visible = false;
        }

        public void HideEverything()
        {
            //things to disappear for add
            groupBoxPlaceOrder.Visible = false;
            listBoxProducts.Visible = false;
            buttonReturnMenu.Visible = false;
            labelAllProducts.Visible = false;


            //things to disappear for readone
            listBoxOrdersFound.Visible = false;
            labelOrderDetails.Visible = false;
            panelIDInput.Visible = false;
            buttonFindOrders.Enabled = false;
            listBoxProducts.Items.Clear();
            listBoxPrintOrders.Items.Clear();
            listBoxPrintOrders.Visible = false;
            textBoxIDInput.Enabled = false;
            buttonDelete.Visible = false;
            buttonModify.Visible = false;
            numericUpDown1.Visible = false;
            labelAmountModify.Visible = false;
            groupBoxModifyOrder.Visible = false;
            IDChooser.SelectedIndex = -1;
            labelInstructions.Visible = false;
        }

        public void HideMainMenu()
        {
            buttonCreate.Visible = false;
            buttonReadOne.Visible = false;
            buttonReadAll.Visible = false;
        }

        //opens the create new order page
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            HideMainMenu();
            labelOrderTitle.Text = "Place Order";
            listBoxProducts.Visible = true;

            listBoxProducts.Text = ""; //need readall function to work first
            labelAllProducts.Visible = true;
            groupBoxPlaceOrder.Visible = true;
            buttonReturnMenu.Visible = true;
            labelOrderNumber.Text = "Order Number " + Convert.ToString(orderBLL.GetOrderNumber());
            PrintAllProducts();
        }


        private void buttonPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                orderBLL.Create(int.Parse(textBoxCustomerID.Text), int.Parse(textBoxProductNumber.Text), Convert.ToInt32(numericUpDownAmount.Value));
                MessageBox.Show("Thank you for shopping at Toys of All Sorts!");
                labelOrderNumber.Text = "Order Number " + Convert.ToString(orderBLL.GetOrderNumber());
                //update listbox to show new stock numbers (waiting on ReadAll)
                listBoxProducts.Items.Clear();
                PrintAllProducts();
                textBoxCustomerID.Clear();
                textBoxProductNumber.Clear();
            }
            catch (ProductNumberNotFound)
            {
                MessageBox.Show("Product Not Found");
            }
            catch (CustomerNotFound)
            {
                MessageBox.Show("Customer not found");
            }
            catch
            {
                MessageBox.Show("Please fill in all textboxes");
            }

        }

        private void buttonReadOne_Click(object sender, EventArgs e)
        {
            HideMainMenu();
            labelOrderTitle.Text = "Find Orders";
            buttonReturnMenu.Enabled = true;
            buttonReturnMenu.Visible = true;
            panelIDInput.Visible = true;
            buttonFindOrders.Enabled = true;
            IDChooser.Enabled = true;
            textBoxIDInput.Enabled = true;
            labelOrderDetails.Visible = true;
            listBoxOrdersFound.Visible = true;
            listBoxOrdersFound.Text = null;
            textBoxIDInput.Text = null;
            listBoxOrdersFound.Items.Clear();
            IDChooser.Text = "By:";
            textBoxIDInput.Enabled = false;
            IDChooser.SelectedIndex = -1;
            buttonDelete.Visible = true;
            buttonModify.Visible = true;
            numericUpDown1.Visible = true;
            labelAmountModify.Visible = true;

        }

        private void buttonReadAll_Click(object sender, EventArgs e)
        {
            HideMainMenu();
            labelOrderTitle.Text = "All Orders";
            buttonReturnMenu.Visible = true;
            listBoxPrintOrders.Visible = true;
            listBoxPrintOrders.Size = new System.Drawing.Size(694, 429); //resize text box to make room for place order groupbox
            PrintAllOrders();

        }

        private void orderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonReturnMenu_Click(sender, e);
            FormProvider.MainMenu.Show();
        }

        private void PrintAllProducts()
        {
            List<Product> products = productBLL.ReadAll();
            for (int i = 0; i < products.Count; i++)
            {
                listBoxProducts.Items.Add(products[i]);
            }

        }

        private void PrintAllOrders()
        {
            try
            {
                List<Order> orders = orderBLL.ReadAll();
                for (int i = 0; i < orders.Count; i++)
                {
                    listBoxPrintOrders.Items.Add(orders[i]);
                }
            }
            catch
            {
                MessageBox.Show("No orders found.");
            }

        }

        private void listBoxProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int listBoxIndex = listBoxProducts.SelectedIndex;
                List<Product> products = productBLL.ReadAll();
                textBoxProductNumber.Text = Convert.ToString(products[listBoxIndex].ProductNumber);
            }
            catch { }

        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Toys of All Sorts is an abstract toy company by Shira Laury and Chani Wachsstock");
        }

        private void buttonFindOrders_Click(object sender, EventArgs e)
        {
            listBoxOrdersFound.Items.Clear();
            try
            {

                if (IDChooser.SelectedIndex >= 0)
                {
                    if (IDChooser.SelectedIndex == 0)
                    {
                        CustomerOrdersFoundPrint(int.Parse(textBoxIDInput.Text));
                    }
                    else if (IDChooser.SelectedIndex == 1)
                    {
                        ProductOrdersFoundPrint(int.Parse(textBoxIDInput.Text));
                    }
                    else if (IDChooser.SelectedIndex == 2)
                    {
                        temporaryStorage = new List<Order>();
                        Order currentOrder = orderBLL.ReadOrderViaOrderNum(int.Parse(textBoxIDInput.Text));
                        temporaryStorage.Add(currentOrder);
                        listBoxOrdersFound.Items.Add(currentOrder);
                        groupBoxModifyOrder.Visible = true;
                        groupBoxModifyOrder.Text = "Order Number " + currentOrder.OrderNumber;
                        listBoxOrdersFound.SelectedIndex = 0;
                    }

                }




            }
            catch (ExceptionCustomerHasNoOrders ex)
            {
                MessageBox.Show("No orders listed under this customer ID. Please make sure this is a valid ID, or place a new order.", "Error");
            }
            catch (ExceptionProductHasNoOrders ex)
            {
                MessageBox.Show("No orders listed under this product ID. Please make sure this is a valid ID, or place a new order.", "Error");
            }
            catch (ExceptionOrderNumInvalid)
            {
                MessageBox.Show("Invalid ID number", "Error");
            }
            catch
            {
                MessageBox.Show("Please enter an ID");
            }

        }

        private void CustomerOrdersFoundPrint(int customerID)
        {

            temporaryStorage = orderBLL.ReadOrderViaCustomer(customerID);

            for (int i = 0; i < temporaryStorage.Count; i++)
            {
                listBoxOrdersFound.Items.Add(temporaryStorage[i]);
            }
            labelInstructions.Visible = true;
        }
        private void ProductOrdersFoundPrint(int productID)
        {

            temporaryStorage = orderBLL.ReadOrderViaProduct(productID);
            for (int i = 0; i < temporaryStorage.Count; i++)
            {
                listBoxOrdersFound.Items.Add(temporaryStorage[i]);
            }
            labelInstructions.Visible = true;
        }


        //makes sure a selection is made by user. Baruch Hashem, the DomainUpDown doesn't let you go back to default position  

        private void IDChooser_SelectedItemChanged(object sender, EventArgs e)
        {
            textBoxIDInput.Enabled = true;
            listBoxOrdersFound.Items.Clear();
            textBoxIDInput.Clear();
            groupBoxModifyOrder.Visible = false;
            labelInstructions.Visible = false;
        }

        private void listBoxOrdersFound_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                groupBoxModifyOrder.Text = "Order Number " + temporaryStorage[listBoxOrdersFound.SelectedIndex].OrderNumber;
                groupBoxModifyOrder.Visible = true;
                numericUpDown1.Value = temporaryStorage[listBoxOrdersFound.SelectedIndex].AmountOrdered;
                buttonModify.Enabled = false;
            }
            catch { }

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            buttonModify.Enabled = true;
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            try
            {
                int orderNum;
                if (IDChooser.SelectedIndex == 2)
                {
                    orderNum = int.Parse(textBoxIDInput.Text);
                }
                else
                {
                    orderNum = temporaryStorage[listBoxOrdersFound.SelectedIndex].OrderNumber;
                }
                int amountToOrder = Convert.ToInt32(numericUpDown1.Value);
                orderBLL.Update(orderNum, amountToOrder);
                MessageBox.Show("Order Modified");
                buttonFindOrders_Click(sender, e);
                buttonModify.Enabled = false;
                groupBoxModifyOrder.Visible = false;
            }
            catch
            {
                MessageBox.Show("Not enough in stock.");//why is this an exception? How  would a user give in vlaid info?
                                                        //They need to click the item for it to even show modify!
                                                        //if they try to request more than what is in stock:)
            }

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int orderNum;

            orderNum = temporaryStorage[listBoxOrdersFound.SelectedIndex].OrderNumber;


            orderBLL.Delete(orderNum);
            temporaryStorage.Remove(temporaryStorage[listBoxOrdersFound.SelectedIndex]);
            if (temporaryStorage.Count != 0)
            {
                buttonFindOrders_Click(sender, e);
            }
            else
            {
                listBoxOrdersFound.Items.Clear();
            }
            if (temporaryStorage.Count == 1)
            {
                listBoxOrdersFound.SelectedIndex = 0;
            }
            else
            {
                groupBoxModifyOrder.Visible = false;
            }

        }

        private void listBoxPrintOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                temporaryStorage = orderBLL.ReadAll();
                int orderNum = temporaryStorage[listBoxPrintOrders.SelectedIndex].OrderNumber; ;
                buttonReturnMenu_Click(sender, e);
                buttonReadOne_Click(sender, e);
                IDChooser.SelectedIndex = 2;
                textBoxIDInput.Text = Convert.ToString(orderNum);
                buttonFindOrders_Click(sender, e);
            }
            catch
            {

            }


        }
    }
}
