<!-- ABOUT THE PROJECT -->
## ShopsRUs API

ShopsRUs is an existing retail outlet. The systems calculates discount to customers on all their web/mobile platforms. 
They require a set of APIs to be built that provide capabilities to calculate discounts, generate the total costs and generate the 
invoices for customers

### Logic Used In Generating Invoice
* If the user is an affiliate of the store, user gets a 10% discount
* If the user is an employee of the store, user gets a 30% discount
* If the user has been a customer for over 2 years, user gets a 5% discount
* For every $100 on the bill, there would be a $5 discount (e.g. for $990, you get 
$45 as a discount)
* The percentage based discounts do not apply on groceries
* A user can get only one of the percentage based discounts on a bill

### Built With

* ASP.NET Core Web API


<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple example steps.

### Prerequisites

Things you need, to use the software and how to install them.
* .NET SDK 5
	```sh
	https://dotnet.microsoft.com/download/dotnet/5.0
	```
* MSSql Server Management Studio
	```sh
	https://aka.ms/ssmsfullsetup
	```
* Web Browser/POSTMAN/INSOMNIA
	```sh
	https://www.postman.com/downloads/
	```

### Installation
1. Clone the repository here
	```sh
	https://github.com/divinedian/ShopsRUs.git
	```
2. Open the ShopsRUs.sln using Visual Studio
3. Open Test explorer and run all test
4. Click run within visual studio

### Try out Some End Points
1. Create a discount type using the `AddDiscountType` endpoint. This will save a usertype name same as the discount type.
2. Create a customer using the `CreateCustomer` endpoint. This will return the customerid e.g. `3cc165e2-e8f5-425f-841a-b1661debba6e`, save this for the order process.
3. Create a category using the `CreateCategory` endpoint. This will return a Categoryid e.g. `3cc165e2-e8f5-425f-841a-b1661debba6e` which you should save for the process of adding items.
4. Create an item using the categoryId from step3 above.
5. Using the customerId from step2, and some of the items saved in step4, place an order using the `GetTotalInvoiceAmount` endpoint
	*A sample request body for step 5*
	```sh
	{
	  "customerId": "3cc165e2-e8f5-425f-841a-b1661debba6e",
	  "orderItems": [
		{
		  "item": "Meat",
		  "quantities": 2
		},
		{
		  "item": "Vegetable",
		  "quantities": 9
		},
		{
		  "item": "Prawns",
		  "quantities": 2
		}
	  ]
	}
	```

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "contribution".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/GetInspired`)
3. Commit your Changes (`git commit -m 'Add some Inspiration'`)
4. Push to the Branch (`git push origin feature/GetInspired`)
5. Open a Pull Request

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- LICENSE -->
## License

Distributed under the MIT License.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Diana Ekwere - [@linkedIn_handle](www.linkedin.com/in/dianaetukekwere) - ekwerediana@gmail.com

Project Link: [ShopsRUs API](https://github.com/divinedian/ShopsRUs.git)

<p align="right">(<a href="#top">back to top</a>)</p>
