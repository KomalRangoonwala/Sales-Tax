using System;
using System.Linq;
using System.Collections.Generic;

// ---------------------------------------------------
// Author: Komal Rangoonwala for Makkajai Dev Challenge
// Date: 19 Apr 2021
// ---------------------------------------------------

// A structure to hold and manage the data
public class Program
{
	public class Order
	{
		List<Product> objList = new List<Product>();
		public Order(List<Product> objList_)
		{
			this.objList = objList_;
		}
	}
	
	public class Product
	{
		public string Name = string.Empty;
		public double Price = 0;
		public int Quantity = 0;
		
		public Product(string Name_, double Price_, int Quantity_)
		{
			this.Name = Name_;
			this.Price = Price_;
			this.Quantity = Quantity_;
		}
	}
	
	#region Input methods
	// Method to test some fixed inputs as given in the question
	public static List<Product> GetFixedInput(int TestCase)
	{
		List<Product> lstProducts = new List<Product>();
		switch(TestCase)
		{
			case 1:
				Product objProduct1 = new Product("book", 12.49, 1);
				lstProducts.Add(objProduct1);
				
				Product objProduct2 = new Product("music CD", 14.99, 1);
				lstProducts.Add(objProduct2);
				
				Product objProduct3 = new Product("chocolate bar", 0.85, 1);
				lstProducts.Add(objProduct3);
				break;
				
			case 2:
				Product objProduct4 = new Product("imported box of chocolates", 10.00, 1);
				lstProducts.Add(objProduct4);
				
				Product objProduct5 = new Product("imported bottle of perfume", 47.50, 1);
				lstProducts.Add(objProduct5);				
				break;
				
			case 3:
				Product objProduct6 = new Product("imported bottle of perfume", 27.99, 1);
				lstProducts.Add(objProduct6);
				
				Product objProduct7 = new Product("bottle of perfume", 18.99, 1);
				lstProducts.Add(objProduct7);
				
				Product objProduct8 = new Product("packet of headache pills", 9.75, 1);
				lstProducts.Add(objProduct8);
				
				Product objProduct9 = new Product("box of imported chocolates", 11.25, 1);
				lstProducts.Add(objProduct9);
				break;
		}
		
		return lstProducts;
	}
	
	// Method to test user input
	public static List<Product> GetUserInput()
	{
		Console.WriteLine("Enter the number of products: ");
		int intTotalProducts = Convert.ToInt32(Console.ReadLine());

		List<Product> lstProducts = new List<Product>();
		for(int i=1; i<=intTotalProducts; i++)
		{
			Console.WriteLine("#" + i.ToString() + " Enter product name: ");
			string strName = Console.ReadLine();
			
			Console.WriteLine("#" + i.ToString() + " Enter product price: ");
			double dblPrice = Convert.ToDouble(Console.ReadLine());
			
			Console.WriteLine("#" + i.ToString() + " Enter product quantity: ");
			int intQuantity = Convert.ToInt32(Console.ReadLine());
			
			Product objProduct = new Product(strName, dblPrice, intQuantity);
			lstProducts.Add(objProduct);
		}
		
		return lstProducts;
	}
	#endregion
		
	#region Main method
	// Main method where the execution begins
	public static void Main()
	{
		try
		{
			Console.WriteLine("Do you want to test fixed input or user input? Enter 1 for fixed system input, 2 for user input:");
			int intInputType = Convert.ToInt32(Console.ReadLine());
			if(intInputType != 1 && intInputType != 2)
				throw new Exception("Invalid input! Please enter either 1 or 2.");

			List<Product> lstProducts = null;
			if(intInputType == 1)
			{
				Console.WriteLine("Which test case do you want to test? 1, 2, or 3?");
				int intTestCase = Convert.ToInt32(Console.ReadLine());
				if(intTestCase < 1 || intTestCase > 3)
					throw new Exception("Invalid test case! Please enter either 1, 2 or 3.");
				
				lstProducts = GetFixedInput(intTestCase);
			}
			else
				lstProducts = GetUserInput();

			double dblTotalTax = 0;
			double dblTotalAmount = 0;

			List<Product> lstInvoice = ProcessOrders(lstProducts, ref dblTotalTax, ref dblTotalAmount);
			PrintInvoice(lstInvoice, dblTotalTax, dblTotalAmount);
		}
		catch(Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
	}
	#endregion
		
	#region Core methods
	// Method to process the actual data
	public static List<Product> ProcessOrders(List<Product> lstProducts, ref double dblTotalTax, ref double dblTotalAmount)
	{
		List<Product> lstInvoice = new List<Product>();
		
		foreach(Product objProduct in lstProducts)
		{
			int intTotalTax = CalculateTax(objProduct.Name);
			double dblFinalAmount = 0;
			double dblTaxAmount = 0;
			if(intTotalTax > 0)
			{
				double dblSumAmount = objProduct.Price * objProduct.Quantity;
				dblTaxAmount = RoundUpTo((dblSumAmount * intTotalTax) / 100);
				dblTotalTax += dblTaxAmount;
				dblFinalAmount = dblSumAmount + dblTaxAmount;
			}
			else
				dblFinalAmount = objProduct.Price * objProduct.Quantity;

			dblTotalAmount += dblFinalAmount;

			Product objInvoiceProduct = new Product(objProduct.Name, dblFinalAmount, objProduct.Quantity);
			lstInvoice.Add(objInvoiceProduct);
		}
		
		return lstInvoice;
	}
	
	// Method to display/print the invoice generated and it is called by the Main method
	public static void PrintInvoice(List<Product> lstInvoice, double dblTotalTax, double dblTotalAmount)
	{
		Console.WriteLine("\nYour invoice:\n");
		foreach(Product objProduct in lstInvoice)
			Console.WriteLine(objProduct.Quantity + " " + objProduct.Name + " at " + objProduct.Price);
		
		Console.WriteLine("Sales Taxes: " + dblTotalTax.ToString("F2") + "\nTotal: " + dblTotalAmount.ToString());
	}
	#endregion
	
	#region Utility methods
	public static double RoundUpTo(double ValueToRound)
	{
		// Rounds upto nearest 0.05;
		return Math.Ceiling(ValueToRound * 20) / 20;
	}
	
	public static bool IsImported(string ItemName)
	{
		if(ItemName.ToLower().Contains("imported"))
			return true;
		else
			return false;
	}
	
	public static bool IsExemptFromTax(string ItemName)
	{
		// More values can be added here depending on the data that we are expecting.
		List<string> lstExemptItems = new List<string>(new string[] { "book", "food", "medicine", "pills", "chocolate", "chocolates", "novel" });
		return lstExemptItems.Any(s=>ItemName.ToLower().Contains(s));
	}
	
	// Method to calculate the tax of the given item
	public static int CalculateTax(string ItemName)
	{
		int intRegularTax = 10;
		int intImportTax = 5;
		int intTotalTax = 0;
		
		bool blnShouldApplyImportTax = IsImported(ItemName);
		bool blnShouldApplyRegularTax = IsExemptFromTax(ItemName) ? false : true;
		
		if(blnShouldApplyImportTax)
			intTotalTax += intImportTax;
		
		if(blnShouldApplyRegularTax)
			intTotalTax += intRegularTax;
		
		return intTotalTax;
	}
	#endregion
}
