# MeApunto.Online
[![NET6Build](https://github.com/RarceD/MeApuntoBackend/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/RarceD/MeApuntoBackend/actions/workflows/dotnet.yml)

## What it is
This is the second version of an existing backend wrote in flask (python), the first one has performance problems and was not
scalable enough for all the clients requirements.

That's why the refactor starts on db and ends with new endpoints with more functionallity;

## Stuff that should be done: 

- Cache service to prevent getting courts and urbas information allways.
- Migration script to copy one sqlite3 to other following schemas.


## Technology used:
| System |  Technology  | 
|:-----|:--------:|
| Frontend   | **React v18** |
| Backend   |  **.NET CORE 6**   |
## Functionallity:
This app let you store booking information about your tenis/padel/yoga classes in order not to go physically to the accademy/gym to make a reservation.

## Where does it run?
| System |  Technology  | 
|:-----|:--------:|
| DB   | **SQLite** |
| Deployment   | **VPS Ubuntu 20.4** |
| Proxy   | **Nginx** |

