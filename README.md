# Test API (.NET 8 + SQLite + Swagger)

## Run
    dotnet restore
    dotnet run

Open http://localhost:5000 -> Swagger UI (no auth needed).
The SQLite file `testapi.db` is created automatically with 3 seed products.

## Endpoints (`/api/products`)
| Method  | Route                | Purpose                      |
|---------|----------------------|------------------------------|
| GET     | /api/products        | List (search, page, pageSize)|
| GET     | /api/products/{id}   | Get one                      |
| HEAD    | /api/products/{id}   | Exists check, no body        |
| POST    | /api/products        | Create                       |
| PUT     | /api/products/{id}   | Replace whole object         |
| PATCH   | /api/products/{id}   | Partial update               |
| DELETE  | /api/products/{id}   | Delete                       |
| OPTIONS | /api/products        | Allowed methods (Allow hdr)  |

## curl examples
    curl http://localhost:5000/api/products
    curl -X POST http://localhost:5000/api/products -H "Content-Type: application/json" -d '{"name":"Monitor","price":150,"quantity":5}'
    curl -X PATCH http://localhost:5000/api/products/1 -H "Content-Type: application/json" -d '{"price":899}'
    curl -X DELETE http://localhost:5000/api/products/1
    curl -I http://localhost:5000/api/products/2
    curl -X OPTIONS -i http://localhost:5000/api/products

Reset DB: stop the app and delete `testapi.db`.
