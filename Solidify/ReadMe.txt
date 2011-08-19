Background

For each category of products, a background job running overnight generates a "hotlist" of top sellers.  
There is a hotlist for

(1) The current week
(2) The current month

The job gets the data it requires (categories for the store, a product catalog and sales data) from external
services.  These services are owned by the SAP team and cannot be modified in any way by the Web team. The
SAP team and Web team often have different names and terms for the same concept, making communication troublesome.
SAP terms are generally four or five characters long and capitalized. 

Different stores and store types have different rules about which products they will allow to appear in the 
hotlist and these rules have been subject to some change.  

The job has proven to be unreliable, buggy and difficult to change. An attempt was made to try to add some
automatic testing to the job but the results have been disappointing, some people arguing that it has made
the job even harder to understand and test.

The job has suddenly stopped working.  

Instead of simply fixing the job as it is, it has been decided to undertake a substantial refactoring to make it easier to understand, 
easier to change and easier to test.   

You have complete discretion to implement the solution as you see fit.


Business Rules

Each category on each store should have a generated toplist consisting of the top three best selling products in that category
on that store (based on quantity sold), subject to the inclusion rules.

* Distribution stores (B2B) do not include sales to Public organisations when calculating the top sellers.
* Consumer stores (B2C) do not include sales to Public or Commercial organisations when calculating the top sellers.
* Demo products are not included.
* Discontinued (End of Life) products are not included.
* Itegra does not include products priced under 1000kr.
* Norek.no and Norek.no do not include products priced under 500kr.
* The Weekly hotlist only counts sales within the current week of the year.
* The Monthly hotlist only counts sales within the current calendar month.
* Products that are out of stock are not included in the weekly hotlist.
* Products that are out of stock and will not be in stock within 1 week are not included in the Monthly hotlist.


Restrictions

The Solidify.Services project should be treated as immutable.  The service method calls and xml format are a given.
They cannot be altered.  You are free to alter the xml in any way you wish after it has been returned.

The xml structure of the hotlist is owned by the web and can, if desired, be altered.

Anything in the Solidify project can be altered.

Anything in the Solidify.Tests project can be altered.

Use of third party libraries is permitted.

Where multiple terms are used for the same concept, you can "unilaterally" decide which comes from the "Ubiquitous language".

The use of stubs and fakes are allowed where a proper implementation would require too much time and the function is not directly
related to the business requirements.

Readability and ease of understanding should take precedence over performance at all times.  This is an overnight job
and has no forseeable performance restrictions.
