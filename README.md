# Library management system

This project is a basic Library management system, it is a client server application used by librarians. The main functionality of a library management system is organizing and maintaining all the records including records of books and records of borrowers

Modules implemented in the project are:
- Book Search: Module to search for books for a book, given any combination of ISBN, title, and/or Author(s).
- Book Loans: Module to Checkout and Check-in a book by searching for the book using ISBN, Card Number or Borrower name.
- Borrower Management: Module to create new borrowers after validating the inputs.
- Fines: Module to display, update and pay fines if the book hasn’t been returned on time.

## SYSTEM ARCHITECTURE
The application has been built on Microsoft’s ASP.Net MVC 5 architecture. The database used is Microsoft SQL Server 2016. The Model-View-Controller (MVC) architectural pattern separates an application into three main components: the model, the view, and the controller. The reason for using this architecture is because it helps you create applications that separate the different aspects of the application (input logic, business logic, and UI logic), while providing a loose coupling between these elements. Stored procedures SQL Server was used as it allows modular programming and faster execution.

## DESIGN DESCISIONS AND ASSUMPTIONS
### 1. Table creation:
1. ISBN13 from the dataset was chosen as the primary key for the Book table.
2. Author_Id from the Authors table was made a computed column with function Identity(1,1) so that every tuple gets a unique id and is sequential.
3. Borrowers card_id was a six digit sequential number with 0 padded on the left.
To keep the format compatible with the existing borrower id a sequence was created which starts at 1001 and increments 1 for every tuple. It was padded with 0s on the left.
4. Borrowers address was concatenated and included in a single attribute `address`.
5. No modifications were done for the borrower ssn and phone number format.
6. Loan_id of the book loans table is a sequence which starts at 1001.
7. Date_in is assumed to null if the book has been checked out but hasn’t been returned
8. Due_date is computed as Date_out + 14 days.
9. Fine amount is calculated only when the job is run on the application.
10. Fine amount does not update every day so the values may be inconsistent if the job isn’t run.
11. Paid column of Fines table is a BIT with length 1.
12. Paid column assumes 1 to be fine paid and 0 to be unpaid.
### 2. Data Loading and normalization
a. Data loading and normalization was done using MSSQL. No external scripts or tools were used.
b. The books.csv was loaded to the Database as a table ‘ibooks’.
c. The borrowers.csv was loaded to the Database as table ‘borrowers’.
d. Books with empty authors were retained by using left join.
e. No tuples were inserted in the authors or book_authors table for book entries with null values for author in book table.
f. Book isbn and title were inserted using a select command on ibooks.
g. Author names from ibooks were split with delimiter as comma (,).
h. A tuple was created in the Authors table for every unique author name.
i. Borrower data was inserted using the temporary borrowers table.
j. Address column is a concatenation of Address, City and State of the temporary table.
### 3. Application
1. Front end of the web application uses the default bootstrap theme.
2. Navigation has been provided on the NavBar on top and on the Home page.
3. Navigation to modules such as checkout is only provided through book search.
4. On page load no data is loaded for Book search, Book Loan search.
5. Form validation is provided for the Borrower creation form by making the fields as required fields.
6. Fines is calculated on the application and a SQL Bulk Copy is executed to update it.
7. Book Checkout link is made available only to rows which haven’t been already checked-out.
8. Alert message is prompted to user if more than 3 books are tried to loan for a single borrower.
9. Alert message is prompted to user if a new borrower creation is attempted with existing SSN.
10. Display of Fines filters out previously paid fines by default.
11. Application does not allow payment of a fine for books that are not yet returned.
