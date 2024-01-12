# ShipsCinema

## Description
ShipsCinema is a dynamic theatre application that offers a diverse selection of movies, providing users with a seamless experience for account registration and login, movie selection, showtime scheduling, seat booking, checkout, and reservation management.

## Directory Structure
- **ShipsCinema Folder:** This directory contains all essential .CS and JSON files necessary to run the application.
- **ShipsCinemaTest Folder:** Reserved for Unit Tests to ensure the application's stability and performance.

## Essential Files
To ensure the proper functioning of the application, ensure the presence of the following files:
- `ShipsCinema > Datasources > ads`
- `ShipsCinema > Datasources > movies`
- `ShipsCinema > Datasources > UserAccounts`

**Important Note:** The UserAccounts file properties are set to "Copy if New." Any updates to accounts are reflected only in `ShipsCinema/Bin/Debug/Net7.0/Datasources/UserAccounts`. The original UserAccounts file remains in place to retain an admin account, even after solution cleaning or rebuilding.

## Launching the Application
To start the application, open `ShipsCinema.sln`. A default admin account is pre-configured for immediate access:

- **Email:** admin@shipscinema.com
- **Password:** Admin123

Additionally, a default user account is available for testing:

- **Email:** johndoe@mail.com
- **Password:** Password123

## Admin Account Management
For granting admin privileges to user accounts, follow these steps:

1. Login to the default admin account noted above.
2. Navigate to `Set Admin Rights` in the application.
3. Select a user to grant them admin privileges.

**Note:** Only the default admin has the authority to assign admin privileges to user accounts.

## Additional Information
- While using the application, please note that sometimes lowering the zoom below 100% may be necessary for the application to function properly. The overview of seats does not fit on the entire screen when the console is zoomed in too much, you can use `Ctrl` + `-` to zoom out.  

- Financial Reports are located in different places:
  - For Visual Studio, you can find them in the bin folder.
  - For Visual Studio Code, navigate from the Main folder to "Fuji Films> ShipsCinema," and you will find the "Financial Reports" there.

