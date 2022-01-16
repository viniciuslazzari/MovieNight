# MovieNight 📽️
Backend for a cinema management system.

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)

# Checklist
- [X] Movies routes
- [X] Sessions routes
- [X] Tickets routes
- [X] Auth0 authentication
- [X] CORS permissions
- [X] HTTPS enabled
- [X] Database table for logs
- [X] Json success and error models

# How to use 🚀

### Testing with Insomnia 🌙
This project comes with a Insomnia collection called `InsomniaCinemaApi.json` of all the requests to the API, feel free to use.

### Auth0 authentication 🔓
Some routes are protected and need an access token to be accessed. To get the token, send a request to the **Auth** route in Insomnia and all the requests will be updated.

### Routes 🛤️

##### Movies

```
GET - http://localhost:0000/api/movies
GET - http://localhost:0000/api/movies/{id}
[PROTECTED] POST - http://localhost:0000/api/movies
[PROTECTED] PUT - http://localhost:0000/api/movies/{id}
[PROTECTED] DELETE - http://localhost:0000/api/movies/{id}
```

##### Sessions

```
GET - http://localhost:0000/api/sessions
GET - http://localhost:0000/api/sessions/{id}
[PROTECTED] POST - http://localhost:0000/api/sessions
[PROTECTED] PUT - http://localhost:0000/api/sessions/{id}
[PROTECTED] DELETE - http://localhost:0000/api/sessions/{id}
```

##### Tickets

```
GET - http://localhost:0000/api/tickets
GET - http://localhost:0000/api/tickets/{id}
[PROTECTED] POST - http://localhost:0000/api/tickets
```

### Models 🧱

##### Movie

```
{
	"id": "0440e787-eb94-49a5-ab3c-1b13aaf608e6",
	"title": "Wolf of wall street,
	"duration": 300,
	"synopsis": "About money!",
	"sessions": []
}
```

##### Session

``` 
{
    "id": "c2d0422b-c227-41a1-a6c2-300003b25e85",
	"movieId": "0440e787-eb94-49a5-ab3c-1b13aaf608e6",
	"date": "2023-01-13T23:07:00",
	"maxOccupation": 10,
	"price": 15.0,
	"tickets": []
}
```

##### Ticket

``` 
{
	"id": "641b95b8-9ba4-471c-8cc6-fd16785b1569",
	"sessionId": "c2d0422b-c227-41a1-a6c2-300003b25e85",
	"client": "Vinícius Lazzari",
	"amount": 3
}
```

## Technologies 💻
Build with:
- [ASP.NET Core 5.0](https://github.com/dotnet/aspnetcore)
- [Entity Framework Core](https://docs.microsoft.com/pt-br/ef/core/)
- [Serilog](https://github.com/serilog/serilog)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server)
- [Auth0](https://auth0.com/)

## Author 🧙
- [viniciuslazzari](https://github.com/viniciuslazzari)