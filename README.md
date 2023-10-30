# POS barcode Label Print

### Overview ###
This is a program that prints barcode labels in an easy way by connecting directly to the POS (Point of Sale) database using C#.
I generated Barcode using https://github.com/barnhill/barcodelib.


### Usage ###

First, enter the database connection information (server name, ID, password, database name)
Then select the Columns to use for the next label.
Then, when you scan the barcode, it will retrieve the code from the database and create a label.

### Example ###
1.pcAmerica
	server name / database name / user name / password
	Table : Inventory
	Column (ItemName) : ItemName
	Column (Price) : Retail_price
	Column (Barcode) : ItemNum
When you scan production barcode at itemNo textbox, search item at database	


### Support ###
If you find this or any of my software useful and decide its worth supporting.  You can do so here:  [Donate](https://www.paypal.com/donate/?business=WA7K74M5VX8MN&no_recurring=0&currency_code=USD)

### Copyright and license ###

Copyright 2007-2023 Wonyoung. Code released under the [Apache License, Version 2.0](https://github.com/wonyun70/POSAddOnBarcodeLabelPrint/blob/master/POSAddOnBarcodeLabelPrint/LICENSE).
